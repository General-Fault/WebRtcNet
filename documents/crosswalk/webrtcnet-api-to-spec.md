# WebRtcNet.Api to W3C Spec Crosswalk

Status: seeded

This crosswalk maps high-value `WebRtcNet.Api` symbols to current W3C specification anchors and records expected adaptation notes for .NET usage.

## WebRTC 1.0

| API Symbol | Spec Section | Anchor URL | Status | Adaptation Notes |
| --- | --- | --- | --- | --- |
| `RtcPeerConnection` | RTCPeerConnection interface | <https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection> | partial | JS promise/event model maps to `Task` and .NET events; partial interop implementation remains in some members. |
| `RtcConfiguration` | RTCConfiguration dictionary | <https://www.w3.org/TR/webrtc/#rtcconfiguration-dictionary> | partial | Dictionary members map to C# properties; enum/value coercion rules need explicit validation behavior in managed API. |
| `RtcIceCandidate` | RTCIceCandidate interface | <https://www.w3.org/TR/webrtc/#dom-rtcicecandidate> | implemented | Constructor/dictionary parsing maps well; candidate parsing behavior should preserve spec terminology in XML docs. |
| `RtcIceCandidateErrorEventArgs` | RTCPeerConnectionIceErrorEvent interface | <https://www.w3.org/TR/webrtc/#rtcpeerconnectioniceerrorevent> | implemented | Payload exposes `address`/`port` as nullable and `url`/`errorCode`/`errorText` as required fields aligned to the spec event payload. |
| `RtcRtpSender` | RTCRtpSender interface | <https://www.w3.org/TR/webrtc/#dom-rtcrtpsender> | partial | Promise-returning operations map to `Task`; parameter mutation semantics require interop parity checks. |
| `RtcRtpReceiver` | RTCRtpReceiver interface | <https://www.w3.org/TR/webrtc/#dom-rtcrtpreceiver> | partial | Contract includes nullable `jitterBufferTarget` and receiver parameter/stat APIs; parity remains partial while interop coverage is expanded. |
| `RtcRtpTransceiver` | RTCRtpTransceiver interface | <https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver> | partial | Direction/currentDirection state alignment requires nullability-safe representation in .NET. |
| `RtcRtpParameters` | RTP interfaces and dictionaries | <https://www.w3.org/TR/webrtc/#rtcrtpinterface> | partial | Includes RID modeling via `RtcRtpCodingParameters`; continue tracking dictionary evolution for forward-compatible defaults. |
| `RtcDataChannel` | RTCDataChannel interface | <https://www.w3.org/TR/webrtc/#dom-rtcdatachannel> | implemented | Event handler attributes become C# events/delegates; binary type semantics require explicit managed mapping. |
| `RtcPeerConnection.GetStats` | `getStats` operation | <https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-getstats> | partial | Return contract maps to managed report interfaces; detailed dictionary parity delegated to webrtc-stats mapping. |

## Media Capture and Streams

| API Symbol | Spec Section | Anchor URL | Status | Adaptation Notes |
| --- | --- | --- | --- | --- |
| `MediaDevices` (`Host.MediaDevices`) | MediaDevices interface and `Navigator.mediaDevices` access pattern | <https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-getusermedia> | partial | `Navigator` is adapted to static `WebRtcNet.Host`; `Host.MediaDevices` provides same-object access while permission/device selection remains host-platform dependent. |
| `MediaStream` | MediaStream interface | <https://www.w3.org/TR/mediacapture-streams/#mediastream> | implemented | ID/events map to managed interfaces; source/track ownership lifetimes must stay explicit in wrapper docs. |
| `MediaStreamTrack` | MediaStreamTrack interface | <https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-applyconstraints> | partial | `applyConstraints()` maps to nullable optional argument in C# (`MediaTrackConstraints?`). |
| `MediaStreamConstraints` | MediaStreamConstraints dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints> | partial | JS union bool/object semantics represented with typed C# properties and constraint wrappers; local-IDL parity assertions cover bool/object member mapping in `MediaStreamConstraintsIdlParityTests`. |
| `MediaTrackConstraints` | MediaTrackConstraints dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraints> | partial | Constrain* exact/ideal model implemented via dedicated nested constraint types, including ordered `Advanced` constraint-set entries; continue parity pass for remaining edge members. |
| `MediaTrackCapabilities` | MediaTrackCapabilities dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities> | partial | Sequence-valued capability members map to CLR collections (`IReadOnlyList<T>`) with no legacy scalar compatibility adapters; scalar spec members remain scalar. |
| `MediaTrackSettings` | MediaTrackSettings dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings> | partial | Settings are snapshot values in C# POCO form; ensure naming and nullability match spec optionality. |
| `MediaTrackConstraintSet` usage | Constraint set semantics | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraintset> | partial | Spec uses loose dictionaries; .NET model exposes a dedicated `MediaTrackConstraintSet` POCO for strongly-typed reusable sets (including `MediaTrackConstraints.Advanced`). |
| `Constraint processing behavior` | Constraints algorithm | <https://www.w3.org/TR/mediacapture-streams/#constraints> | partial | API contract now exposes required-constraint detection and ordered advanced-set processing semantics; local-IDL parity assertions cover advanced ordering/validation and required-vs-ideal detection in `MediaTrackConstraintTests`; device capability feasibility remains interop/runtime dependent. |

## WebRTC Stats

| API Symbol | Spec Section | Anchor URL | Status | Adaptation Notes |
| --- | --- | --- | --- | --- |
| `IRtcStatsReport` | RTCStatsReport object model | <https://www.w3.org/TR/webrtc-stats/#rtcstatsreport-object> | partial | JS map-like iteration maps to typed managed report interface. |
| `RtcStatsReport` | RTCStatsReport interface terms | <https://www.w3.org/TR/webrtc-stats/#dom-rtcstatsreport> | partial | CLR data structures should preserve ID/type/timestamp semantics from spec dictionaries. |
| `Inbound RTP stats` | `RTCInboundRtpStreamStats` | <https://www.w3.org/TR/webrtc-stats/#dom-rtcinboundrtpstreamstats> | todo | Expand concrete managed dictionary classes and property coverage. |
| `Outbound RTP stats` | `RTCOutboundRtpStreamStats` | <https://www.w3.org/TR/webrtc-stats/#dom-rtcoutboundrtpstreamstats> | todo | Expand concrete managed dictionary classes and property coverage. |
| `Transport stats` | `RTCTransportStats` | <https://www.w3.org/TR/webrtc-stats/#dom-rtctransportstats> | todo | Add/verify transport-level statistics representation. |

## Next Pass Priorities

1. Confirm each `partial`/`todo` row against current interop implementation status.
2. Add direct issue links for each uncovered gap.
3. Annotate each row with unit/integration test coverage references.
