namespace WebRtcNet;

/// <summary>
/// Shared RTP codec shape used by codec capability and parameter dictionaries.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpcodec"/>
public record RtcRtpCodec
{
	/// <summary>
	/// Initializes codec values with defaults.
	/// </summary>
	public RtcRtpCodec()
	{
	}

	/// <summary>
	/// Initializes codec values.
	/// </summary>
	/// <param name="mimeType">The codec MIME media type/subtype.</param>
	/// <param name="clockRate">The codec clock rate in hertz.</param>
	/// <param name="channels">The channel count when specified.</param>
	/// <param name="sdpFmtpLine">The SDP fmtp parameters string.</param>
	public RtcRtpCodec(string mimeType, ulong clockRate, ushort? channels, string sdpFmtpLine)
	{
		MimeType = mimeType ?? string.Empty;
		ClockRate = clockRate;
		Channels = channels;
		SdpFmtpLine = sdpFmtpLine ?? string.Empty;
	}

	/// <summary>
	/// The codec MIME media type/subtype.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodec-mimetype"/>
	public string MimeType { get; set; } = string.Empty;

	/// <summary>
	/// The codec clock rate expressed in Hertz.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodec-clockrate"/>
	public ulong ClockRate { get; set; }

	/// <summary>
	/// The maximum number of channels supported by this codec, or null when unspecified.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodec-channels"/>
	public ushort? Channels { get; set; }

	/// <summary>
	/// The format-specific parameters from the codec's SDP fmtp line, if any.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodec-sdpfmtpline"/>
	public string SdpFmtpLine { get; set; } = string.Empty;
}

/// <summary>
/// RTP codec parameters dictionary.
/// </summary>
/// <seealso cref="RtcRtpParameters.Codecs"/>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpcodecparameters"/>
public record RtcRtpCodecParameters : RtcRtpCodec
{
	/// <summary>
	/// Initializes codec parameters with default values.
	/// </summary>
	public RtcRtpCodecParameters()
	{
	}

	/// <summary>
	/// Initializes codec parameters.
	/// </summary>
	/// <param name="payloadType">The RTP payload type for this codec.</param>
	/// <param name="mimeType">The codec MIME media type/subtype.</param>
	/// <param name="clockRate">The codec clock rate in hertz.</param>
	/// <param name="channels">The channel count when specified.</param>
	/// <param name="sdpFmtpLine">The SDP fmtp parameters string.</param>
	public RtcRtpCodecParameters(byte payloadType, string mimeType, ulong clockRate, ushort? channels, string sdpFmtpLine)
		: base(mimeType, clockRate, channels, sdpFmtpLine)
	{
		PayloadType = payloadType;
	}

	/// <summary>
	/// The RTP payload type associated with this codec.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodecparameters-payloadtype"/>
	public byte PayloadType { get; set; }
}
