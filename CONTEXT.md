# WebRtcNet Domain Glossary

This file is a glossary of canonical terms used in the WebRtcNet codebase.
It contains **no implementation details** — only term definitions.

---

## Terms

### Host
A peer that initiates a WebRTC session by opening a TCP listener and waiting for
an incoming connection before creating an SDP offer.

### Guest
A peer that joins a WebRTC session by dialling the Host's IP address and port,
then responding to the Host's SDP offer with an SDP answer.

### Signaling
The out-of-band exchange of SDP offers/answers and ICE candidates between two
peers. In this repository the term refers specifically to message exchange; it
does not imply a server. BasicVideoChat uses direct TCP signaling.

### SignalingMessage
A newline-delimited JSON envelope carrying one of: `Offer`, `Answer`,
`Candidate`, or `Bye`.

### BasicVideoChat
The first example application. Demonstrates a peer-to-peer audio/video call over
a direct TCP signaling channel, targeting both .NET 10 and .NET Framework 4.8.

### WpfVideoRenderer
A prototype `VideoRenderer` implementation backed by a WPF `WriteableBitmap`.
Lives in `examples/BasicVideoChat` until a proper `WebRtcNet.Wpf` renderer
assembly is created (see issue #36).
