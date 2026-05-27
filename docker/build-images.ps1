param(
  [Parameter(Mandatory = $true)]
  [string]$WebRtcBranch,
  [string]$Registry = 'ghcr.io/general-fault',
  [switch]$FastDevSync,
  [switch]$SkipToolchain,
  [switch]$SkipSync,
  [switch]$SkipBuild,
  [switch]$Publish
)

$ErrorActionPreference = 'Stop'

function Import-DotEnvIfPresent {
  param(
    [Parameter(Mandatory = $true)][string]$Path
  )

  if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
    return
  }

  Write-Host "Loading environment variables from $Path"

  foreach ($line in Get-Content -LiteralPath $Path) {
    $trimmed = $line.Trim()
    if ([string]::IsNullOrWhiteSpace($trimmed) -or $trimmed.StartsWith('#')) {
      continue
    }

    $separator = $trimmed.IndexOf('=')
    if ($separator -lt 1) {
      continue
    }

    $name = $trimmed.Substring(0, $separator).Trim()
    $value = $trimmed.Substring($separator + 1).Trim()

    if (($value.StartsWith('"') -and $value.EndsWith('"')) -or ($value.StartsWith("'") -and $value.EndsWith("'"))) {
      $value = $value.Substring(1, $value.Length - 2)
    }

    if ([string]::IsNullOrWhiteSpace([Environment]::GetEnvironmentVariable($name))) {
      [Environment]::SetEnvironmentVariable($name, $value, 'Process')
    }
  }
}

function Get-RegistryHost {
  param(
    [Parameter(Mandatory = $true)][string]$RegistryReference
  )

  $parts = $RegistryReference.Split('/', 2)
  return $parts[0]
}

function Invoke-Docker {
  param(
    [Parameter(Mandatory = $true)][string[]]$ArgumentList
  )

  & docker @ArgumentList
  if ($LASTEXITCODE -ne 0) {
    throw "docker $($ArgumentList -join ' ') failed with exit code $LASTEXITCODE"
  }
}

function Invoke-DockerLogin {
  param(
    [Parameter(Mandatory = $true)][string]$RegistryReference
  )

  $registryHost = Get-RegistryHost -RegistryReference $RegistryReference
  $username = [Environment]::GetEnvironmentVariable('GHCR_USERNAME')
  if ([string]::IsNullOrWhiteSpace($username)) {
    throw "GHCR_USERNAME is required when using -Publish. Set it in your environment or in a repo-local .env file."
  }

  $token = [Environment]::GetEnvironmentVariable('GHCR_PAT')
  if ([string]::IsNullOrWhiteSpace($token)) {
    throw "GHCR_PAT is required when using -Publish. Set a classic PAT with write:packages in your environment or in a repo-local .env file."
  }

  Write-Host "Logging in to $registryHost as $username..."
  $token | & docker login $registryHost -u $username --password-stdin
  if ($LASTEXITCODE -ne 0) {
    throw "docker login $registryHost failed with exit code $LASTEXITCODE"
  }
}

function Invoke-DockerBuild {
  param(
    [Parameter(Mandatory = $true)][string[]]$ArgumentList
  )
  Invoke-Docker (@('build') + $ArgumentList)
}

function Test-DockerImageExists {
  param(
    [Parameter(Mandatory = $true)][string]$ImageReference
  )

  & docker image inspect $ImageReference *> $null
  return ($LASTEXITCODE -eq 0)
}

function Resolve-PreferredImageReference {
  param(
    [Parameter(Mandatory = $true)][string]$DisplayName,
    [Parameter(Mandatory = $true)][string[]]$LocalCandidates,
    [Parameter(Mandatory = $true)][string]$RemoteCandidate
  )

  foreach ($candidate in $LocalCandidates) {
    if (Test-DockerImageExists -ImageReference $candidate) {
      Write-Host "Using local $DisplayName image: $candidate"
      return $candidate
    }
  }

  Write-Host "No local $DisplayName image found. Falling back to: $RemoteCandidate"
  return $RemoteCandidate
}

function Resolve-LocalOrDefaultImageReference {
  param(
    [Parameter(Mandatory = $true)][string]$DisplayName,
    [Parameter(Mandatory = $true)][string[]]$LocalCandidates,
    [Parameter(Mandatory = $true)][string]$DefaultReference
  )

  foreach ($candidate in $LocalCandidates) {
    if (Test-DockerImageExists -ImageReference $candidate) {
      Write-Host "Using local $DisplayName image: $candidate"
      return $candidate
    }
  }

  Write-Host "No local $DisplayName image found. Using transient stage: $DefaultReference"
  return $DefaultReference
}

function Remove-DockerImageIfExists {
  param(
    [Parameter(Mandatory = $true)][string]$ImageReference
  )

  if (Test-DockerImageExists -ImageReference $ImageReference) {
    Write-Host "Removing transient image reference: $ImageReference"
    Invoke-Docker @('image', 'rm', '-f', $ImageReference)
  }
}

# Load optional repo-local secrets for Docker publish.
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
Import-DotEnvIfPresent -Path (Join-Path $repoRoot '.env')

# Validate -SkipBuild usage
if ($SkipBuild) {
  if ($SkipToolchain -or $SkipSync) {
    Write-Host "Note: -SkipBuild takes precedence; -SkipToolchain and -SkipSync are ignored."
  }
  Write-Host "Skipping WebRTC compilation stages. Using existing local images for artifacts build."
}

if ($Publish) {
  Invoke-DockerLogin -RegistryReference $Registry
}

try {
  # Stage 1: toolchain image (VS Build Tools, depot_tools, git) — rebuilt infrequently
  if (-not $SkipBuild -and -not $SkipToolchain) {
    Write-Host "Building webrtc-toolchain..."
    Invoke-DockerBuild @(
      '-f', 'docker\Dockerfile.webrtc-toolchain',
      '-t', 'webrtc-toolchain',
      '-t', 'webrtc-toolchain:latest',
      '-t', "webrtc-toolchain:$WebRtcBranch",
      '.'
    )
  }

  # Stage 2: sync WebRTC sources for the specified branch
  if (-not $SkipBuild -and -not $SkipSync) {
    $toolchainImageForSync = Resolve-PreferredImageReference `
      -DisplayName 'webrtc-toolchain' `
      -LocalCandidates @("webrtc-toolchain:$WebRtcBranch", 'webrtc-toolchain:latest', 'webrtc-toolchain') `
      -RemoteCandidate "$Registry/webrtc-toolchain:latest"

    Write-Host "Building webrtc-sync (branch $WebRtcBranch)..."
    $syncArgs = @(
      '-f', 'docker\Dockerfile.webrtc-sync',
      '-t', 'webrtc-sync',
      '-t', 'webrtc-sync:latest',
      '-t', "webrtc-sync:$WebRtcBranch",
      '--build-arg', "WEBRTC_BRANCH=$WebRtcBranch"
    )
    $syncArgs += @('--build-arg', "WEBRTC_TOOLCHAIN_IMAGE=$toolchainImageForSync")
    if ($FastDevSync) {
      $syncArgs += @('--build-arg', 'FAST_DEV_SYNC=true')
    }
    $syncArgs += '.'
    Invoke-DockerBuild $syncArgs
  }

  if (-not $SkipBuild) {
    $syncImageForBuild = Resolve-PreferredImageReference `
      -DisplayName 'webrtc-sync' `
      -LocalCandidates @("webrtc-sync:$WebRtcBranch", 'webrtc-sync:latest', 'webrtc-sync') `
      -RemoteCandidate "$Registry/webrtc-sync:$WebRtcBranch"

    # Stage 3: compile WebRTC
    Write-Host "Building webrtc-build..."
    Invoke-DockerBuild @(
      '-f', 'docker\Dockerfile.webrtc-build',
      '--target', 'webrtc-build',
      '-t', 'webrtc-build',
      '-t', 'webrtc-build:latest',
      '-t', "webrtc-build:$WebRtcBranch",
      '--build-arg', "WEBRTC_SYNC_IMAGE=$syncImageForBuild",
      '--build-arg', "WEBRTC_BRANCH=$WebRtcBranch",
      '.'
    )
  }

  $buildImageForWebRtc = Resolve-PreferredImageReference `
    -DisplayName 'webrtc-build' `
    -LocalCandidates @("webrtc-build:$WebRtcBranch", 'webrtc-build:latest', 'webrtc-build') `
    -RemoteCandidate "$Registry/webrtc-build:$WebRtcBranch"

  # Stage 4: build final publishable webrtc image tag (webrtc-artifacts-stage and webrtc-artifacts are internal transient stages)
  $artifactsTag = "msvc-shared-$WebRtcBranch"
  $artifactsImage = "webrtc:$artifactsTag"
  $artifactsRegistryImage = "$Registry/webrtc:$artifactsTag"

  Write-Host "Building $artifactsImage..."
  Invoke-DockerBuild @(
    '-f', 'docker\Dockerfile.webrtc',
    '--target', 'webrtc',
    '-t', $artifactsImage,
    '-t', $artifactsRegistryImage,
    '--build-arg', "WEBRTC_BUILD_IMAGE=$buildImageForWebRtc",
    '--build-arg', "WEBRTC_BRANCH=$WebRtcBranch",
    '.'
  )

  if ($Publish) {
    Write-Host "Pushing webrtc:$artifactsTag..."
    Invoke-Docker @('push', $artifactsRegistryImage)
  }
} finally {
  # Prune untagged layers so failed/interrupted builds do not leave orphaned <none> images.
  & docker image prune -f --filter 'dangling=true' | Out-Null
}
