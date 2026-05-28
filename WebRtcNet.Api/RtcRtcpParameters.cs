namespace WebRtcNet;

/// <summary>
/// RTCP parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtcpparameters"/>
/// <seealso cref="RtcRtpParameters"/>
public sealed record class RtcRtcpParameters
{
    public RtcRtcpParameters()
    {
    }

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
