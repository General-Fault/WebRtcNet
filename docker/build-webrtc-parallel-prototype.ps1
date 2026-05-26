param()

$ErrorActionPreference = 'Stop'

$webrtcRoot = $env:WEBRTC_WORK_ROOT
$webrtcSrc = Join-Path $webrtcRoot 'src'
$webrtcOutRoot = Join-Path $webrtcSrc 'out'
$parallelConfigs = if ($env:WEBRTC_PARALLEL_CONFIGS) { [int]$env:WEBRTC_PARALLEL_CONFIGS } else { 4 }
$ninjaJobsPerConfig = if ($env:WEBRTC_NINJA_JOBS_PER_CONFIG) { [int]$env:WEBRTC_NINJA_JOBS_PER_CONFIG } else { 0 }

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

function Resolve-BuildToolsPath {
  if ($env:BUILDTOOLS_PATH -and (Test-Path "$($env:BUILDTOOLS_PATH)\Common7\Tools\VsDevCmd.bat")) {
    return $env:BUILDTOOLS_PATH
  }

  $vsWhere = 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe'
  if (Test-Path $vsWhere) {
    $installPath = (& $vsWhere -latest -products * -property installationPath | Select-Object -First 1)
    if ($installPath) {
      return $installPath.Trim()
    }
  }

  throw 'Failed to locate Visual Studio Build Tools installation.'
}

function Apply-SharedWebRtcMutations {
  Set-Location $webrtcSrc
  Invoke-NativeCommand gclient @('runhooks')

  $winBuildGnPath = Join-Path $webrtcSrc 'build\config\win\BUILD.gn'
  $winBuildGn = Get-Content -Raw $winBuildGnPath
  if ($winBuildGn.Contains('configs = [ ":static_crt" ]')) {
    $winBuildGn = $winBuildGn.Replace(
      'configs = [ ":static_crt" ]',
      'configs = [ ":dynamic_crt" ]')
    Set-Content -Path $winBuildGnPath -Value $winBuildGn -Encoding Ascii
  }

  $vsToolchainPath = Join-Path $webrtcSrc 'build\vs_toolchain.py'
  (Get-Content -Raw $vsToolchainPath).
    Replace("('api-ms-win-downlevel-kernel32-l2-1-0.dll', False)", "('api-ms-win-downlevel-kernel32-l2-1-0.dll', True)").
    Replace("('api-ms-win-eventing-provider-l1-1-0.dll', False)", "('api-ms-win-eventing-provider-l1-1-0.dll', True)") |
    Set-Content -Encoding Ascii $vsToolchainPath
}

function Emit-ProgressLine {
  param(
    [Parameter(Mandatory = $true)][string]$Label,
    [Parameter(Mandatory = $true)][string]$Line
  )

  $progressMatch = [regex]::Match($Line, '^\[(\d+)/(\d+)\]\s*(.*)$')
  if ($progressMatch.Success) {
    Write-Output ("PROGRESS|{0}|{1}|{2}|{3}" -f $Label, $progressMatch.Groups[1].Value, $progressMatch.Groups[2].Value, $progressMatch.Groups[3].Value)
    return
  }

  Write-Output ("OUTPUT|{0}|{1}" -f $Label, $Line)
}

if (!(Test-Path (Join-Path $webrtcSrc '.git'))) {
  throw "WebRTC source checkout is missing at $webrtcSrc."
}

$buildToolsPath = Resolve-BuildToolsPath
$env:vs2026_install = $buildToolsPath
$vcVarsAll = Join-Path $buildToolsPath 'VC\Auxiliary\Build\vcvarsall.bat'
if (!(Test-Path $vcVarsAll)) {
  throw "vcvarsall.bat was not found at $vcVarsAll"
}

Apply-SharedWebRtcMutations

$configs = @(
  @{ Config = 'Debug';   Platform = 'Win32'; Cpu = 'x86'; IsDebug = 'true';  EnableIteratorDebugging = 'true';  VsArch = 'x64_x86' },
  @{ Config = 'Release'; Platform = 'Win32'; Cpu = 'x86'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; VsArch = 'x64_x86' },
  @{ Config = 'Debug';   Platform = 'x64';   Cpu = 'x64'; IsDebug = 'true';  EnableIteratorDebugging = 'true';  VsArch = 'x64' },
  @{ Config = 'Release'; Platform = 'x64';   Cpu = 'x64'; IsDebug = 'false'; EnableIteratorDebugging = 'false'; VsArch = 'x64' }
)

$configs | ForEach-Object -Parallel {
  $config = $_
  $label = '{0}-{1}' -f $config.Config, $config.Platform
  $outDir = Join-Path $using:webrtcOutRoot ("{0}\{1}" -f $config.Config, $config.Platform)

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
    'clang_use_chrome_plugins=false',
    'rtc_enable_protobuf=false',
    'use_custom_libcxx=false'
  )
  Set-Content -Path (Join-Path $outDir 'args.gn') -Value ($args -join ' ') -Encoding Ascii

  Write-Output ("STATE|{0}|starting" -f $label)
  $ninjaJobsArg = if ($using:ninjaJobsPerConfig -gt 0) { " -j $using:ninjaJobsPerConfig" } else { '' }
  $command = "call `"$using:vcVarsAll`" $($config.VsArch) && cd /d `"$using:webrtcSrc`" && gn gen --ide=vs `"$outDir`" --filters=//:webrtc && autoninja -C `"$outDir`"$ninjaJobsArg"
  & cmd.exe /S /C $command 2>&1 | ForEach-Object {
    if ($null -ne $_) {
      $line = $_.ToString()
      $progressMatch = [regex]::Match($line, '^\[(\d+)/(\d+)\]\s*(.*)$')
      if ($progressMatch.Success) {
        Write-Output ("PROGRESS|{0}|{1}|{2}|{3}" -f $label, $progressMatch.Groups[1].Value, $progressMatch.Groups[2].Value, $progressMatch.Groups[3].Value)
      } elseif ($line.Length -gt 0) {
        Write-Output ("OUTPUT|{0}|{1}" -f $label, $line)
      }
    }
  }
  if ($LASTEXITCODE -ne 0) {
    throw "Parallel WebRTC build failed for $label (exit code $LASTEXITCODE)"
  }
  Write-Output ("STATE|{0}|done" -f $label)
} -ThrottleLimit $parallelConfigs
