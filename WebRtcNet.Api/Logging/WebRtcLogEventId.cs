namespace WebRtcNet.Logging;

/// <summary>
/// Event ID constants for structured logging across WebRtcNet layers.
/// Event IDs are grouped by category to enable telemetry aggregation.
/// </summary>
public enum WebRtcLogEventId
{
	// WebRTC events (1000-1999)
	// PeerConnection events (1000-1099)
	/// <summary>Peer connection state transition.</summary>
	PeerConnectionStateChanged = 1000,
	/// <summary>Peer connection created.</summary>
	PeerConnectionCreated = 1001,
	/// <summary>Peer connection closed.</summary>
	PeerConnectionClosed = 1002,
	/// <summary>Peer connection error.</summary>
	PeerConnectionError = 1003,

	// DataChannel events (1100-1199)
	/// <summary>Data channel opened.</summary>
	DataChannelOpened = 1100,
	/// <summary>Data channel closed.</summary>
	DataChannelClosed = 1101,
	/// <summary>Data channel error.</summary>
	DataChannelError = 1102,

	// Media events (1200-1299)
	/// <summary>Audio device initialized.</summary>
	AudioDeviceInitialized = 1200,
	/// <summary>Video device initialized.</summary>
	VideoDeviceInitialized = 1201,
	/// <summary>Audio processing warning.</summary>
	AudioProcessingWarning = 1202,

	// Transport/ICE events (1300-1399)
	/// <summary>ICE state changed.</summary>
	IceStateChanged = 1300,
	/// <summary>ICE gathering state changed.</summary>
	IceGatheringStateChanged = 1301,
	/// <summary>ICE connection error.</summary>
	IceConnectionError = 1302,

	// Codec/RTP events (1400-1499)
	/// <summary>Codec negotiation event.</summary>
	CodecNegotiation = 1400,
	/// <summary>RTP statistics event.</summary>
	RtpStats = 1401,

	// Audio Processing events (1500-1599)
	/// <summary>Acoustic echo cancellation warning.</summary>
	AecWarning = 1500,
	/// <summary>Noise suppression warning.</summary>
	NoiseSuppressionWarning = 1501,

	// Other WebRTC events (1600-1699)
	/// <summary>Uncategorized WebRTC event.</summary>
	WebRtcOther = 1600,

	// Media enumeration events (2000-2099)
	/// <summary>Audio device enumeration started.</summary>
	AudioEnumerationStarted = 2000,
	/// <summary>Audio device enumeration failed.</summary>
	AudioEnumerationFailed = 2001,
	/// <summary>Video device enumeration started.</summary>
	VideoEnumerationStarted = 2002,
	/// <summary>Video device enumeration failed.</summary>
	VideoEnumerationFailed = 2003,
	/// <summary>Audio capability query completed successfully.</summary>
	AudioCapabilityQueryCompleted = 2004,
	/// <summary>Video DirectShow capability scan completed.</summary>
	VideoCapabilityQueryCompleted = 2005,

	// Media constraints events (2100-2199)
	/// <summary>Constraint validation event.</summary>
	ConstraintValidation = 2100,
	/// <summary>Constraint validation error.</summary>
	ConstraintError = 2101,

	// Interop errors (3000-3099)
	/// <summary>Interop media devices failure.</summary>
	InteropMediaDevicesFailed = 3000,
	/// <summary>Interop peer connection failure.</summary>
	InteropPeerConnectionFailed = 3001,
	/// <summary>Interop HRESULT failure.</summary>
	InteropHResultFailure = 3002,
}
