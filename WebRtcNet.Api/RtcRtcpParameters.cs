namespace WebRtcNet;

/// <summary>
/// RTCP parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtcpparameters"/>
/// <seealso cref="RtcRtpParameters"/>
public sealed record RtcRtcpParameters
{
	/// <summary>
	/// Initializes RTCP parameters with default values.
	/// </summary>
	public RtcRtcpParameters()
	{
	}

	/// <summary>
	/// Initializes RTCP parameters.
	/// </summary>
	/// <param name="cName">The RTCP canonical name.</param>
	/// <param name="reducedSize">Whether reduced-size RTCP is enabled.</param>
	public RtcRtcpParameters(string cName, bool reducedSize)
	{
		CName = cName ?? string.Empty;
		ReducedSize = reducedSize;
	}

	/// <summary>
	/// The Canonical Name (CNAME) used by RTCP (e.g. in SDES messages).
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtcpparameters-cname"/>
	public string CName { get; set; } = string.Empty;

	/// <summary>
	/// Whether reduced size RTCP is configured.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtcpparameters-reducedsize"/>
	public bool ReducedSize { get; set; }
}
