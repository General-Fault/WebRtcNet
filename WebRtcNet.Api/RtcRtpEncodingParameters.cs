namespace WebRtcNet;

/// <summary>
/// RTP encoding parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters"/>
public sealed record RtcRtpEncodingParameters
{
	public RtcRtpEncodingParameters()
	{
	}

	public RtcRtpEncodingParameters(bool active, ulong? maxBitrate, double scaleResolutionDownBy)
		: this(active, maxBitrate, scaleResolutionDownBy, null, null)
	{
	}

	public RtcRtpEncodingParameters(
		bool active, ulong? maxBitrate, double scaleResolutionDownBy, RtcRtpCodec codec, double? maxFramerate)
	{
		Active = active;
		MaxBitrate = maxBitrate;
		ScaleResolutionDownBy = scaleResolutionDownBy;
		Codec = codec;
		MaxFramerate = maxFramerate;
	}

	/// <summary>
	/// Indicates whether this encoding is active.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-active"/>
	public bool Active { get; set; }

	/// <summary>
	/// The maximum bitrate for this encoding, or null when unspecified.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-maxbitrate"/>
	public ulong? MaxBitrate { get; set; }

	/// <summary>
	/// The codec selected for this encoding, or null when not specified.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-codec"/>
	public RtcRtpCodec Codec { get; set; } = null;

	/// <summary>
	/// The maximum framerate for this encoding, or null when unspecified.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-maxframerate"/>
	public double? MaxFramerate { get; set; }

	/// <summary>
	/// The scale factor used to reduce the resolution of the encoded media.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-scaleresolutiondownby"/>
	public double ScaleResolutionDownBy { get; set; }
}
