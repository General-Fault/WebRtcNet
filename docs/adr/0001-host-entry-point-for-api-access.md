# Host entry point for API access

WebRtcNet will expose a static `WebRtcNet.Host` as the non-browser equivalent of `Navigator`, with `Host.MediaDevices` and `Host.CreatePeerConnection(...)` as the public entry points for client code. We chose this to remove direct `WebRtcInterop` usage from applications while preserving the existing abstract managed/native layering in `WebRtcNet.Api`. We rejected static injection on API model types because type-initialization order is implicit and brittle compared to an explicit root surface.
