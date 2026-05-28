namespace WebRtcNet;

/// <summary>
/// RTP encoding parameters dictionary.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters"/>
public sealed record class RtcRtpEncodingParameters
{
    public RtcRtpEncodingParameters()
    {
    }

    public RtcRtpEncodingParameters(bool active, ulong? maxBitrate, double scaleResolutionDownBy)
    {
        Active = active;
        MaxBitrate = maxBitrate;
        ScaleResolutionDownBy = scaleResolutionDownBy;
    }

    public bool Active { get; set; }

    public ulong? MaxBitrate { get; set; }

    public double ScaleResolutionDownBy { get; set; }
}
