param(
  [ValidateSet('full', 'sync', 'build')][string]$Mode = 'full'
)

$ErrorActionPreference = 'Stop'

$env:PATH = "$env:DEPOT_TOOLS_PATH;$env:PATH"
$env:DEPOT_TOOLS_WIN_TOOLCHAIN = '0'
$env:DEPOT_TOOLS_UPDATE = '0'
$env:GYP_MSVS_VERSION = '2022'
$env:GYP_MSVS_OVERRIDE_PATH = if ($env:GYP_MSVS_OVERRIDE_PATH) { $env:GYP_MSVS_OVERRIDE_PATH } else { 'C:\BuildTools' }
git config --global --add safe.directory C:/depot_tools
git config --global --add safe.directory C:/src
git config --global --add safe.directory C:/src/src

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

function Apply-PatchBestEffort {
  param([Parameter(Mandatory = $true)][string]$PatchPath)

  & git apply --ignore-whitespace --whitespace=nowarn $PatchPath
  if ($LASTEXITCODE -eq 0) {
    return
  }

  Write-Warning "Patch did not apply cleanly, retrying with --reject: $PatchPath"
  & git apply --ignore-whitespace --whitespace=nowarn --reject $PatchPath
  if ($LASTEXITCODE -ne 0) {
    Write-Warning "Patch still has rejects; continuing with partial application: $PatchPath"
  }
}

function Resolve-BuildToolsPath {
  $vsWhere = 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe'
  if (Test-Path $vsWhere) {
    $installPath = (& $vsWhere -latest -products * -property installationPath | Select-Object -First 1)
    if ($installPath) {
      return $installPath.Trim()
    }
  }

  if (Test-Path 'C:\BuildTools\Common7\Tools\VsDevCmd.bat') {
    return 'C:\BuildTools'
  }

  throw 'Failed to locate Visual Studio Build Tools installation.'
}

function Resolve-WindowsSdkEnvironment {
  $sdkBase = Join-Path ${env:ProgramFiles(x86)} 'Windows Kits\10'
  $sdkIncludeRoot = Join-Path $sdkBase 'Include'
  if (!(Test-Path $sdkIncludeRoot)) {
    throw "Windows SDK include root not found at $sdkIncludeRoot"
  }

  $latestSdk = Get-ChildItem $sdkIncludeRoot -Directory |
    Where-Object {
      (Test-Path (Join-Path $_.FullName 'um')) -and
      (Test-Path (Join-Path $_.FullName 'ucrt')) -and
      (Test-Path (Join-Path $_.FullName 'shared'))
    } |
    Sort-Object Name -Descending |
    Select-Object -First 1
  if (!$latestSdk) {
    throw "No complete Windows SDK version folders found under $sdkIncludeRoot"
  }

  $env:WINDOWSSDKDIR = "$sdkBase\"
  $env:WindowsSdkDir = $env:WINDOWSSDKDIR
  $env:WindowsSDKVersion = "$($latestSdk.Name)\"
  $env:UCRTVersion = $latestSdk.Name
}

function Test-DebugVcrtAvailable {
  param([Parameter(Mandatory = $true)][string]$Cpu)

  $runtimeDir = if ($Cpu -eq 'x86') { "$env:WINDIR\SysWOW64" } else { "$env:WINDIR\System32" }
  return (Test-Path (Join-Path $runtimeDir 'msvcp140d.dll')) -and (Test-Path (Join-Path $runtimeDir 'vcruntime140d.dll'))
}

function Ensure-WindowsSdkDebuggersFiles {
  $sdkDebuggersRoot = Join-Path $env:WINDOWSSDKDIR 'Debuggers'
  $copies = @(
    @{ Cpu = 'x86'; SourceDir = "$env:WINDIR\SysWOW64" },
    @{ Cpu = 'x64'; SourceDir = "$env:WINDIR\System32" }
  )

  foreach ($entry in $copies) {
    $targetDir = Join-Path $sdkDebuggersRoot $entry.Cpu
    New-Item -ItemType Directory -Force $targetDir | Out-Null
    foreach ($dll in @('dbghelp.dll', 'dbgcore.dll')) {
      $source = Join-Path $entry.SourceDir $dll
      if (Test-Path $source) {
        Copy-Item -Force $source (Join-Path $targetDir $dll)
      }
    }
  }
}

function Ensure-NinjaAvailable {
  if (Get-Command ninja.exe -ErrorAction SilentlyContinue) {
    return
  }

  Write-Host 'Installing Ninja because depot_tools did not provision it.'
  Invoke-NativeCommand choco @('install', '-y', 'ninja')
}

function Ensure-VcVarsAllShim {
  param([Parameter(Mandatory = $true)][string]$BuildToolsPath)

  $legacyVcVarsAll = Join-Path $BuildToolsPath 'VC\vcvarsall.bat'
  $auxVcVarsAll = Join-Path $BuildToolsPath 'VC\Auxiliary\Build\vcvarsall.bat'
  if (Test-Path $legacyVcVarsAll) {
    return
  }

  if (Test-Path $auxVcVarsAll) {
    New-Item -ItemType Directory -Force (Split-Path $legacyVcVarsAll -Parent) | Out-Null
    $shim = @"
@echo off
set "_arch=%1"
set "_winsdk=10.0.22621.0"
if not "%WindowsSDKVersion%"=="" set "_winsdk=%WindowsSDKVersion:\=%"
if "%_arch%"=="" set "_arch=amd64"
call "$auxVcVarsAll" %_arch% %_winsdk%
"@
    Set-Content -Path $legacyVcVarsAll -Value $shim -Encoding Ascii
    return
  }

  $candidate = Get-ChildItem -Path (Join-Path $BuildToolsPath 'VC\Tools\MSVC') -Filter vcvarsall.bat -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1
  if ($candidate) {
    New-Item -ItemType Directory -Force (Split-Path $auxVcVarsAll -Parent) | Out-Null
    $shim = "@echo off`r`ncall `"$($candidate.FullName)`" %*`r`n"
    Set-Content -Path $auxVcVarsAll -Value $shim -Encoding Ascii
    return
  }

  $vsDevCmd = Join-Path $BuildToolsPath 'Common7\Tools\VsDevCmd.bat'
  if (!(Test-Path $vsDevCmd)) {
    throw "Unable to locate vcvarsall.bat under $BuildToolsPath and VsDevCmd.bat is missing at $vsDevCmd."
  }

  New-Item -ItemType Directory -Force (Split-Path $auxVcVarsAll -Parent) | Out-Null
  $shim = @"
@echo off
set _target=x64
set "_winsdk=10.0.22621.0"
if not "%WindowsSDKVersion%"=="" set "_winsdk=%WindowsSDKVersion:\=%"
if /I "%1"=="amd64_x86" set _target=x86
if /I "%1"=="amd64_arm" set _target=arm
if /I "%1"=="amd64_arm64" set _target=arm64
call "$vsDevCmd" -host_arch=x64 -arch=%_target% -winsdk=%_winsdk%
"@
  Set-Content -Path $auxVcVarsAll -Value $shim -Encoding Ascii
}

if ($Mode -eq 'full' -or $Mode -eq 'sync') {
  Set-Location C:\src
  if (!(Test-Path C:\src\src\.git)) {
    Invoke-NativeCommand "$env:DEPOT_TOOLS_PATH\fetch.bat" @('--nohooks', 'webrtc')
  }

  Set-Location C:\src\src
  $branchRef = if ($env:WEBRTC_BRANCH) { $env:WEBRTC_BRANCH } else { 'refs/branch-heads/7778' }
  $branchName = if ($env:WEBRTC_BRANCH_NAME) { $env:WEBRTC_BRANCH_NAME } else { 'webrtcnet_7778' }
  Invoke-NativeCommand git @('fetch', 'origin', $branchRef)
  Invoke-NativeCommand git @('checkout', '-B', $branchName, 'FETCH_HEAD')
  $syncArgs = @('sync', '--with_branch_heads')
  if ($env:FAST_DEV_SYNC -ne 'true') {
    $syncArgs += '-D'
  }
  Invoke-NativeCommand gclient $syncArgs
}

if ($Mode -eq 'full' -or $Mode -eq 'build') {
  if (!(Test-Path C:\src\src\.git)) {
    throw 'WebRTC source checkout is missing at C:\src\src. Run sync mode first.'
  }

  $buildToolsPath = Resolve-BuildToolsPath
  $vsDevCmd = Join-Path $buildToolsPath 'Common7\Tools\VsDevCmd.bat'
  if (!(Test-Path $vsDevCmd)) {
    throw "VsDevCmd.bat was not found at $vsDevCmd"
  }
  if (!(Test-Path (Join-Path $buildToolsPath 'MSBuild\Current\Bin\MSBuild.exe'))) {
    throw "MSBuild.exe was not found under $buildToolsPath"
  }
  Ensure-VcVarsAllShim $buildToolsPath
  $env:vs2022_install = $buildToolsPath
  Resolve-WindowsSdkEnvironment
  Ensure-WindowsSdkDebuggersFiles
  Ensure-NinjaAvailable

  Set-Location C:\src\src\third_party\googletest\src
  Apply-PatchBestEffort 'C:\patches\0001-Compile-for-C-CLI.patch'
  Set-Location C:\src\src
  Invoke-NativeCommand gclient @('runhooks')
  Apply-PatchBestEffort 'C:\patches\0001-compile-for-windows-using-dynamic-c-library.patch'
  # Force dynamic CRT (/MD, /MDd) for all targets so the static lib is
  # compatible with C++/CLI projects that require the shared MSVC runtime.
  $winBuildGnPath = 'C:\src\src\build\config\win\BUILD.gn'
  $winBuildGn = Get-Content -Raw $winBuildGnPath
  if ($winBuildGn.Contains('configs = [ ":static_crt" ]')) {
    $winBuildGn = $winBuildGn.Replace(
      'configs = [ ":static_crt" ]',
      'configs = [ ":dynamic_crt" ]')
    Set-Content -Path $winBuildGnPath -Value $winBuildGn -Encoding Ascii
  }
  $peerConnectionHeaderPath = 'C:\src\src\api\peer_connection_interface.h'
  $peerConnectionHeader = Get-Content -Raw $peerConnectionHeaderPath
  if ($peerConnectionHeader.Contains('PeerConnectionFactoryDependencies&&) = default;')) {
    $peerConnectionHeader = $peerConnectionHeader.Replace(
      'PeerConnectionFactoryDependencies&&) = default;',
      'PeerConnectionFactoryDependencies&&);')
    Set-Content -Path $peerConnectionHeaderPath -Value $peerConnectionHeader -Encoding Ascii
  }
  $peerConnectionCcPath = 'C:\src\src\api\peer_connection_interface.cc'
  $peerConnectionCc = Get-Content -Raw $peerConnectionCcPath
  if (!$peerConnectionCc.Contains('PeerConnectionFactoryDependencies::operator=(')) {
    $peerConnectionCc = $peerConnectionCc.Replace(
      "PeerConnectionFactoryDependencies::PeerConnectionFactoryDependencies(`r`n    PeerConnectionFactoryDependencies&&) = default;`r`n`r`nPeerConnectionFactoryDependencies::~PeerConnectionFactoryDependencies() =",
      "PeerConnectionFactoryDependencies::PeerConnectionFactoryDependencies(`r`n    PeerConnectionFactoryDependencies&&) = default;`r`n`r`nPeerConnectionFactoryDependencies& PeerConnectionFactoryDependencies::operator=(`r`n    PeerConnectionFactoryDependencies&&) = default;`r`n`r`nPeerConnectionFactoryDependencies::~PeerConnectionFactoryDependencies() =")
    Set-Content -Path $peerConnectionCcPath -Value $peerConnectionCc -Encoding Ascii
  }
  $vsToolchainPath = 'C:\src\src\build\vs_toolchain.py'
  (Get-Content -Raw $vsToolchainPath).
    Replace("('api-ms-win-downlevel-kernel32-l2-1-0.dll', False)", "('api-ms-win-downlevel-kernel32-l2-1-0.dll', True)").
    Replace("('api-ms-win-eventing-provider-l1-1-0.dll', False)", "('api-ms-win-eventing-provider-l1-1-0.dll', True)") |
    Set-Content -Encoding Ascii $vsToolchainPath

  $configs = @(
    @{ Config = 'Debug';   Platform = 'Win32'; Cpu = 'x86'; IsDebug = 'true';  EnableIteratorDebugging = 'true';  ExtraArgs = @('use_custom_libcxx=false') },
    @{ Config = 'Release'; Platform = 'Win32'; Cpu = 'x86'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; ExtraArgs = @('use_custom_libcxx=false') },
    @{ Config = 'Debug';   Platform = 'x64';   Cpu = 'x64'; IsDebug = 'true';  EnableIteratorDebugging = 'true';  ExtraArgs = @('use_custom_libcxx=false') },
    @{ Config = 'Release'; Platform = 'x64';   Cpu = 'x64'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; ExtraArgs = @('use_custom_libcxx=false') }
  )
  $configs = $configs | Where-Object {
    if ($_['IsDebug'] -ne 'true') {
      return $true
    }

    if (Test-DebugVcrtAvailable $_['Cpu']) {
      return $true
    }

    Write-Warning "Skipping $($_['Config'])|$($_['Platform']) because debug VC runtime DLLs are not available."
    return $false
  }

  foreach ($config in $configs) {
    $outDir = Join-Path C:\src\src\out ("{0}\{1}" -f $config.Config, $config.Platform)
    New-Item -ItemType Directory -Force $outDir | Out-Null
    $args = @(
      "target_cpu=`"$($config.Cpu)`"",
      "is_debug=$($config.IsDebug)",
      'is_component_build=false',
      "enable_iterator_debugging=$($config.EnableIteratorDebugging)",
      'use_lld=false',
      'rtc_include_tests=false',
      'rtc_build_tools=false',
      'rtc_build_examples=false',
      'enable_libaom=false',
      'rtc_enable_protobuf=false'
    ) + $config.ExtraArgs
    $gnArgs = $args -join ' '
    Set-Content -Path (Join-Path $outDir 'args.gn') -Value $gnArgs -Encoding Ascii
    $vsArch = if ($config.Cpu -eq 'x86') { 'x86' } else { 'x64' }
    $sdkEnvCmds = @(
      "set `"WINDOWSSDKDIR=$($env:WINDOWSSDKDIR)`"",
      "set `"WindowsSdkDir=$($env:WindowsSdkDir)`"",
      "set `"WindowsSDKVersion=$($env:WindowsSDKVersion)`"",
      "set `"UCRTVersion=$($env:UCRTVersion)`""
    )
    $sdkEnvCmd = $sdkEnvCmds -join ' && '
    $command = "call `"$vsDevCmd`" -arch=$vsArch -host_arch=x64 && $sdkEnvCmd && cd /d `"C:\src\src`" && gn gen --ide=vs2022 `"$outDir`" --filters=//:webrtc && autoninja -C `"$outDir`""
    Invoke-NativeCommand cmd @('/S', '/C', $command)
  }

  New-Item -ItemType Directory -Force C:\artifacts\src | Out-Null
  New-Item -ItemType Directory -Force C:\artifacts\out | Out-Null
  robocopy C:\src\src C:\artifacts\src /E /R:1 /W:1 /NFL /NDL /NJH /NJS /NP | Out-Null
  if ($LASTEXITCODE -ge 8) {
    throw "robocopy C:\src\src failed with exit code $LASTEXITCODE"
  }
  robocopy C:\src\src\out C:\artifacts\out /E /R:1 /W:1 /NFL /NDL /NJH /NJS /NP | Out-Null
  if ($LASTEXITCODE -ge 8) {
    throw "robocopy C:\src\src\out failed with exit code $LASTEXITCODE"
  }
}
