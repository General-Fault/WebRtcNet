# WebRtcNet.Api to W3C Spec Crosswalk

Status: seeded

This crosswalk maps high-value `WebRtcNet.Api` symbols to current W3C specification anchors and records expected adaptation notes for .NET usage.

## WebRTC 1.0

| API Symbol | Spec Section | Anchor URL | Status | Adaptation Notes |
| --- | --- | --- | --- | --- |
| `IRtcPeerConnection` | RTCPeerConnection interface | <https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection> | partial | JS promise/event model maps to `Task` and .NET events; partial interop implementation remains in some members. |
| `RtcConfiguration` | RTCConfiguration dictionary | <https://www.w3.org/TR/webrtc/#rtcconfiguration-dictionary> | partial | Dictionary members map to C# properties; enum/value coercion rules need explicit validation behavior in managed API. |
| `RtcIceCandidate` | RTCIceCandidate interface | <https://www.w3.org/TR/webrtc/#dom-rtcicecandidate> | implemented | Constructor/dictionary parsing maps well; candidate parsing behavior should preserve spec terminology in XML docs. |
| `IRtcRtpSender` | RTCRtpSender interface | <https://www.w3.org/TR/webrtc/#dom-rtcrtpsender> | partial | Promise-returning operations map to `Task`; parameter mutation semantics require interop parity checks. |
| `IRtcRtpTransceiver` | RTCRtpTransceiver interface | <https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver> | partial | Direction/currentDirection state alignment requires nullability-safe representation in .NET. |
| `RtcRtpParameters` | RTP interfaces and dictionaries | <https://www.w3.org/TR/webrtc/#rtcrtpinterface> | partial | Dictionary evolution may add members not present yet; keep forward-compatible defaults. |
| `IRtcDataChannel` | RTCDataChannel interface | <https://www.w3.org/TR/webrtc/#dom-rtcdatachannel> | implemented | Event handler attributes become C# events/delegates; binary type semantics require explicit managed mapping. |
| `IRtcPeerConnection.GetStats` | `getStats` operation | <https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-getstats> | partial | Return contract maps to managed report interfaces; detailed dictionary parity delegated to webrtc-stats mapping. |

## Media Capture and Streams

| API Symbol | Spec Section | Anchor URL | Status | Adaptation Notes |
| --- | --- | --- | --- | --- |
| `IMediaDevices` | MediaDevices interface | <https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-getusermedia> | partial | JS permission/device selection model maps to host-dependent .NET platform behavior. |
| `IMediaStream` | MediaStream interface | <https://www.w3.org/TR/mediacapture-streams/#mediastream> | implemented | ID/events map to managed interfaces; source/track ownership lifetimes must stay explicit in wrapper docs. |
| `IMediaStreamTrack` | MediaStreamTrack interface | <https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-applyconstraints> | partial | `applyConstraints()` maps to nullable optional argument in C# (`MediaTrackConstraints?`). |
| `MediaStreamConstraints` | MediaStreamConstraints dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints> | partial | JS union bool/object semantics represented with typed C# properties and constraint wrappers. |
| `MediaTrackConstraints` | MediaTrackConstraints dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraints> | partial | Constrain* exact/ideal model implemented via dedicated nested constraint types; continue parity pass for edge members. |
| `MediaTrackCapabilities` | MediaTrackCapabilities dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities> | partial | Capability unions/lists map to CLR collections and nullable values. |
| `MediaTrackSettings` | MediaTrackSettings dictionary | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings> | partial | Settings are snapshot values in C# POCO form; ensure naming and nullability match spec optionality. |
| `MediaTrackConstraintSet` usage | Constraint set semantics | <https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraintset> | partial | Spec uses loose dictionaries; .NET model enforces stronger typing and validation hooks. |
| `Constraint processing behavior` | Constraints algorithm | <https://www.w3.org/TR/mediacapture-streams/#constraints> | todo | Algorithmic parity docs/tests need explicit mapping to current interop behavior. |

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
