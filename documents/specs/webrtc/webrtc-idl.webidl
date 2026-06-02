// Source: https://www.w3.org/TR/webrtc/
// Extracted on: 2026-05-28
// IDL blocks: 85

WebIDLdictionary RTCConfiguration {
  sequence<RTCIceServer> iceServers = [];
  RTCIceTransportPolicy iceTransportPolicy = "all";
  RTCBundlePolicy bundlePolicy = "balanced";
  RTCRtcpMuxPolicy rtcpMuxPolicy = "require";
  sequence<RTCCertificate> certificates = [];
  [EnforceRange] octet iceCandidatePoolSize = 0;
};

WebIDLdictionary RTCIceServer {
  required (DOMString or sequence<DOMString>) urls;
  DOMString username;
  DOMString credential;
};

WebIDLenum RTCIceTransportPolicy {
  "relay",
  "all"
};

WebIDLenum RTCBundlePolicy {
  "balanced",
  "max-compat",
  "max-bundle"
};

WebIDLenum RTCRtcpMuxPolicy {
  "require"
};

WebIDLdictionary RTCOfferAnswerOptions {};

WebIDLdictionary RTCOfferOptions : RTCOfferAnswerOptions {
  boolean iceRestart = false;
};

WebIDLdictionary RTCAnswerOptions : RTCOfferAnswerOptions {};

WebIDLenum RTCSignalingState {
  "stable",
  "have-local-offer",
  "have-remote-offer",
  "have-local-pranswer",
  "have-remote-pranswer",
  "closed"
};

WebIDLenum RTCIceGatheringState {
  "new",
  "gathering",
  "complete"
};

WebIDLenum RTCPeerConnectionState {
  "closed",
  "failed",
  "disconnected",
  "new",
  "connecting",
  "connected"
};

WebIDLenum RTCIceConnectionState {
  "closed",
  "failed",
  "disconnected",
  "new",
  "checking",
  "completed",
  "connected"
};

WebIDL[Exposed=Window]
interface RTCPeerConnection : EventTarget  {
  constructor(optional RTCConfiguration configuration = {});
  Promise<RTCSessionDescriptionInit> createOffer(optional RTCOfferOptions options = {});
  Promise<RTCSessionDescriptionInit> createAnswer(optional RTCAnswerOptions options = {});
  Promise<undefined> setLocalDescription(optional RTCLocalSessionDescriptionInit description = {});
  readonly attribute RTCSessionDescription? localDescription;
  readonly attribute RTCSessionDescription? currentLocalDescription;
  readonly attribute RTCSessionDescription? pendingLocalDescription;
  Promise<undefined> setRemoteDescription(RTCSessionDescriptionInit description);
  readonly attribute RTCSessionDescription? remoteDescription;
  readonly attribute RTCSessionDescription? currentRemoteDescription;
  readonly attribute RTCSessionDescription? pendingRemoteDescription;
  Promise<undefined> addIceCandidate(optional RTCIceCandidateInit candidate = {});
  readonly attribute RTCSignalingState signalingState;
  readonly attribute RTCIceGatheringState iceGatheringState;
  readonly attribute RTCIceConnectionState iceConnectionState;
  readonly attribute RTCPeerConnectionState connectionState;
  readonly attribute boolean? canTrickleIceCandidates;
  undefined restartIce();
  RTCConfiguration getConfiguration();
  undefined setConfiguration(optional RTCConfiguration configuration = {});
  undefined close();
  attribute EventHandler onnegotiationneeded;
  attribute EventHandler onicecandidate;
  attribute EventHandler onicecandidateerror;
  attribute EventHandler onsignalingstatechange;
  attribute EventHandler oniceconnectionstatechange;
  attribute EventHandler onicegatheringstatechange;
  attribute EventHandler onconnectionstatechange;

  // Legacy Interface Extensions
  // Supporting the methods in this section is optional.
  // If these methods are supported
  // they must be implemented as defined
  // in section "Legacy Interface Extensions"
  Promise<undefined> createOffer(RTCSessionDescriptionCallback successCallback,
                            RTCPeerConnectionErrorCallback failureCallback,
                            optional RTCOfferOptions options = {});
  Promise<undefined> setLocalDescription(RTCLocalSessionDescriptionInit description,
                                    VoidFunction successCallback,
                                    RTCPeerConnectionErrorCallback failureCallback);
  Promise<undefined> createAnswer(RTCSessionDescriptionCallback successCallback,
                             RTCPeerConnectionErrorCallback failureCallback);
  Promise<undefined> setRemoteDescription(RTCSessionDescriptionInit description,
                                     VoidFunction successCallback,
                                     RTCPeerConnectionErrorCallback failureCallback);
  Promise<undefined> addIceCandidate(RTCIceCandidateInit candidate,
                                VoidFunction successCallback,
                                RTCPeerConnectionErrorCallback failureCallback);
};

WebIDLcallback RTCPeerConnectionErrorCallback = undefined (DOMException error);

WebIDLcallback RTCSessionDescriptionCallback = undefined (RTCSessionDescriptionInit description);

WebIDLpartial dictionary RTCOfferOptions {
  boolean offerToReceiveAudio;
  boolean offerToReceiveVideo;
};

WebIDLenum RTCSdpType {
  "offer",
  "pranswer",
  "answer",
  "rollback"
};

WebIDL[Exposed=Window]
interface RTCSessionDescription {
  constructor(RTCSessionDescriptionInit descriptionInitDict);
  readonly attribute RTCSdpType type;
  readonly attribute DOMString sdp;
  [Default] RTCSessionDescriptionInit toJSON();
};

WebIDLdictionary RTCSessionDescriptionInit {
  required RTCSdpType type;
  DOMString sdp = "";
};

WebIDLdictionary RTCLocalSessionDescriptionInit {
  RTCSdpType type;
  DOMString sdp = "";
};

[Exposed=Window]
interface RTCIceCandidate {
  constructor(optional RTCIceCandidateInit candidateInitDict = {});
  readonly attribute DOMString candidate;
  readonly attribute DOMString? sdpMid;
  readonly attribute unsigned short? sdpMLineIndex;
  readonly attribute DOMString? foundation;
  readonly attribute RTCIceComponent? component;
  readonly attribute unsigned long? priority;
  readonly attribute DOMString? address;
  readonly attribute RTCIceProtocol? protocol;
  readonly attribute unsigned short? port;
  readonly attribute RTCIceCandidateType? type;
  readonly attribute RTCIceTcpCandidateType? tcpType;
  readonly attribute DOMString? relatedAddress;
  readonly attribute unsigned short? relatedPort;
  readonly attribute DOMString? usernameFragment;
  readonly attribute RTCIceServerTransportProtocol? relayProtocol;
  readonly attribute DOMString? url;
  RTCIceCandidateInit toJSON();
};

WebIDLdictionary RTCIceCandidateInit {
  DOMString candidate = "";
  DOMString? sdpMid = null;
  unsigned short? sdpMLineIndex = null;
  DOMString? usernameFragment = null;
};

WebIDLenum RTCIceProtocol {
  "udp",
  "tcp"
};

WebIDLenum RTCIceTcpCandidateType {
  "active",
  "passive",
  "so"
};

WebIDLenum RTCIceCandidateType {
  "host",
  "srflx",
  "prflx",
  "relay"
};

WebIDLenum RTCIceServerTransportProtocol {
  "udp",
  "tcp",
  "tls",
};

WebIDL[Exposed=Window]
interface RTCPeerConnectionIceEvent : Event {
  constructor(DOMString type, optional RTCPeerConnectionIceEventInit eventInitDict = {});
  readonly attribute RTCIceCandidate? candidate;
  readonly attribute DOMString? url;
};

WebIDLdictionary RTCPeerConnectionIceEventInit : EventInit {
  RTCIceCandidate? candidate;
  DOMString? url;
};

WebIDL[Exposed=Window]
interface RTCPeerConnectionIceErrorEvent : Event {
  constructor(DOMString type, RTCPeerConnectionIceErrorEventInit eventInitDict);
  readonly attribute DOMString? address;
  readonly attribute unsigned short? port;
  readonly attribute DOMString url;
  readonly attribute unsigned short errorCode;
  readonly attribute USVString errorText;
};

WebIDLdictionary RTCPeerConnectionIceErrorEventInit : EventInit {
  DOMString? address;
  unsigned short? port;
  DOMString url;
  required unsigned short errorCode;
  USVString errorText;
};

WebIDLpartial interface RTCPeerConnection {
  static Promise<RTCCertificate>
      generateCertificate(AlgorithmIdentifier keygenAlgorithm);
};

WebIDLdictionary RTCCertificateExpiration {
  [EnforceRange] unsigned long long expires;
};

WebIDL[Exposed=Window, Serializable]
interface RTCCertificate {
  readonly attribute EpochTimeStamp expires;
  sequence<RTCDtlsFingerprint> getFingerprints();
};

WebIDL          partial interface RTCPeerConnection {
  sequence<RTCRtpSender> getSenders();
  sequence<RTCRtpReceiver> getReceivers();
  sequence<RTCRtpTransceiver> getTransceivers();
  RTCRtpSender addTrack(MediaStreamTrack track, MediaStream... streams);
  undefined removeTrack(RTCRtpSender sender);
  RTCRtpTransceiver addTransceiver((MediaStreamTrack or DOMString) trackOrKind,
                                   optional RTCRtpTransceiverInit init = {});
  attribute EventHandler ontrack;
};

WebIDLdictionary RTCRtpTransceiverInit {
  RTCRtpTransceiverDirection direction = "sendrecv";
  sequence<MediaStream> streams = [];
  sequence<RTCRtpEncodingParameters> sendEncodings = [];
};

WebIDLenum RTCRtpTransceiverDirection {
  "sendrecv",
  "sendonly",
  "recvonly",
  "inactive",
  "stopped"
};

WebIDL[Exposed=Window]
interface RTCRtpSender {
  readonly attribute MediaStreamTrack? track;
  readonly attribute RTCDtlsTransport? transport;
  static RTCRtpCapabilities? getCapabilities(DOMString kind);
  Promise<undefined> setParameters(RTCRtpSendParameters parameters,
      optional RTCSetParameterOptions setParameterOptions = {});
  RTCRtpSendParameters getParameters();
  Promise<undefined> replaceTrack(MediaStreamTrack? withTrack);
  undefined setStreams(MediaStream... streams);
  Promise<RTCStatsReport> getStats();
};

WebIDLdictionary RTCRtpParameters {
  required sequence<RTCRtpHeaderExtensionParameters> headerExtensions;
  required RTCRtcpParameters rtcp;
  required sequence<RTCRtpCodecParameters> codecs;
};

WebIDLdictionary RTCRtpSendParameters : RTCRtpParameters {
  required DOMString transactionId;
  required sequence<RTCRtpEncodingParameters> encodings;
};

WebIDLdictionary RTCRtpReceiveParameters : RTCRtpParameters {
};

WebIDLdictionary RTCRtpCodingParameters {
  DOMString rid;
};

WebIDLdictionary RTCRtpEncodingParameters : RTCRtpCodingParameters {
  boolean active = true;
  RTCRtpCodec codec;
  unsigned long maxBitrate;
  double maxFramerate;
  double scaleResolutionDownBy;
};

WebIDLdictionary RTCRtcpParameters {
  DOMString cname;
  boolean reducedSize;
};

WebIDLdictionary RTCRtpHeaderExtensionParameters {
  required DOMString uri;
  required unsigned short id;
  boolean encrypted = false;
};

WebIDLdictionary RTCRtpCodec {
  required DOMString mimeType;
  required unsigned long clockRate;
  unsigned short channels;
  DOMString sdpFmtpLine;
};

WebIDLdictionary RTCRtpCodecParameters : RTCRtpCodec {
  required octet payloadType;
};

WebIDLdictionary RTCRtpCapabilities {
  required sequence<RTCRtpCodec> codecs;
  required sequence<RTCRtpHeaderExtensionCapability> headerExtensions;
};

WebIDLdictionary RTCRtpHeaderExtensionCapability {
  required DOMString uri;
};

WebIDLdictionary RTCSetParameterOptions {
};

WebIDL[Exposed=Window]
interface RTCRtpReceiver {
  readonly attribute MediaStreamTrack track;
  readonly attribute RTCDtlsTransport? transport;
  static RTCRtpCapabilities? getCapabilities(DOMString kind);
  RTCRtpReceiveParameters getParameters();
  sequence<RTCRtpContributingSource> getContributingSources();
  sequence<RTCRtpSynchronizationSource> getSynchronizationSources();
  Promise<RTCStatsReport> getStats();
  attribute DOMHighResTimeStamp? jitterBufferTarget;
};

WebIDLdictionary RTCRtpContributingSource {
  required DOMHighResTimeStamp timestamp;
  required unsigned long source;
  double audioLevel;
  required unsigned long rtpTimestamp;
};

WebIDLdictionary RTCRtpSynchronizationSource : RTCRtpContributingSource {};

WebIDL[Exposed=Window]
interface RTCRtpTransceiver {
  readonly attribute DOMString? mid;
  [SameObject] readonly attribute RTCRtpSender sender;
  [SameObject] readonly attribute RTCRtpReceiver receiver;
  attribute RTCRtpTransceiverDirection direction;
  readonly attribute RTCRtpTransceiverDirection? currentDirection;
  undefined stop();
  undefined setCodecPreferences(sequence<RTCRtpCodec> codecs);
};

WebIDL[Exposed=Window]
interface RTCDtlsTransport : EventTarget {
  [SameObject] readonly attribute RTCIceTransport iceTransport;
  readonly attribute RTCDtlsTransportState state;
  sequence<ArrayBuffer> getRemoteCertificates();
  attribute EventHandler onstatechange;
  attribute EventHandler onerror;
};

WebIDLenum RTCDtlsTransportState {
  "new",
  "connecting",
  "connected",
  "closed",
  "failed"
};

WebIDLdictionary RTCDtlsFingerprint {
  DOMString algorithm;
  DOMString value;
};

WebIDL[Exposed=Window]
interface RTCIceTransport : EventTarget {
  readonly attribute RTCIceRole role;
  readonly attribute RTCIceComponent component;
  readonly attribute RTCIceTransportState state;
  readonly attribute RTCIceGathererState gatheringState;
  sequence<RTCIceCandidate> getLocalCandidates();
  sequence<RTCIceCandidate> getRemoteCandidates();
  RTCIceCandidatePair? getSelectedCandidatePair();
  RTCIceParameters? getLocalParameters();
  RTCIceParameters? getRemoteParameters();
  attribute EventHandler onstatechange;
  attribute EventHandler ongatheringstatechange;
  attribute EventHandler onselectedcandidatepairchange;
};

WebIDLdictionary RTCIceParameters {
  DOMString usernameFragment;
  DOMString password;
};

dictionary [Exposed=Window]
interface RTCIceCandidatePair {
  [SameObject] readonly attribute RTCIceCandidate local;
  [SameObject] readonly attribute RTCIceCandidate remote;
};

WebIDLenum RTCIceGathererState {
  "new",
  "gathering",
  "complete"
};

WebIDLenum RTCIceTransportState {
  "closed",
  "failed",
  "disconnected",
  "new",
  "checking",
  "completed",
  "connected"
};

WebIDLenum RTCIceRole {
  "unknown",
  "controlling",
  "controlled"
};

WebIDLenum RTCIceComponent {
  "rtp",
  "rtcp"
};

WebIDL[Exposed=Window]
interface RTCTrackEvent : Event {
  constructor(DOMString type, RTCTrackEventInit eventInitDict);
  readonly attribute RTCRtpReceiver receiver;
  readonly attribute MediaStreamTrack track;
  [SameObject] readonly attribute FrozenArray<MediaStream> streams;
  readonly attribute RTCRtpTransceiver transceiver;
};

WebIDLdictionary RTCTrackEventInit : EventInit {
  required RTCRtpReceiver receiver;
  required MediaStreamTrack track;
  sequence<MediaStream> streams = [];
  required RTCRtpTransceiver transceiver;
};

WebIDL          partial interface RTCPeerConnection {
  readonly attribute RTCSctpTransport? sctp;
  RTCDataChannel createDataChannel(USVString label,
                                   optional RTCDataChannelInit dataChannelDict = {});
  attribute EventHandler ondatachannel;
};

WebIDL[Exposed=Window]
interface RTCSctpTransport : EventTarget {
  readonly attribute RTCDtlsTransport transport;
  readonly attribute RTCSctpTransportState state;
  readonly attribute unrestricted double maxMessageSize;
  readonly attribute unsigned short? maxChannels;
  attribute EventHandler onstatechange;
};

WebIDLenum RTCSctpTransportState {
  "connecting",
  "connected",
  "closed"
};

WebIDL[Exposed=(Window,DedicatedWorker), Transferable]
interface RTCDataChannel : EventTarget {
  readonly attribute USVString label;
  readonly attribute boolean ordered;
  readonly attribute unsigned short? maxPacketLifeTime;
  readonly attribute unsigned short? maxRetransmits;
  readonly attribute USVString protocol;
  readonly attribute boolean negotiated;
  readonly attribute unsigned short? id;
  readonly attribute RTCDataChannelState readyState;
  readonly attribute unsigned long bufferedAmount;
  [EnforceRange] attribute unsigned long bufferedAmountLowThreshold;
  attribute EventHandler onopen;
  attribute EventHandler onbufferedamountlow;
  attribute EventHandler onerror;
  attribute EventHandler onclosing;
  attribute EventHandler onclose;
  undefined close();
  attribute EventHandler onmessage;
  attribute BinaryType binaryType;
  undefined send(USVString data);
  undefined send(Blob data);
  undefined send(ArrayBuffer data);
  undefined send(ArrayBufferView data);
};

WebIDLdictionary RTCDataChannelInit {
  boolean ordered = true;
  [EnforceRange] unsigned short maxPacketLifeTime;
  [EnforceRange] unsigned short maxRetransmits;
  USVString protocol = "";
  boolean negotiated = false;
  [EnforceRange] unsigned short id;
};

WebIDLenum RTCDataChannelState {
  "connecting",
  "open",
  "closing",
  "closed"
};

WebIDL[Exposed=Window]
interface RTCDataChannelEvent : Event {
  constructor(DOMString type, RTCDataChannelEventInit eventInitDict);
  readonly attribute RTCDataChannel channel;
};

WebIDLdictionary RTCDataChannelEventInit : EventInit {
  required RTCDataChannel channel;
};

WebIDL          partial interface RTCRtpSender {
  readonly attribute RTCDTMFSender? dtmf;
};

WebIDL[Exposed=Window]
interface RTCDTMFSender : EventTarget {
  undefined insertDTMF(DOMString tones, optional unsigned long duration = 100, optional unsigned long interToneGap = 70);
  attribute EventHandler ontonechange;
  readonly attribute boolean canInsertDTMF;
  readonly attribute DOMString toneBuffer;
};

WebIDL[Exposed=Window]
interface RTCDTMFToneChangeEvent : Event {
  constructor(DOMString type, optional RTCDTMFToneChangeEventInit eventInitDict = {});
  readonly attribute DOMString tone;
};

WebIDL          dictionary RTCDTMFToneChangeEventInit : EventInit {
  DOMString tone = "";
};

WebIDL          partial interface RTCPeerConnection {
  Promise<RTCStatsReport> getStats(optional MediaStreamTrack? selector = null);
};

WebIDL[Exposed=Window]
interface RTCStatsReport {
  readonly maplike<DOMString, object>;
};

WebIDLdictionary RTCStats {
  required DOMHighResTimeStamp timestamp;
  required RTCStatsType type;
  required DOMString id;
};

WebIDL[Exposed=Window]
interface RTCError : DOMException {
  constructor(RTCErrorInit init, optional DOMString message = "");
  readonly attribute RTCErrorDetailType errorDetail;
  readonly attribute long? sdpLineNumber;
  readonly attribute long? sctpCauseCode;
  readonly attribute unsigned long? receivedAlert;
  readonly attribute unsigned long? sentAlert;
};

WebIDLdictionary RTCErrorInit {
  required RTCErrorDetailType errorDetail;
  long sdpLineNumber;
  long sctpCauseCode;
  unsigned long receivedAlert;
  unsigned long sentAlert;
};

WebIDLenum RTCErrorDetailType {
  "data-channel-failure",
  "dtls-failure",
  "fingerprint-failure",
  "sctp-failure",
  "sdp-syntax-error",
  "hardware-encoder-not-available",
  "hardware-encoder-error"
};

WebIDL[Exposed=Window]
interface RTCErrorEvent : Event {
  constructor(DOMString type, RTCErrorEventInit eventInitDict);
  [SameObject] readonly attribute RTCError error;
};

WebIDL          dictionary RTCErrorEventInit : EventInit {
  required RTCError error;
};
