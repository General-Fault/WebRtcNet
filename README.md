# WebRtcNet

A .NET implementation of the WebRTC standard built using the WebRTC Project native client for Windows desktop applications. WebRtcNet is not endorsed by or affiliated with Google or the WebRTC Project in any way.

WebRtcNet currently uses WebRTC branch-heads/7778, which corresponds to Chromium milestone 148 (see https://chromiumdash.appspot.com/branches).

See [WebRTC 1.0: Real-time Communication Between Browsers](https://www.w3.org/TR/webrtc/) for API documentation.

## Contribution and AI guidance

- Human contributor workflow requirements: `CONTRIBUTING.md`
- AI execution guardrails for this repository: `Agents.md`

## Local W3C spec corpus

This repository keeps a local standards corpus in `documents\specs\` to support standards-alignment work in `WebRtcNet.Api` without repeated web fetching.

- Canonical snapshots: `TR-*.html`
- Copilot-friendly artifacts: `*-summary.md`, `*-idl.webidl`, and `specs\index\spec-map.*`
- API mapping: `documents\crosswalk\webrtcnet-api-to-spec.md`

Refresh with:

```powershell
.\scripts\update-spec-docs.ps1
```

Recommended refresh triggers:

1. Monthly (scheduled automation is available in `.github\workflows\update-spec-docs.yml`).
2. After public API changes in `WebRtcNet.Api` that affect spec alignment.

## Local development quick start

The easiest way to get started is to pull the pre-built WebRTC artifacts from the GitHub Container Registry. This avoids a multi-hour native build.

**Prerequisites:** Docker Desktop configured for Windows containers.

```powershell
.\docker\get-webrtc-artifacts.ps1 -WebRtcBranch 7778
```

This pulls `ghcr.io/general-fault/webrtc:msvc-shared-7778` and extracts headers, `.inc`/module files, and libraries to `third-party\google\webrtc\` inside the repository. No files are written outside the repo.

If you already built the artifacts image locally, you can skip GHCR and extract directly from that local image:

```powershell
.\docker\get-webrtc-artifacts.ps1 -Local
```

`WebRtcInterop.BuildPaths.props` automatically detects the artifact directory and sets the correct include and library paths. No environment variables or Visual Studio restart are required.

## Building WebRtcNet

```powershell
dotnet restore WebRtcNet.slnx
dotnet build WebRtcNet.Api\WebRtcNet.Api.csproj -c Debug
dotnet msbuild WebRtcInterop\WebRtcInterop.Framework.vcxproj /p:Configuration=Debug /p:Platform=x64
```

Or build the full solution:

```powershell
dotnet msbuild WebRtcNet.slnx /p:Configuration=Debug /p:Platform=x64
```

## Running tests

```powershell
dotnet test WebRtcNet.Api.UnitTests\WebRtcNet.Api.UnitTests.csproj
```

C++/CLI interop tests (requires a built WebRTC):

```powershell
dotnet msbuild WebRtcInterop.UnitTests\WebRtcInterop.UnitTests.vcxproj /p:Configuration=Debug /p:Platform=x64
.\WebRtcInterop.UnitTests\x64\Debug\WebRtcInterop.UnitTests.exe
```

## Docker pipeline

The build pipeline uses individual-stage Dockerfiles rather than a single monolithic file. `docker buildx` is not used — Windows containers require classic `docker build`.

| Dockerfile | Purpose |
|---|---|
| `docker\Dockerfile.webrtc-toolchain` | VS Build Tools, depot_tools, git |
| `docker\Dockerfile.webrtc-sync` | Syncs WebRTC source for a given branch |
| `docker\Dockerfile.webrtc-build` | Compiles WebRTC (`webrtc-build` stage) |
| `docker\Dockerfile.webrtc` | Creates `webrtc-artifacts-stage`, `webrtc-artifacts`, and final `webrtc` image tags from prebuilt inputs |
| `docker\Dockerfile.webrtcnet` | Builds WebRtcNet using artifacts from GHCR |

`docker\build-images.ps1` orchestrates the full pipeline:

```powershell
.\docker\build-images.ps1 -WebRtcBranch 7778
```

By default this builds all images locally without publishing. Add `-Publish` to push only the artifacts image (`ghcr.io/general-fault/webrtc:msvc-shared-<branch>`) to GHCR. Skip stages you haven't changed with `-SkipToolchain` and `-SkipSync`. Speed up the sync stage during Dockerfile iteration with `-FastDevSync`.

For publishing, the script can automatically load credentials from a repo-local `.env` file and run `docker login` for you. Copy `.env.example` to `.env` and set:

```powershell
GHCR_USERNAME=General-Fault
GHCR_PAT=<classic PAT with write:packages>
```

Then publish normally:

```powershell
.\docker\build-images.ps1 -WebRtcBranch 7778 -Publish
```

The WebRTC build takes several hours. The GitHub Actions workflow (`build-webrtc.yml`) can be triggered manually via `workflow_dispatch` and runs on `windows-2025`. It pushes only `webrtc:msvc-shared-<branch>` to GHCR. A self-hosted runner is recommended for the WebRTC build.

## Building WebRTC natively (without Docker)

If you intend to build the native WebRTC library outside of the Docker pipeline, follow the [Google WebRTC native build instructions](https://webrtc.googlesource.com/src/+/main/docs/native-code/index.md) — they change periodically. The short version:

1. Install Visual Studio 2022 or later (Community Edition is sufficient).
   - "Desktop development with C++" workload
   - 10.0.26100.0 Windows 11 SDK

2. Install [Chromium depot_tools](https://commondatastorage.googleapis.com/chrome-infra-docs/flat/depot_tools/docs/html/depot_tools_tutorial.html).

3. Configure git:

   ```
   git config --global user.name "My Name"
   git config --global user.email "name@email"
   git config --global core.autocrlf false
   git config --global core.filemode false
   git config branch.autosetupmerge always
   git config branch.autosetuprebase always
   ```

4. Fetch and sync the WebRTC source (~10 GB):

   ```
   fetch --nohooks webrtc
   git checkout -b webrtcnet_148 refs/remotes/branch-heads/7778
   gclient sync
   ```

5. Point Visual Studio at your local build by setting environment variables or by placing the source tree so that `WebRtcInterop.BuildPaths.props` can find it:
   - `WEBRTC_SRC_PATH` — path to the WebRTC `src\` directory (overrides the repo-local default)
   - `WEBRTC_OUT_PATH` — path where build outputs land (overrides the repo-local default)
   - Leave `WEBRTC_PREBUILT` unset (or `0`) so the gn/ninja custom build step runs on first build.
