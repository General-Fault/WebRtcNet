namespace WebRtcNet;

/// <summary>
/// Describes the type of SDP payload represented by an <see cref="RtcSessionDescription" />.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsdptype" />
public enum RtcSdpType
{
	/// <summary>
	/// Indicates the description is an SDP offer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsdptype-offer" />
	Offer,

	/// <summary>
	/// Indicates the description is a provisional SDP answer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsdptype-pranswer" />
	PrAnswer,

	/// <summary>
	/// Indicates the description is a final SDP answer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsdptype-answer" />
	Answer,

	/// <summary>
	/// Indicates the description rolls back the current local or remote description.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsdptype-rollback" />
	Rollback,
}

/// <summary>
/// Represents an SDP session description used by <see cref="RtcPeerConnection" /> operations.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsessiondescriptioninit" />
public struct RtcSessionDescription
{
	/// <summary>
	/// Initializes a session description with a type and SDP payload.
	/// </summary>
	/// <param name="type">The SDP description type.</param>
	/// <param name="sdp">The SDP payload text.</param>
	public RtcSessionDescription(RtcSdpType type, string sdp)
	{
		Type = type;
		Sdp = sdp;
	}

	/// <summary>
	/// Gets the SDP description type.
	/// </summary>
	public readonly RtcSdpType Type;

	/// <summary>
	/// Gets the SDP payload string.
	/// </summary>
	public readonly string Sdp;
}