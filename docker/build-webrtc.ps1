#requires -Version 7.0

[CmdletBinding()]
param(
  [ValidateSet('full', 'sync', 'build')][string]$Mode = 'full'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
if (Get-Variable -Name PSNativeCommandUseErrorActionPreference -ErrorAction Ignore) {
  $PSNativeCommandUseErrorActionPreference = $true
}

$env:PATH = "$env:DEPOT_TOOLS_PATH;$env:PATH"
$env:GYP_MSVS_VERSION = if ($env:MSVS_VERSION) { $env:MSVS_VERSION } else { '2026' }
$env:GYP_MSVS_OVERRIDE_PATH = if ($env:GYP_MSVS_OVERRIDE_PATH) { $env:GYP_MSVS_OVERRIDE_PATH } else { $env:BUILDTOOLS_PATH }
$webrtcRoot = $env:WEBRTC_WORK_ROOT
$webrtcSrc = Join-Path $webrtcRoot 'src'
$webrtcOutRoot = Join-Path $webrtcSrc 'out'
$webrtcGitPath = Join-Path $webrtcSrc '.git'
git config --global --add safe.directory $webrtcSrc

function Invoke-NativeCommand {
  param(
    [Parameter(Mandatory = $true)][string]$FilePath,
    [Parameter()][string[]]$ArgumentList = @()
  )

  & $FilePath @ArgumentList
  if ($LASTEXITCODE -ne 0) {
    throw "Command failed with exit code ${LASTEXITCODE}: $FilePath $($ArgumentList -join ' ')"
  }
}

function Apply-GitPatch {
  param(
    [Parameter(Mandatory = $true)][string]$RepositoryPath,
    [Parameter(Mandatory = $true)][string]$PatchPath
  )

  if (-not (Test-Path -LiteralPath $PatchPath -PathType Leaf)) {
    throw "Patch file was not found: $PatchPath"
  }

  Push-Location -LiteralPath $RepositoryPath
  try {
    & git apply --check --ignore-space-change --ignore-whitespace $PatchPath
    if ($LASTEXITCODE -eq 0) {
      Invoke-NativeCommand git @('am', '--3way', $PatchPath)
      return
    }

    & git apply --check --reverse --ignore-space-change --ignore-whitespace $PatchPath
    if ($LASTEXITCODE -eq 0) {
      Write-Host "Patch already applied: $PatchPath"
      return
    }

    throw "Patch did not apply cleanly and does not appear to be already applied: $PatchPath"
  }
  finally {
    Pop-Location
  }
}

function Resolve-BuildToolsPath {
  if ($env:BUILDTOOLS_PATH) {
    $vsDevCmdPath = Join-Path $env:BUILDTOOLS_PATH 'Common7\Tools\VsDevCmd.bat'
    if (Test-Path -LiteralPath $vsDevCmdPath -PathType Leaf) {
      return $env:BUILDTOOLS_PATH
    }
  }

  $vsWhere = 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe'
  if (Test-Path -LiteralPath $vsWhere -PathType Leaf) {
    $installPath = (& $vsWhere -latest -products * -property installationPath | Select-Object -First 1)
    if ($installPath) {
      $env:BUILDTOOLS_PATH = $installPath.Trim()
      Write-Verbose "Found Visual Studio installation at $env:BUILDTOOLS_PATH"
      return $env:BUILDTOOLS_PATH
    }
  }

  throw 'Failed to locate Visual Studio Build Tools installation.'
}

function Get-VcVarsArchForTargetCpu {
  param(
    [Parameter(Mandatory = $true)][string]$TargetCpu
  )

  # The build pipeline is run on a x64 host. But if you want to run this script locally and are using ARM64 or x86,
  # this function will need to be modified.
  switch ($TargetCpu) {
    # Explicit host->target mappings keep vcvarsall aligned with gn target_cpu.
    'x86' { return 'x64_x86' }
    'x64' { return 'x64' }
    'arm64' { return 'x64_arm64' }
    'arm' {
      throw "target_cpu='arm' is not supported by the installed vcvarsall.bat (no x64_arm/x86_arm argument). Remove ARM configs or use a toolchain that supports ARM32."
    }
    default { throw "Unsupported target CPU '$TargetCpu'" }
  }
}

if ($Mode -in @('full', 'sync')) {
  # Sync happens in 3 steps. Fetch, Checkout, and Sync.
  # Step 1 - Fetch
  New-Item -ItemType Directory -Force -Path $webrtcRoot | Out-Null
  Push-Location -LiteralPath $webrtcRoot
  try {
    if (-not (Test-Path -LiteralPath $webrtcGitPath)) {
      Invoke-NativeCommand "$env:DEPOT_TOOLS_PATH\fetch.bat" @('--nohooks', 'webrtc')
    }

    # Step 2 - Checkout
    Push-Location -LiteralPath $webrtcSrc
    try {
      if ($env:WEBRTC_BRANCH) {
        $branchRef = "refs/branch-heads/$($env:WEBRTC_BRANCH)"
        $branchName = if ($env:WEBRTC_BRANCH_NAME) { $env:WEBRTC_BRANCH_NAME } else { "webrtcnet_$($env:WEBRTC_BRANCH)" }
        Invoke-NativeCommand git @('fetch', 'origin', $branchRef)
        Invoke-NativeCommand git @('checkout', '-B', $branchName, 'FETCH_HEAD')
      }

      # Step 3 - Sync
      $syncArgs = @('sync', '--with_branch_heads')
      if ($env:FAST_DEV_SYNC -ne 'true') { $syncArgs += '-D' }
      Invoke-NativeCommand gclient $syncArgs
    }
    finally {
      Pop-Location
    }
  }
  finally {
    Pop-Location
  }
}

if ($Mode -in @('full', 'build')) {
  if (-not (Test-Path -LiteralPath $webrtcGitPath)) {
    throw "WebRTC source checkout is missing at $webrtcSrc. Run sync mode first."
  }

  $buildToolsPath = Resolve-BuildToolsPath
  $env:vs2026_install = $buildToolsPath
  $vcVarsAll = Join-Path $buildToolsPath 'VC\Auxiliary\Build\vcvarsall.bat'
  if (-not (Test-Path -LiteralPath $vcVarsAll -PathType Leaf)) {
    throw "vcvarsall.bat was not found at $vcVarsAll"
  }
  $msBuildPath = Join-Path $buildToolsPath 'MSBuild\Current\Bin\MSBuild.exe'
  if (-not (Test-Path -LiteralPath $msBuildPath -PathType Leaf)) {
    throw "MSBuild.exe was not found under $buildToolsPath"
  }

  Push-Location -LiteralPath $webrtcSrc
  try {
    Invoke-NativeCommand gclient @('runhooks')

    $patchFiles = Get-ChildItem -LiteralPath 'C:\build\patches' -File |
      Where-Object { $_.Name -match '^[0-9]{4}-.+\.patch$' } |
      Sort-Object Name
    foreach ($patchFile in $patchFiles) {
      Apply-GitPatch -RepositoryPath $webrtcSrc -PatchPath $patchFile.FullName
    }

    # Force dynamic CRT (/MD, /MDd) for all targets so the static lib is
    # compatible with C++/CLI projects that require the shared MSVC runtime.
    $winBuildGnPath = Join-Path $webrtcSrc 'build\config\win\BUILD.gn'
    $winBuildGn = Get-Content -Raw -LiteralPath $winBuildGnPath
    if ($winBuildGn.Contains('configs = [ ":static_crt" ]')) {
      $winBuildGn = $winBuildGn.Replace(
        'configs = [ ":static_crt" ]',
        'configs = [ ":dynamic_crt" ]')
      Set-Content -LiteralPath $winBuildGnPath -Value $winBuildGn -Encoding Ascii
    }

    $vsToolchainPath = Join-Path $webrtcSrc 'build\vs_toolchain.py'
    (Get-Content -Raw -LiteralPath $vsToolchainPath).
      Replace("('api-ms-win-downlevel-kernel32-l2-1-0.dll', False)", "('api-ms-win-downlevel-kernel32-l2-1-0.dll', True)").
      Replace("('api-ms-win-eventing-provider-l1-1-0.dll', False)", "('api-ms-win-eventing-provider-l1-1-0.dll', True)") |
      Set-Content -Encoding Ascii -LiteralPath $vsToolchainPath

    $configs = @(
      @{ Config = 'Debug';   Platform = 'Win32'; Cpu = 'x86'; IsDebug = 'true';  EnableIteratorDebugging = 'true';  ExtraArgs = @() },
      @{ Config = 'Release'; Platform = 'Win32'; Cpu = 'x86'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; ExtraArgs = @() },
      @{ Config = 'Debug';   Platform = 'x64';   Cpu = 'x64'; IsDebug = 'true';  EnableIteratorDebugging = 'true';  ExtraArgs = @() },
      @{ Config = 'Release'; Platform = 'x64';   Cpu = 'x64'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; ExtraArgs = @() },
      # ARM64 disables WGC because it can pull desktop-capture paths that reference
      # SharedXDisplay::~SharedXDisplay and fail interop linking with packaged webrtc.lib.
      @{ Config = 'Debug';   Platform = 'ARM64'; Cpu = 'arm64'; IsDebug = 'true';  EnableIteratorDebugging = 'false';  ExtraArgs = @('rtc_enable_win_wgc=false') }, #iterator debugging fails for ARM64 builds.
      @{ Config = 'Release'; Platform = 'ARM64'; Cpu = 'arm64'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; ExtraArgs = @('rtc_enable_win_wgc=false') }
    )

    foreach ($config in $configs) {
      $outDir = Join-Path $webrtcOutRoot "$($config.Config)\$($config.Platform)"
      New-Item -ItemType Directory -Force -Path $outDir | Out-Null
      $args = @(
        "target_cpu=`"$($config.Cpu)`"",
        'use_custom_libcxx=false',
        'libcxx_is_shared=true',
        "is_debug=$($config.IsDebug)",
        "enable_iterator_debugging=$($config.EnableIteratorDebugging)",
        'use_lld=false',
        'is_component_build=false',
        'clang_use_chrome_plugins=false',
        'rtc_include_tests=false',
        'rtc_build_tools=false',
        'rtc_build_examples=false',
        'rtc_enable_symbol_export=true'
        'rtc_enable_protobuf=false',
        'enable_libaom=false'
      ) + $config.ExtraArgs
      $gnArgs = $args -join ' '
      Set-Content -LiteralPath (Join-Path $outDir 'args.gn') -Value $gnArgs -Encoding Ascii
      $vsArch = Get-VcVarsArchForTargetCpu -TargetCpu $config.Cpu
      $command = "call `"$vcVarsAll`" $vsArch && cd /d `"$webrtcSrc`" && gn gen --ide=vs `"$outDir`" --filters=//:webrtc && autoninja -C `"$outDir`""
      Invoke-NativeCommand cmd @('/S', '/C', $command)
    }
  }
  finally {
    Pop-Location
  }
}
