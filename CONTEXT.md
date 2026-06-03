# WebRtcNet Domain Glossary

This file is a glossary of canonical terms used in the WebRtcNet codebase.
It contains **no implementation details** — only term definitions.

---

## Terms

### Host
The API root object used as the non-browser equivalent of `Navigator`, including
access to `MediaDevices`.

### Caller
A peer that initiates a WebRTC session by creating and sending the SDP offer.

### Callee
A peer that receives the SDP offer and responds with an SDP answer.

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

## Relationships

- **Host** exposes **MediaDevices** as the API entry point for capture access.
- A **Caller** sends an SDP offer and a **Callee** returns an SDP answer.

## Flagged ambiguities

- "Host" previously meant the signaling initiator peer; resolved to the API root
  object. Signaling roles are now **Caller** and **Callee**.
