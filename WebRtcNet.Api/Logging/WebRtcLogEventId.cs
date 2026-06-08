namespace WebRtcNet.Logging;

/// <summary>
/// Event ID constants for structured logging across WebRtcNet layers.
/// Event IDs are grouped by category to enable telemetry aggregation.
/// </summary>
public enum WebRtcLogEventId : int
{
	// WebRTC events (1000-1999)
	// PeerConnection events (1000-1099)
	PeerConnectionStateChanged = 1000,
	PeerConnectionCreated = 1001,
	PeerConnectionClosed = 1002,
	PeerConnectionError = 1003,

	// DataChannel events (1100-1199)
	DataChannelOpened = 1100,
	DataChannelClosed = 1101,
	DataChannelError = 1102,

	// Media events (1200-1299)
	AudioDeviceInitialized = 1200,
	VideoDeviceInitialized = 1201,
	AudioProcessingWarning = 1202,

	// Transport/ICE events (1300-1399)
	IceStateChanged = 1300,
	IceGatheringStateChanged = 1301,
	IceConnectionError = 1302,

	// Codec/RTP events (1400-1499)
	CodecNegotiation = 1400,
	RtpStats = 1401,

	// Audio Processing events (1500-1599)
	AecWarning = 1500,
	NoiseSuppressionWarning = 1501,

	// Other WebRTC events (1600-1699)
	WebRtcOther = 1600,

	// Media enumeration events (2000-2099)
	AudioEnumerationStarted = 2000,
	AudioEnumerationFailed = 2001,
	VideoEnumerationStarted = 2002,
	VideoEnumerationFailed = 2003,

	// Media constraints events (2100-2199)
	ConstraintValidation = 2100,
	ConstraintError = 2101,

	// Interop errors (3000-3099)
	InteropMediaDevicesFailed = 3000,
	InteropPeerConnectionFailed = 3001,
	InteropHResultFailure = 3002,
}
