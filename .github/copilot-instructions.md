# Copilot instructions for WebRtcNet

## Build and test commands

### Managed projects (SDK-style, multi-target net10.0 + net48)

- Restore packages: `dotnet restore WebRtcNet.slnx`
- Build the API contract assembly: `dotnet build WebRtcNet.Api\WebRtcNet.Api.csproj -c Debug`
- Build the legacy wrapper assembly: `dotnet build WebRtcNet\WebRtcNet.csproj -c Debug`
- Run the managed NUnit test suite: `dotnet test WebRtcNet.Api.UnitTests\WebRtcNet.Api.UnitTests.csproj`
- Run a single managed test: `dotnet test WebRtcNet.Api.UnitTests\WebRtcNet.Api.UnitTests.csproj --filter "FullyQualifiedName=WebRtcNet.UnitTests.RtcConfigurationTests.RtcConfiguration_Constructor_Defaults_Test"`

### C++/CLI interop projects (requires WEBRTC_SRC_PATH and WEBRTC_OUT_PATH env vars)

- Build the full solution for x64: `msbuild WebRtcNet.slnx /p:Configuration=Debug /p:Platform=x64`
- Build just the Framework interop DLL: `msbuild WebRtcInterop\WebRtcInterop.Framework.vcxproj /p:Configuration=Debug /p:Platform=x64`
- Build the interop C++ unit tests: `msbuild WebRtcInterop.UnitTests\WebRtcInterop.UnitTests.vcxproj /p:Configuration=Debug /p:Platform=x64`
- Run interop C++ unit tests: `WebRtcInterop.UnitTests\x64\Debug\WebRtcInterop.UnitTests.exe` (Google Test runner)

### Local developer setup (WebRTC pre-built artifacts from GHCR)

Pull the pre-built WebRTC artifacts image and extract them into the repo:

```powershell
.\docker\get-webrtc-artifacts.ps1 -WebRtcBranch 7778
```

Or extract from an already-built local image without pulling from GHCR:

```powershell
.\docker\get-webrtc-artifacts.ps1 -Local
```

This extracts artifacts to `third-party\google\webrtc\` inside the repository. No files are written outside the repo. `WebRtcInterop.BuildPaths.props` auto-detects the directory and sets `WebRtcSrcRoot`, `WebRtcOutRoot`, and `WebRtcPrebuilt=1` — no environment variables or Visual Studio restart required.

To override with a non-default path or for CI, set `WEBRTC_SRC_PATH` and `WEBRTC_OUT_PATH` env vars; the props file prefers explicit env vars over the repo-local fallback.

### Docker / CI image builds

Build all WebRTC pipeline images locally:

```powershell
.\docker\build-images.ps1 -WebRtcBranch 7778
```

Publishing is opt-in: add `-Publish` to push only the artifacts image (`ghcr.io/general-fault/webrtc:msvc-shared-<branch>`) to GHCR.

Skip slow stages during iteration: `-SkipToolchain`, `-SkipSync`.

## High-level architecture

### Managed layers

- `WebRtcNet.Api\` is the primary public contract assembly. It defines W3C-style WebRTC interfaces, DTOs, enums, and event args: `IRtcPeerConnection`, `IMediaStream`, `RtcConfiguration`, `RtcIceServer`, etc. Targets `net10.0` and `net48`.
- `WebRtcNet\` is a legacy wrapper assembly that re-exports `WebRtcNet.Api` types. It exists for backwards compatibility and also targets `net10.0` and `net48`.
- `WebRtcNet.Api.UnitTests\` contains NUnit tests for the managed API layer. These test only pure managed code and have no native dependency.

### Interop layer

- `WebRtcInterop\` is the C++/CLI bridge. Two project files target different runtimes:
  - `WebRtcInterop.Framework.vcxproj` — targets .NET Framework (`CLRSupport=true`, used in production and in CI via `build-webrtcnet.ps1`)
  - `WebRtcInterop.Core.vcxproj` — targets .NET Core (`CLRSupport=NetCore`, experimental)
  - Both share source via `WebRtcInterop.Shared.vcxitems`
- `RtcPeerConnectionFactory` owns the native `PeerConnectionFactoryInterface` singleton and the signaling thread.
- `RtcPeerConnection`, `RtcDataChannel`, `MediaStream`, and related wrappers hold native `scoped_refptr` handles via the `NativeWrapper<T>` template base class (`NativeWrapper.h`).
- `WebRtcInterop\Marshaling\` contains all `marshal_as<>` specializations for converting between managed DTOs/enums and native WebRTC types. Cross-boundary value translation belongs here, not inline in business logic.
- `WebRtcInterop\Observers\` adapts native callbacks into managed events via observer classes that call `FireOn...` helpers on the wrapper types.
- `WebRtcInterop\MediaStream.cpp` contains the concrete `Media::GetUserMedia` implementation; device enumeration and native audio/video source creation happen here, not in C#.
- `WebRtcInterop.UnitTests\` is a standalone C++ project using Google Test. It tests marshalling and interop behavior directly against native WebRTC types.

### Native dependency and build paths

- `WebRtcInterop.BuildPaths.props` maps env vars to MSBuild properties: `WEBRTC_SRC_PATH` → `$(WebRtcSrcRoot)`, `WEBRTC_OUT_PATH` → `$(WebRtcOutRoot)`. When those env vars are absent it falls back to `third-party\google\webrtc\include` and `third-party\google\webrtc\lib` relative to the repo root. The `WebRtcPrebuilt` property is set to `1` automatically when either `WEBRTC_PREBUILT=1` env var is present or the repo-local artifact directory exists.
- `WebRtcInterop.Shared.vcxitems` gates the gn/ninja custom build step on `'$(WebRtcPrebuilt)' != '1'` (MSBuild property, not raw env var).
- Include paths span `$(WebRtcSrcRoot)`, `$(WebRtcSrcRoot)\third_party\abseil-cpp`, `$(WebRtcSrcRoot)\third_party\libyuv\include`, `$(WebRtcSrcRoot)\third_party\jsoncpp\source\include`, and others (see `WebRtcInterop.Shared.vcxitems`).
- Prebuilt artifact layout in the GHCR image (`ghcr.io/general-fault/webrtc:msvc-shared-{branch}`): `C:\webrtc-artifacts\include\` (headers + `.inc` + `.ifc`, full subtree from `src\`) and `C:\webrtc-artifacts\lib\{configuration}\{architecture}\webrtc.lib` (no `obj` segment).

### Docker pipeline

Individual-stage Dockerfiles in `docker\` — not a monolithic file:
1. `docker\Dockerfile.webrtc-toolchain` — VS BuildTools, depot_tools, git
2. `docker\Dockerfile.webrtc-sync` — syncs WebRTC source for a given branch
3. `docker\Dockerfile.webrtc-build` — compiles WebRTC (`webrtc-build` stage)
4. `docker\Dockerfile.webrtc` — creates `webrtc-artifacts-stage`, `webrtc-artifacts`, and final `webrtc` image tags from prebuilt inputs
5. `docker\Dockerfile.webrtcnet` — builds WebRtcNet using artifacts from the GHCR image

`buildx` is not used — Windows containers require classic `docker build`.

## Key conventions

- `WebRtcNet.Api` is the contract assembly. If you add API surface there, you also need matching wrapper, marshalling, and observer work in `WebRtcInterop`.
- Do not assume every interface member is implemented. Many interop methods still throw `NotImplementedException`, especially around stream enumeration, stats, identity, and parts of peer connection setup.
- Managed argument validation uses `System.Diagnostics.Contracts.Contract.Requires(...)` rather than ad hoc null checks.
- Interop wrapper classes follow the same lifetime pattern: destructor/finalizer pair, and a `GetNative...(throwOnDisposed)` helper that throws `ObjectDisposedException` when the native handle is gone.
- Some interop methods require concrete wrapper instances, not arbitrary interface implementations. Peer connection stream operations `dynamic_cast` `IMediaStream` to `WebRtcInterop::MediaStream` before unwrapping the native object.
- `marshal_as<>` specializations in `Marshaling\` use either switch-based dispatch (simple enums) or bidirectional `std::map` helpers (`marshal_mapped_native_type` / `marshal_mapped_managed_type` in `MarshalEnums.h`) for types where both directions are needed.
- The solution uses standard `Debug` and `Release` configurations; use these directly for normal development and CI.
- `docker\build-webrtcnet.ps1` hardcodes `C:\BuildTools` for VS paths; `docker\Dockerfile.webrtcnet` must install VS Build Tools to that path.
