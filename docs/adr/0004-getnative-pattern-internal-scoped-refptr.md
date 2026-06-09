# GetNative helpers stay internal and return scoped_refptr

## Status

Accepted

## Date

2026-06-09

## Context

Issue #61 through #64 refactor the WebRtcInterop boundary so the managed API no longer exposes `GetNative*Handle(bool)` methods and the interop layer no longer round-trips native pointers through `IntPtr`.

The old pattern forced each wrapper to expose a public contract method that returned `IntPtr`, then immediately re-cast that pointer back into native WebRTC types inside C++/CLI. That added unnecessary marshaling and made the native lifetime contract harder to reason about.

## Decision

WebRtcNet will keep `GetNative*` helpers as **internal interop-only members** and have them return `webrtc::scoped_refptr<T>` by value.

The public `WebRtcNet.Api` contract types no longer define these helpers. Interop types keep the same `throwOnDisposed` behavior, but the native return path now preserves reference counting directly instead of exposing raw pointers or `IntPtr`.

Native ownership for interop wrappers is handled with `NativeWrapper<T>` composition where needed, so the wrapper can hold and dispose the native reference-counted object safely.

## Consequences

### Positive

- Removes `IntPtr` marshaling for internal native access.
- Preserves reference counting across the managed/native boundary.
- Keeps `throwOnDisposed` semantics intact for existing callers.
- Makes the native access path easier to follow in interop code.

### Negative

- Requires a separate internal/native access helper for each wrapper type.
- Adds one more layer of abstraction between the managed API and native implementation.
- `NativeWrapper<T>` and the interop wrappers must stay aligned so ownership remains explicit.

## Alternatives considered

1. **Keep returning `IntPtr` from public API methods**  
   Rejected because it keeps the unnecessary pointer round-trip and exposes internal native mechanics to the public contract.

2. **Use public wrapper inheritance for ownership**  
   Rejected because the interop types already inherit the managed API base classes and need a single inheritance chain.

3. **Use composition only, without `GetNative*` helpers**  
   Rejected because the interop layer still needs a direct native access path for implementation details.

## References

- Issue #61 — Refactor Media types: scoped_refptr GetNative* pattern
- Issue #62 — Refactor RtcDataChannel: scoped_refptr GetNative* pattern
- Issue #63 — Refactor RtcIceTransport: scoped_refptr GetNative* pattern
- Issue #64 — Refactor RtcPeerConnection and stub types: scoped_refptr GetNative* pattern
- Issue #65 — Document GetNative* refactor: create ADR and update documentation
- `WebRtcInterop\Media\MediaStream.cpp`
- `WebRtcInterop\Media\MediaStreamTrack.cpp`
- `WebRtcInterop\RtcDataChannel.cpp`
- `WebRtcInterop\RtcIceTransport.cpp`
- `WebRtcInterop\RtcPeerConnection.cpp`
