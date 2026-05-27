## WebRTC patch set

This directory contains local WebRTC source patches that are applied during the Docker-based native build (`docker\build-webrtc.ps1 -Mode build`).

## Active patches

### `0001-fix-peerconnectionfactorydependencies-mediafactory-move-assignment.patch`

**What it changes**

- `api\peer_connection_interface.h`: changes `PeerConnectionFactoryDependencies::operator=(PeerConnectionFactoryDependencies&&)` from inline `= default` to a declaration.
- `api\peer_connection_interface.cc`: adds the out-of-line defaulted definition for that move-assignment operator.

**Why this exists**

This avoids an incomplete-type instantiation path around `MediaFactory` when building WebRTC in the current WebRtcNet toolchain configuration.

**How it is applied**

- `docker\Dockerfile.webrtc-build` copies `third-party\google\webrtc\*.patch` to `C:\build\patches\`.
- `docker\build-webrtc.ps1` runs `gclient runhooks`, then loads patches matching `^[0-9]{4}-.+\.patch$`, sorts by filename, and:
  - runs `git apply --check` to verify clean apply,
  - applies with `git am --3way`,
  - or skips when `git apply --check --reverse` shows the patch is already present.

## Patch numbering

Patch prefixes (`0001`, `0002`, ...) are apply-order markers for a patch series (same convention as `git format-patch`), not permanent IDs. Keep numbering contiguous based on required apply order.

## Removal criteria

This patch can be removed when all of the following are true:

1. The target WebRTC branch already contains an equivalent upstream fix.
2. The Docker/native build succeeds without this patch across the supported configurations used by this repo.
3. No downstream WebRtcNet interop build failures regress after removing it.

When removing it, also remove the entry from this README so the local patch set stays authoritative.
