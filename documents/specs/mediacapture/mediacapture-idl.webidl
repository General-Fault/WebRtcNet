// Source: https://www.w3.org/TR/mediacapture-streams/
// Extracted on: 2026-05-28
// IDL blocks: 44

WebIDL[Exposed=Window]
interface MediaStream : EventTarget {
  constructor();
  constructor(MediaStream stream);
  constructor(sequence<MediaStreamTrack> tracks);
  readonly attribute DOMString id;
  sequence<MediaStreamTrack> getAudioTracks();
  sequence<MediaStreamTrack> getVideoTracks();
  sequence<MediaStreamTrack> getTracks();
  MediaStreamTrack? getTrackById(DOMString trackId);
  undefined addTrack(MediaStreamTrack track);
  undefined removeTrack(MediaStreamTrack track);
  MediaStream clone();
  readonly attribute boolean active;
  attribute EventHandler onaddtrack;
  attribute EventHandler onremovetrack;
};

WebIDL[Exposed=Window]
interface MediaStreamTrack : EventTarget {
  readonly attribute DOMString kind;
  readonly attribute DOMString id;
  readonly attribute DOMString label;
  attribute boolean enabled;
  readonly attribute boolean muted;
  attribute EventHandler onmute;
  attribute EventHandler onunmute;
  readonly attribute MediaStreamTrackState readyState;
  attribute EventHandler onended;
  MediaStreamTrack clone();
  undefined stop();
  MediaTrackCapabilities getCapabilities();
  MediaTrackConstraints getConstraints();
  MediaTrackSettings getSettings();
  Promise<undefined> applyConstraints(optional MediaTrackConstraints constraints = {});
};

WebIDLenum MediaStreamTrackState {
  "live",
  "ended"
};

WebIDLdictionary MediaTrackSupportedConstraints {
  boolean width = true;
  boolean height = true;
  boolean aspectRatio = true;
  boolean frameRate = true;
  boolean facingMode = true;
  boolean resizeMode = true;
  boolean sampleRate = true;
  boolean sampleSize = true;
  boolean echoCancellation = true;
  boolean autoGainControl = true;
  boolean noiseSuppression = true;
  boolean latency = true;
  boolean channelCount = true;
  boolean deviceId = true;
  boolean groupId = true;
  boolean backgroundBlur = true;
};

WebIDLdictionary MediaTrackCapabilities {
  ULongRange width;
  ULongRange height;
  DoubleRange aspectRatio;
  DoubleRange frameRate;
  sequence<DOMString> facingMode;
  sequence<DOMString> resizeMode;
  ULongRange sampleRate;
  ULongRange sampleSize;
  sequence<(boolean or DOMString)> echoCancellation;
  sequence<boolean> autoGainControl;
  sequence<boolean> noiseSuppression;
  DoubleRange latency;
  ULongRange channelCount;
  DOMString deviceId;
  DOMString groupId;
  sequence<boolean> backgroundBlur;
};

WebIDLdictionary MediaTrackConstraints : MediaTrackConstraintSet {
  sequence<MediaTrackConstraintSet> advanced;
};

WebIDLdictionary MediaTrackConstraintSet {
  ConstrainULong width;
  ConstrainULong height;
  ConstrainDouble aspectRatio;
  ConstrainDouble frameRate;
  ConstrainDOMString facingMode;
  ConstrainDOMString resizeMode;
  ConstrainULong sampleRate;
  ConstrainULong sampleSize;
  ConstrainBooleanOrDOMString echoCancellation;
  ConstrainBoolean autoGainControl;
  ConstrainBoolean noiseSuppression;
  ConstrainDouble latency;
  ConstrainULong channelCount;
  ConstrainDOMString deviceId;
  ConstrainDOMString groupId;
  ConstrainBoolean backgroundBlur;
};

WebIDLdictionary MediaTrackSettings {
  unsigned long width;
  unsigned long height;
  double aspectRatio;
  double frameRate;
  DOMString facingMode;
  DOMString resizeMode;
  unsigned long sampleRate;
  unsigned long sampleSize;
  (boolean or DOMString) echoCancellation;
  boolean autoGainControl;
  boolean noiseSuppression;
  double latency;
  unsigned long channelCount;
  DOMString deviceId;
  DOMString groupId;
  boolean backgroundBlur;
};

WebIDLenum VideoFacingModeEnum {
  "user",
  "environment",
  "left",
  "right"
};

WebIDLenum VideoResizeModeEnum {
  "none",
  "crop-and-scale"
};

WebIDLenum EchoCancellationModeEnum {
  "all",
  "remote-only"
};

WebIDL[Exposed=Window]
interface MediaStreamTrackEvent : Event {
  constructor(DOMString type, MediaStreamTrackEventInit eventInitDict);
  [SameObject] readonly attribute MediaStreamTrack track;
};

WebIDLdictionary MediaStreamTrackEventInit : EventInit {
  required MediaStreamTrack track;
};

WebIDL[Exposed=Window]
interface OverconstrainedError : DOMException {
  constructor(DOMString constraint, optional DOMString message = "");
  readonly attribute DOMString constraint;
};

WebIDLpartial interface Navigator {
  [SameObject, SecureContext] readonly attribute MediaDevices mediaDevices;
};

WebIDL[Exposed=Window, SecureContext]
interface MediaDevices : EventTarget {
  attribute EventHandler ondevicechange;
  Promise<sequence<MediaDeviceInfo>> enumerateDevices();
};

WebIDL[Exposed=Window, SecureContext]
interface MediaDeviceInfo {
  readonly attribute DOMString deviceId;
  readonly attribute MediaDeviceKind kind;
  readonly attribute DOMString label;
  readonly attribute DOMString groupId;
  [Default] object toJSON();
};

WebIDLenum MediaDeviceKind {
  "audioinput",
  "audiooutput",
  "videoinput"
};

WebIDL[Exposed=Window, SecureContext]
interface InputDeviceInfo : MediaDeviceInfo {
  MediaTrackCapabilities getCapabilities();
};

WebIDL[Exposed=Window]
interface DeviceChangeEvent : Event {
  constructor(DOMString type, optional DeviceChangeEventInit eventInitDict = {});
  [SameObject] readonly attribute FrozenArray<MediaDeviceInfo> devices;
  [SameObject] readonly attribute FrozenArray<MediaDeviceInfo> userInsertedDevices;
};

WebIDLdictionary DeviceChangeEventInit : EventInit {
  sequence<MediaDeviceInfo> devices = [];
};

WebIDLpartial interface MediaDevices {
  MediaTrackSupportedConstraints getSupportedConstraints();
  Promise<MediaStream> getUserMedia(optional MediaStreamConstraints constraints = {});
};

WebIDLdictionary MediaStreamConstraints {
  (boolean or MediaTrackConstraints) video = false;
  (boolean or MediaTrackConstraints) audio = false;
};

WebIDLpartial interface Navigator {
  [SecureContext] undefined getUserMedia(MediaStreamConstraints constraints,
                                    NavigatorUserMediaSuccessCallback successCallback,
                                    NavigatorUserMediaErrorCallback errorCallback);
};

WebIDLcallback NavigatorUserMediaSuccessCallback = undefined (MediaStream stream);

WebIDLcallback NavigatorUserMediaErrorCallback = undefined (DOMException error);

WebIDL[Exposed=Window]
interface ConstrainablePattern {
  Capabilities  getCapabilities();
  Constraints   getConstraints();
  Settings      getSettings();
  Promise<undefined> applyConstraints(optional Constraints constraints = {});
};

WebIDLdictionary DoubleRange {
  double max;
  double min;
};

WebIDLdictionary ConstrainDoubleRange : DoubleRange {
  double exact;
  double ideal;
};

WebIDLdictionary ULongRange {
  [Clamp] unsigned long max;
  [Clamp] unsigned long min;
};

WebIDLdictionary ConstrainULongRange : ULongRange {
  [Clamp] unsigned long exact;
  [Clamp] unsigned long ideal;
};

WebIDLdictionary ConstrainBooleanParameters {
  boolean exact;
  boolean ideal;
};

WebIDLdictionary ConstrainDOMStringParameters {
  (DOMString or sequence<DOMString>) exact;
  (DOMString or sequence<DOMString>) ideal;
};

WebIDLdictionary ConstrainBooleanOrDOMStringParameters {
  (boolean or DOMString) exact;
  (boolean or DOMString) ideal;
};

WebIDLtypedef ([Clamp] unsigned long or ConstrainULongRange) ConstrainULong;

WebIDLtypedef (double or ConstrainDoubleRange) ConstrainDouble;

WebIDLtypedef (boolean or ConstrainBooleanParameters) ConstrainBoolean;

WebIDLtypedef (DOMString or
         sequence<DOMString> or
         ConstrainDOMStringParameters) ConstrainDOMString;

WebIDLtypedef (boolean or DOMString or ConstrainBooleanOrDOMStringParameters) ConstrainBooleanOrDOMString;

WebIDLdictionary Capabilities {};

WebIDLdictionary Settings {};

WebIDLdictionary ConstraintSet {};

WebIDLdictionary Constraints : ConstraintSet {
  sequence<ConstraintSet> advanced;
};

WebIDLdictionary CameraDevicePermissionDescriptor : PermissionDescriptor {
  boolean panTiltZoom = false;
};
