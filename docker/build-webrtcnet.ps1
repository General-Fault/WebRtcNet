$ErrorActionPreference = 'Stop'

New-Item -ItemType Directory -Force C:\out\nuget | Out-Null
dotnet restore WebRtcNet.slnx

$vsDevCmd = 'C:\BuildTools\Common7\Tools\VsDevCmd.bat'
$msbuild = 'C:\BuildTools\MSBuild\Current\Bin\MSBuild.exe'
if (!(Test-Path $vsDevCmd)) {
  throw "VsDevCmd.bat was not found at $vsDevCmd"
}
if (!(Test-Path $msbuild)) {
  throw "MSBuild.exe was not found at $msbuild"
}

cmd /S /C "`"$vsDevCmd`" -arch=x64 -host_arch=x64 && `"$msbuild`" WebRtcNet.slnx /p:Configuration=Release /p:Platform=x64 /m"
if ($LASTEXITCODE -ne 0) {
  throw "MSBuild failed with exit code $LASTEXITCODE"
}

dotnet pack WebRtcNet\WebRtcNet.csproj -c Release -p:Platform=x64 --no-build -o C:\out\nuget
