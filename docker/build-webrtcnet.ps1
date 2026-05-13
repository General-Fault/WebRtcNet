$ErrorActionPreference = 'Stop'

New-Item -ItemType Directory -Force C:\out\nuget | Out-Null
dotnet restore WebRtcNet.slnx
dotnet restore WebRtcNet.Api\WebRtcNet.Api.csproj -p:TargetFramework=net47
dotnet restore WebRtcNet\WebRtcNet.csproj -p:TargetFramework=net47

$vsDevCmd = 'C:\BuildTools\Common7\Tools\VsDevCmd.bat'
$msbuild = 'C:\BuildTools\MSBuild\Current\Bin\MSBuild.exe'
$dotnetRoot = Join-Path ${env:ProgramFiles} 'dotnet'
$dotnetExe = Join-Path $dotnetRoot 'dotnet.exe'
$dotnetSdkRoot = Join-Path $dotnetRoot 'sdk'
if (!(Test-Path $vsDevCmd)) {
  throw "VsDevCmd.bat was not found at $vsDevCmd"
}
if (!(Test-Path $msbuild)) {
  throw "MSBuild.exe was not found at $msbuild"
}
if (!(Test-Path $dotnetExe)) {
  throw "dotnet.exe was not found at $dotnetExe"
}
if (!(Test-Path $dotnetSdkRoot)) {
  throw "dotnet SDK root was not found at $dotnetSdkRoot"
}

$latestSdk = Get-ChildItem $dotnetSdkRoot -Directory |
  Sort-Object Name -Descending |
  Select-Object -First 1
if (!$latestSdk) {
  throw "No .NET SDKs found under $dotnetSdkRoot"
}
$msbuildSdksPath = Join-Path $latestSdk.FullName 'Sdks'
if (!(Test-Path $msbuildSdksPath)) {
  throw "MSBuild SDKs path was not found at $msbuildSdksPath"
}

dotnet build WebRtcNet.Api\WebRtcNet.Api.csproj -c Release -f net47 -p:Platform=x64 --no-restore
if ($LASTEXITCODE -ne 0) {
  throw "dotnet build WebRtcNet.Api failed with exit code $LASTEXITCODE"
}

$apiNet47Out = 'WebRtcNet.Api\bin\x64\Release\net47'
$apiNet45Out = 'WebRtcNet.Api\bin\x64\Release\net45'
if (!(Test-Path (Join-Path $apiNet47Out 'WebRtcNet.Api.dll'))) {
  throw "Expected WebRtcNet.Api output missing at $apiNet47Out"
}
New-Item -ItemType Directory -Force $apiNet45Out | Out-Null
Copy-Item -Force (Join-Path $apiNet47Out 'WebRtcNet.Api.*') $apiNet45Out

cmd /S /C "`"$vsDevCmd`" -arch=x64 -host_arch=x64 && set `"DOTNET_ROOT=$dotnetRoot`" && set `"DOTNET_HOST_PATH=$dotnetExe`" && set `"DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR=$dotnetRoot`" && set `"MSBuildSDKsPath=$msbuildSdksPath`" && set `"MSBuildEnableWorkloadResolver=false`" && `"$msbuild`" WebRtcInterop\WebRtcInterop.Framework.vcxproj /p:Configuration=Release /p:Platform=x64 /p:PlatformToolset=v143 /p:BuildProjectReferences=false /m"
if ($LASTEXITCODE -ne 0) {
  throw "MSBuild failed with exit code $LASTEXITCODE"
}

dotnet build WebRtcNet\WebRtcNet.csproj -c Release -f net47 -p:Platform=x64 --no-restore /p:BuildProjectReferences=false
if ($LASTEXITCODE -ne 0) {
  throw "dotnet build WebRtcNet failed with exit code $LASTEXITCODE"
}

dotnet pack WebRtcNet\WebRtcNet.csproj -c Release -f net47 -p:Platform=x64 --no-build -o C:\out\nuget
