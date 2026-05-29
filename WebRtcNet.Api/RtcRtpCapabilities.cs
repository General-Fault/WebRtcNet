using System.Collections.Generic;

namespace WebRtcNet;

/// <summary>
/// Represents a header extension capability.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpheaderextensioncapability-dictionary"/>
public sealed record RtcRtpHeaderExtensionCapability
{
	/// <summary>
	/// Gets or sets the header extension URI.
	/// </summary>
	public string Uri { get; init; } = string.Empty;
}

/// <summary>
/// Represents RTP capabilities for a sender or receiver.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpcapabilities-dictionary"/>
public sealed record RtcRtpCapabilities
{
	/// <summary>
	/// Gets or sets the supported codecs.
	/// </summary>
	public IReadOnlyList<RtcRtpCodecCapability> Codecs { get; init; } = [];

	/// <summary>
	/// Gets or sets the supported RTP header extensions.
	/// </summary>
	public IReadOnlyList<RtcRtpHeaderExtensionCapability> HeaderExtensions { get; init; } = [];
}
