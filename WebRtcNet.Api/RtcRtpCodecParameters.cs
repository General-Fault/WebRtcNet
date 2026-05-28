namespace WebRtcNet;

/// <summary>
/// RTP codec parameters dictionary.
/// </summary>
/// <seealso cref="RtcRtpParameters.Codecs"/>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpcodecparameters"/>
public sealed record RtcRtpCodecParameters
{
	public RtcRtpCodecParameters()
	{
	}

	public RtcRtpCodecParameters(byte payloadType, string mimeType, ulong clockRate, short channels, string sdpFmtpLine)
	{
		PayloadType = payloadType;
		MimeType = mimeType ?? string.Empty;
		ClockRate = clockRate;
		Channels = channels;
		SdpFmtpLine = sdpFmtpLine ?? string.Empty;
	}

	public byte PayloadType { get; set; }

	public string MimeType { get; set; } = string.Empty;

	public ulong ClockRate { get; set; }

	public short Channels { get; set; }

	public string SdpFmtpLine { get; set; } = string.Empty;
}
