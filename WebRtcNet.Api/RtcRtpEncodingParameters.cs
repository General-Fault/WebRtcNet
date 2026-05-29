namespace WebRtcNet;

/// <summary>
/// RTP encoding parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters"/>
public sealed record RtcRtpEncodingParameters
{
	/// <summary>
	/// Initializes encoding parameters with default values.
	/// </summary>
	public RtcRtpEncodingParameters()
	{
	}

	/// <summary>
	/// Initializes encoding parameters without explicitly selecting a codec or max frame rate.
	/// </summary>
	/// <param name="active">Whether the encoding is active.</param>
	/// <param name="maxBitrate">The maximum bitrate in bits per second when specified.</param>
	/// <param name="scaleResolutionDownBy">The downscale factor for encoded media resolution.</param>
	public RtcRtpEncodingParameters(bool active, ulong? maxBitrate, double scaleResolutionDownBy)
		: this(active, maxBitrate, scaleResolutionDownBy, null, null)
	{
	}

	/// <summary>
	/// Initializes encoding parameters.
	/// </summary>
	/// <param name="active">Whether the encoding is active.</param>
	/// <param name="maxBitrate">The maximum bitrate in bits per second when specified.</param>
	/// <param name="scaleResolutionDownBy">The downscale factor for encoded media resolution.</param>
	/// <param name="codec">The codec selected for this encoding when specified.</param>
	/// <param name="maxFramerate">The maximum frame rate when specified.</param>
	public RtcRtpEncodingParameters(
		bool active, ulong? maxBitrate, double scaleResolutionDownBy, RtcRtpCodec? codec, double? maxFramerate)
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
	public RtcRtpCodec? Codec { get; set; }

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
