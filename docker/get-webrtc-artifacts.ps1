<#
.SYNOPSIS
  Pulls a WebRTC artifacts image and extracts headers/libs to a local path.

.DESCRIPTION
  Intended for local development. Run this once after pulling a new WebRTC branch.
  After extraction, open Visual Studio normally — no Docker involvement at build time.

.PARAMETER WebRtcBranch
  The WebRTC branch number (e.g. 7778). Must match a published image tag.
  Also used with -Local to resolve the image tag. Defaults to 7778.

.PARAMETER Registry
  The container registry prefix. Defaults to ghcr.io/general-fault.
  Only valid when not using -Local.

.PARAMETER Local
  Uses the local Docker image tag `webrtc:msvc-shared-<WebRtcBranch>` instead of pulling from GHCR.
  When specified, -Registry is not allowed.

.PARAMETER ArtifactsPath
  Local path where artifacts are extracted. Defaults to third-party\google\webrtc
  inside the repository root so no writes happen outside the repo during local development.
  Choose a repo-local path to avoid writing outside the repository.

.PARAMETER SetEnvVars
  When specified, also sets WEBRTC_SRC_PATH, WEBRTC_OUT_PATH, and WEBRTC_PREBUILT=1
  as user-level environment variables. Not required for local Visual Studio development —
  WebRtcInterop.BuildPaths.props auto-detects the repo-local artifact directory.
  Useful only when pointing VS at a non-default artifacts location.

.EXAMPLE
  .\docker\get-webrtc-artifacts.ps1 -WebRtcBranch 7778

.EXAMPLE
  .\docker\get-webrtc-artifacts.ps1 -Local
#>
[CmdletBinding(DefaultParameterSetName = 'Remote')]
param(
  [Parameter(ParameterSetName = 'Remote')]
  [Parameter(ParameterSetName = 'Local')]
  [string]$WebRtcBranch = '7778',
  [Parameter(ParameterSetName = 'Remote')]
  [string]$Registry = 'ghcr.io/general-fault',
  [Parameter(Mandatory = $true, ParameterSetName = 'Local')]
  [switch]$Local,
  [string]$ArtifactsPath,
  [switch]$SetEnvVars
)

# Default to repo-local path so no writes happen outside the repository
if (-not $ArtifactsPath) {
  $ArtifactsPath = Join-Path (Split-Path $PSScriptRoot -Parent) 'third-party\google\webrtc'
}

$ErrorActionPreference = 'Stop'

if ($PSCmdlet.ParameterSetName -eq 'Local') {
  $image = "webrtc:msvc-shared-$WebRtcBranch"
  Write-Host "Using local image $image..."
  docker image inspect $image 2>$null | Out-Null
  if ($LASTEXITCODE -ne 0) { throw "Local Docker image '$image' was not found. Build it first (for example with .\docker\build-images.ps1)." }
} else {
  $image = "$Registry/webrtc:msvc-shared-$WebRtcBranch"
  Write-Host "Pulling $image..."
  docker pull $image
  if ($LASTEXITCODE -ne 0) { throw "docker pull $image failed with exit code $LASTEXITCODE" }
}

$containerId = (docker create $image).Trim()
if ($LASTEXITCODE -ne 0) { throw "docker create failed with exit code $LASTEXITCODE" }

try {
  Write-Host "Extracting artifacts to $ArtifactsPath..."
  $parent = Split-Path $ArtifactsPath -Parent
  $leafName = Split-Path $ArtifactsPath -Leaf

  New-Item -ItemType Directory -Force $parent | Out-Null

  # docker cp copies the source directory into the parent, creating <parent>\webrtc-artifacts\
  docker cp "${containerId}:C:\webrtc-artifacts" $parent
  if ($LASTEXITCODE -ne 0) { throw "docker cp failed with exit code $LASTEXITCODE" }

  # Rename the extracted directory if the requested leaf name differs
  $extracted = Join-Path $parent 'webrtc-artifacts'
  if ($extracted -ne $ArtifactsPath) {
    if (Test-Path $ArtifactsPath) { Remove-Item $ArtifactsPath -Recurse -Force }
    Rename-Item $extracted $leafName
  }
} finally {
  docker rm $containerId | Out-Null
}

if ($SetEnvVars) {
  $srcPath = Join-Path $ArtifactsPath 'include'
  $outPath = Join-Path $ArtifactsPath 'lib'
  [System.Environment]::SetEnvironmentVariable('WEBRTC_SRC_PATH', $srcPath, 'User')
  [System.Environment]::SetEnvironmentVariable('WEBRTC_OUT_PATH', $outPath, 'User')
  [System.Environment]::SetEnvironmentVariable('WEBRTC_PREBUILT', '1', 'User')
  Write-Host "Set user environment variables:"
  Write-Host "  WEBRTC_SRC_PATH = $srcPath"
  Write-Host "  WEBRTC_OUT_PATH = $outPath"
  Write-Host "  WEBRTC_PREBUILT = 1"
  Write-Host "Restart Visual Studio for the environment changes to take effect."
}

if ($PSCmdlet.ParameterSetName -eq 'Local') {
  Write-Host "Done. Local WebRTC artifacts extracted to $ArtifactsPath"
} else {
  Write-Host "Done. WebRTC $WebRtcBranch artifacts extracted to $ArtifactsPath"
}
