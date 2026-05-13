$ErrorActionPreference = 'Stop'

$env:PATH = "$env:DEPOT_TOOLS_PATH;$env:PATH"
$env:DEPOT_TOOLS_WIN_TOOLCHAIN = '0'
$env:DEPOT_TOOLS_UPDATE = '0'
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
