namespace WebRtcNet;

/// <summary>
/// RTP header extension parameters dictionary.
/// </summary>
/// <seealso cref="RtcRtpParameters"/>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpheaderextensionparameters"/>
public sealed record RtcRtpHeaderExtensionParameters
{
	/// <summary>
	/// Initializes header extension parameters with default values.
	/// </summary>
	public RtcRtpHeaderExtensionParameters()
	{
	}

	/// <summary>
	/// Initializes header extension parameters.
	/// </summary>
	/// <param name="uri">The extension URI.</param>
	/// <param name="id">The negotiated extension ID.</param>
	/// <param name="encrypted">Whether the extension is encrypted.</param>
	public RtcRtpHeaderExtensionParameters(string uri, ushort id, bool encrypted)
	{
		Uri = uri ?? string.Empty;
		Id = id;
		Encrypted = encrypted;
	}

	/// <summary>
	/// Gets or sets the extension URI.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpheaderextensionparameters-uri" />
	public string Uri { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the negotiated extension ID.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpheaderextensionparameters-id" />
	public ushort Id { get; set; }

	/// <summary>
	/// Gets or sets whether the extension is encrypted.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpheaderextensionparameters-encrypted" />
	public bool Encrypted { get; set; }
}
