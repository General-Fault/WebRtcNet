using System.Collections.Generic;
using System.Linq;

namespace WebRtcNet;

/// <summary>
/// RTP parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpparameters" />
public record RtcRtpParameters
{
	public RtcRtpParameters(
		IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions = null,
		RtcRtcpParameters rtcp = null,
		IEnumerable<RtcRtpCodecParameters> codecs = null)
	{
		HeaderExtensions = headerExtensions?.ToList() ?? [];
		Rtcp = rtcp ?? new RtcRtcpParameters();
		Codecs = codecs?.ToList() ?? [];
	}

	public List<RtcRtpHeaderExtensionParameters> HeaderExtensions { get; set; } = [];

	public RtcRtcpParameters Rtcp { get; set; } = new();

	public List<RtcRtpCodecParameters> Codecs { get; set; } = [];
}

/// <summary>
/// RTP sender parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcsendrtpparameters" />
public record RtcRtpSendParameters : RtcRtpParameters
{
	public RtcRtpSendParameters()
	{
	}

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

	public string TransactionId { get; set; } = string.Empty;

	public List<RtcRtpEncodingParameters> Encodings { get; set; } = [];
}

/// <summary>
/// RTP receiver parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcreceivertpparameters" />
public record RtcRtpReceiveParameters : RtcRtpParameters
{
	public RtcRtpReceiveParameters()
	{
	}

	public RtcRtpReceiveParameters(
		IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions,
		RtcRtcpParameters rtcp,
		IEnumerable<RtcRtpCodecParameters> codecs)
		: base(headerExtensions, rtcp, codecs)
	{
	}
}
