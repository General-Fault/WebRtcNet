namespace WebRtcNet;

/// <summary>
/// RTP header extension parameters dictionary.
/// </summary>
/// <seealso cref="RtcRtpParameters"/>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpheaderextensionparameters"/>
public sealed record RtcRtpHeaderExtensionParameters
{
	public RtcRtpHeaderExtensionParameters()
	{
	}

	public RtcRtpHeaderExtensionParameters(string uri, ushort id, bool encrypted)
	{
		Uri = uri ?? string.Empty;
		Id = id;
		Encrypted = encrypted;
	}

	public string Uri { get; set; } = string.Empty;

	public ushort Id { get; set; }

	public bool Encrypted { get; set; }
}
