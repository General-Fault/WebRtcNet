#requires -Version 7.0

param(
  [Alias('debug')][switch]$IncludeDebug
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
if (Get-Variable -Name PSNativeCommandUseErrorActionPreference -ErrorAction Ignore) {
  $PSNativeCommandUseErrorActionPreference = $true
}

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

function Resolve-BuildToolsRoot {
  $defaultBuildTools = 'C:\BuildTools'
  $vsDevCmdInDefault = Join-Path $defaultBuildTools 'Common7\Tools\VsDevCmd.bat'
  if (Test-Path -LiteralPath $vsDevCmdInDefault -PathType Leaf) {
    return $defaultBuildTools
  }

  $vsWhere = 'C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe'
  if (Test-Path -LiteralPath $vsWhere -PathType Leaf) {
    $installPath = (& $vsWhere -latest -products * -property installationPath | Select-Object -First 1)
    if ($installPath) {
      return $installPath.Trim()
    }
  }

  throw "Failed to locate Visual Studio Build Tools. Expected $vsDevCmdInDefault."
}

function Get-BuildConfigurations {
  param([switch]$IncludeDebug)

  $configs = @('Release')
  if ($IncludeDebug) {
    $configs += 'Debug'
  }

  return $configs
}

function Invoke-InteropBuild {
  param(
    [Parameter(Mandatory = $true)][string]$VsDevCmdPath,
    [Parameter(Mandatory = $true)][string]$MsBuildPath,
    [Parameter(Mandatory = $true)][string]$ProjectPath,
    [Parameter(Mandatory = $true)][string]$Configuration,
    [Parameter(Mandatory = $true)][string]$DotnetRoot,
    [Parameter(Mandatory = $true)][string]$DotnetExe,
    [Parameter(Mandatory = $true)][string]$MsBuildSdksPath
  )

  $command = "`"$VsDevCmdPath`" -arch=x64 -host_arch=x64 && " +
    "set `"DOTNET_ROOT=$DotnetRoot`" && " +
    "set `"DOTNET_HOST_PATH=$DotnetExe`" && " +
    "set `"DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR=$DotnetRoot`" && " +
    "set `"MSBuildSDKsPath=$MsBuildSdksPath`" && " +
    "set `"MSBuildEnableWorkloadResolver=false`" && " +
    "`"$MsBuildPath`" $ProjectPath /p:Configuration=$Configuration /p:Platform=x64 /p:BuildProjectReferences=false /m"

  Invoke-NativeCommand -FilePath 'cmd' -ArgumentList @('/S', '/C', $command)
}

New-Item -ItemType Directory -Force -Path 'C:\out\nuget' | Out-Null

$buildToolsRoot = Resolve-BuildToolsRoot
$vsDevCmd = Join-Path $buildToolsRoot 'Common7\Tools\VsDevCmd.bat'
$msbuild = Join-Path $buildToolsRoot 'MSBuild\Current\Bin\MSBuild.exe'
if (-not (Test-Path -LiteralPath $msbuild -PathType Leaf)) {
  throw "MSBuild.exe was not found at $msbuild"
}

$dotnetRoot = Join-Path ${env:ProgramFiles} 'dotnet'
$dotnetExe = Join-Path $dotnetRoot 'dotnet.exe'
$dotnetSdkRoot = Join-Path $dotnetRoot 'sdk'
if (-not (Test-Path -LiteralPath $dotnetExe -PathType Leaf)) {
  throw "dotnet.exe was not found at $dotnetExe"
}
if (-not (Test-Path -LiteralPath $dotnetSdkRoot -PathType Container)) {
  throw "dotnet SDK root was not found at $dotnetSdkRoot"
}

$minimumSdkVersion = [Version]'10.0.300'
$latestSdk = Get-ChildItem -LiteralPath $dotnetSdkRoot -Directory |
  Where-Object { $_.Name -notmatch '-' } |
  Where-Object {
    $parsedVersion = $null
    [Version]::TryParse($_.Name, [ref]$parsedVersion) -and $parsedVersion -ge $minimumSdkVersion
  } |
  Sort-Object { [Version]$_.Name } -Descending |
  Select-Object -First 1
if (-not $latestSdk) {
  throw "No .NET SDK >= $minimumSdkVersion was found under $dotnetSdkRoot"
}

$msbuildSdksPath = Join-Path $latestSdk.FullName 'Sdks'
if (-not (Test-Path -LiteralPath $msbuildSdksPath -PathType Container)) {
  throw "MSBuild SDKs path was not found at $msbuildSdksPath"
}

$configurations = Get-BuildConfigurations -IncludeDebug:$IncludeDebug

Invoke-NativeCommand -FilePath 'dotnet' -ArgumentList @('restore', 'WebRtcNet.slnx')

foreach ($configuration in $configurations) {
  Invoke-NativeCommand -FilePath 'dotnet' -ArgumentList @(
    'build',
    'WebRtcNet.Api\WebRtcNet.Api.csproj',
    '-c', $configuration,
    '-p:Platform=x64',
    '--no-restore'
  )

  foreach ($targetFramework in @('net10.0', 'net48')) {
    $apiOut = Join-Path "WebRtcNet.Api\bin\x64\$configuration\$targetFramework" 'WebRtcNet.Api.dll'
    if (-not (Test-Path -LiteralPath $apiOut -PathType Leaf)) {
      throw "Expected WebRtcNet.Api output missing at $apiOut"
    }
  }

  Invoke-InteropBuild -VsDevCmdPath $vsDevCmd -MsBuildPath $msbuild -ProjectPath 'WebRtcInterop\WebRtcInterop.Framework.vcxproj' -Configuration $configuration -DotnetRoot $dotnetRoot -DotnetExe $dotnetExe -MsBuildSdksPath $msbuildSdksPath
  Invoke-InteropBuild -VsDevCmdPath $vsDevCmd -MsBuildPath $msbuild -ProjectPath 'WebRtcInterop\WebRtcInterop.Core.vcxproj' -Configuration $configuration -DotnetRoot $dotnetRoot -DotnetExe $dotnetExe -MsBuildSdksPath $msbuildSdksPath

  Invoke-NativeCommand -FilePath 'dotnet' -ArgumentList @(
    'build',
    'WebRtcNet\WebRtcNet.csproj',
    '-c', $configuration,
    '-p:Platform=x64',
    '--no-restore',
    '/p:BuildProjectReferences=false'
  )

  foreach ($targetFramework in @('net10.0', 'net48')) {
    $webRtcNetOut = Join-Path "WebRtcNet\bin\x64\$configuration\$targetFramework" 'WebRtcNet.dll'
    if (-not (Test-Path -LiteralPath $webRtcNetOut -PathType Leaf)) {
      throw "Expected WebRtcNet output missing at $webRtcNetOut"
    }
  }
}

Invoke-NativeCommand -FilePath 'dotnet' -ArgumentList @(
  'pack',
  'WebRtcNet\WebRtcNet.csproj',
  '-c', 'Release',
  '-p:Platform=x64',
  '--no-build',
  '-o', 'C:\out\nuget'
)
