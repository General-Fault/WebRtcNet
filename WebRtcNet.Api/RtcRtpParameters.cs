using System.Collections.Generic;
using System.Linq;

namespace WebRtcNet;

/// <summary>
/// RTP parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpparameters" />
public record RtcRtpParameters
{
	/// <summary>
	/// Initializes RTP parameters.
	/// </summary>
	/// <param name="headerExtensions">Optional RTP header extensions.</param>
	/// <param name="rtcp">Optional RTCP parameters.</param>
	/// <param name="codecs">Optional codec parameters.</param>
	public RtcRtpParameters(
		IEnumerable<RtcRtpHeaderExtensionParameters>? headerExtensions = null,
		RtcRtcpParameters? rtcp = null,
		IEnumerable<RtcRtpCodecParameters>? codecs = null)
	{
		HeaderExtensions = headerExtensions?.ToList() ?? [];
		Rtcp = rtcp ?? new RtcRtcpParameters();
		Codecs = codecs?.ToList() ?? [];
	}

	/// <summary>
	/// Gets or sets the RTP header extension parameters.
	/// </summary>
	public List<RtcRtpHeaderExtensionParameters> HeaderExtensions { get; set; } = [];

	/// <summary>
	/// Gets or sets RTCP parameters.
	/// </summary>
	public RtcRtcpParameters Rtcp { get; set; } = new();

	/// <summary>
	/// Gets or sets codec parameters.
	/// </summary>
	public List<RtcRtpCodecParameters> Codecs { get; set; } = [];
}

/// <summary>
/// RTP sender parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcsendrtpparameters" />
public record RtcRtpSendParameters : RtcRtpParameters
{
	/// <summary>
	/// Initializes sender RTP parameters with default values.
	/// </summary>
	public RtcRtpSendParameters()
	{
	}

	/// <summary>
	/// Initializes sender RTP parameters.
	/// </summary>
	/// <param name="headerExtensions">RTP header extension parameters.</param>
	/// <param name="rtcp">RTCP parameters.</param>
	/// <param name="codecs">Codec parameters.</param>
	/// <param name="transactionId">A transaction identifier for the parameter snapshot.</param>
	/// <param name="encodings">Encoding parameter entries.</param>
	public RtcRtpSendParameters(
		IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions,
		RtcRtcpParameters rtcp,
		IEnumerable<RtcRtpCodecParameters> codecs,
		string transactionId,
		IEnumerable<RtcRtpEncodingParameters> encodings)
		: base(headerExtensions, rtcp, codecs)
	{
		TransactionId = transactionId ?? string.Empty;
		Encodings = encodings?.ToList() ?? [];
	}

	/// <summary>
	/// Gets or sets the transaction identifier for this parameter set.
	/// </summary>
	public string TransactionId { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets encoding parameters for this sender.
	/// </summary>
	public List<RtcRtpEncodingParameters> Encodings { get; set; } = [];
}

/// <summary>
/// RTP receiver parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcreceivertpparameters" />
public record RtcRtpReceiveParameters : RtcRtpParameters
{
	/// <summary>
	/// Initializes receiver RTP parameters with default values.
	/// </summary>
	public RtcRtpReceiveParameters()
	{
	}

	/// <summary>
	/// Initializes receiver RTP parameters.
	/// </summary>
	/// <param name="headerExtensions">RTP header extension parameters.</param>
	/// <param name="rtcp">RTCP parameters.</param>
	/// <param name="codecs">Codec parameters.</param>
	public RtcRtpReceiveParameters(
		IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions,
		RtcRtcpParameters rtcp,
		IEnumerable<RtcRtpCodecParameters> codecs)
		: base(headerExtensions, rtcp, codecs)
	{
	}
}
