# Copilot instructions for WebRtcNet

## Build and test commands

- Restore NuGet packages for the NUnit/FakeItEasy test projects before building tests:
  - `nuget restore WebRtcNet.slnx`
- Build the managed contract assembly:
  - `dotnet msbuild WebRtcNet\WebRtcNet.csproj /p:Configuration=Debug`
- Build the managed NUnit test assembly:
  - `dotnet msbuild WebRtcNet\UnitTests\WebRtcNet.UnitTests.csproj /p:Configuration=Debug`
- Build the full solution, including the C++/CLI interop layer:
  - `dotnet msbuild WebRtcNet.slnx /p:Configuration=Debug /p:Platform=x64`
  - `dotnet msbuild WebRtcNet.slnx /p:Configuration=Debug /p:Platform=x86`
- Run the managed NUnit suite:
  - `nunit3-console WebRtcNet\UnitTests\bin\Debug\WebRtcNet.UnitTests.dll`
- Run one managed NUnit test:
  - `nunit3-console WebRtcNet\UnitTests\bin\Debug\WebRtcNet.UnitTests.dll --where "test == WebRtcNet.UnitTests.RtcConfigurationTests.RtcConfiguration_Constructor_Defaults_Test"`

## High-level architecture

- `WebRtcNet\` is the public .NET Framework 4.6 API surface. It mostly defines W3C-style WebRTC contracts, DTOs, enums, and event argument types such as `IRtcPeerConnection`, `IMediaStream`, `RtcConfiguration`, and `RtcIceServer`.
- `WebRtcInterop\` is the implementation layer. It is a C++/CLI bridge that references `WebRtcNet` and wraps the native Google WebRTC client from `third-party\WebRtc\src`.
- `RtcPeerConnectionFactory` owns the native `PeerConnectionFactoryInterface` singleton and the signaling thread. `RtcPeerConnection`, `RtcDataChannel`, `MediaStream`, and related types hold native `scoped_refptr` handles and expose the managed interfaces from `WebRtcNet`.
- `WebRtcInterop\Marshaling\` contains the `marshal_as` specializations that convert between managed DTOs/enums and native WebRTC types. Most cross-boundary value translation happens here, not inline in business logic.
- `WebRtcInterop\Observers\` adapts native callbacks into managed events. The observers call `FireOn...` helpers on the wrapper classes, which then raise the `WebRtcNet` events.
- `WebRtcInterop\MediaStream.cpp` contains the concrete `Media::GetUserMedia` implementation. Device enumeration and native audio/video source creation happen in the interop project, not in the C# project.
- The native dependency is not vendored as built binaries. The interop project expects a local WebRTC checkout/build under `third-party\WebRtc\src`, and links against `out\Debug`, `out\Debug_x64`, `out\Release`, or `out\Release_x64` depending on platform/configuration.

## Key conventions

- Treat `WebRtcNet` as the contract assembly and `WebRtcInterop` as the only real implementation. If you add API surface to `WebRtcNet`, you usually also need matching wrapper, marshalling, and observer work in `WebRtcInterop`.
- Do not assume every interface member is implemented. Several interop methods still throw `NotImplementedException`, especially around stream enumeration, stats, identity, and parts of peer connection setup.
- Managed argument validation commonly uses `System.Diagnostics.Contracts.Contract.Requires(...)` instead of ad hoc null checks.
- Interop wrapper classes follow the same lifetime pattern: destructor/finalizer pair plus a `GetNative...(... throwOnDisposed)` helper that throws `ObjectDisposedException` when the native handle is gone.
- Some interop methods require concrete wrapper instances, not arbitrary interface implementations. For example, peer connection stream operations `dynamic_cast` `IMediaStream` to `WebRtcInterop::MediaStream` before unwrapping the native object.
- Debug builds of `WebRtcInterop` include additional NUnit marshalling tests from `WebRtcInterop\Marshaling\UnitTests\*.cpp`; they are compiled into the interop project itself rather than a separate test project.
