## WebRTC patch set

This directory contains local patch files applied during Docker-based WebRTC builds.

- `0001-fix-peerconnectionfactorydependencies-mediafactory-move-assignment.patch`

`docker\Dockerfile.webrtc-build` copies `third-party\google\webrtc\*.patch` into the build image, and `docker\build-webrtc.ps1` applies those patches with `git am --3way` during the native WebRTC build.
