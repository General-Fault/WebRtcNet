using System;

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
/// Represents an SDP session description produced by <see cref="RtcPeerConnection.CreateOffer"/> or
/// <see cref="RtcPeerConnection.CreateAnswer"/>, or received from a remote peer via signaling.
/// Models <c>RTCSessionDescriptionInit</c> where <c>type</c> is required.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcsessiondescription-class" />
public record struct RtcSessionDescription
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
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsessiondescriptioninit-type" />
	public RtcSdpType Type { get; init; }

	/// <summary>
	/// Gets the SDP payload string.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsessiondescriptioninit-sdp" />
	public string Sdp { get; init; } = string.Empty;

	/// <summary>
	/// Implicitly converts an <see cref="RtcLocalSessionDescriptionInit"/> to an
	/// <see cref="RtcSessionDescription"/>. Throws if <see cref="RtcLocalSessionDescriptionInit.Type"/>
	/// is <see langword="null"/>, as <see cref="Type"/> is required.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <see cref="RtcLocalSessionDescriptionInit.Type"/> is <see langword="null"/>.
	/// </exception>
	public static implicit operator RtcSessionDescription(RtcLocalSessionDescriptionInit description) =>
		new(description.Type ?? throw new InvalidOperationException(
				"Cannot convert RtcLocalSessionDescriptionInit to RtcSessionDescription: Type is null."),
			description.Sdp);
}

/// <summary>
/// The description passed to <see cref="RtcPeerConnection.SetLocalDescription"/>.
/// Models <c>RTCLocalSessionDescriptionInit</c> where <c>type</c> is optional — the implementation
/// may infer the type when it is omitted.
/// </summary>
/// <remarks>
/// Implicitly converts from <see cref="RtcSessionDescription"/> so that the value returned by
/// <see cref="RtcPeerConnection.CreateOffer"/> or <see cref="RtcPeerConnection.CreateAnswer"/>
/// can be passed directly to <see cref="RtcPeerConnection.SetLocalDescription"/> without a cast.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtclocalsessiondescriptioninit" />
public record struct RtcLocalSessionDescriptionInit
{
	/// <summary>
	/// Initializes a local session description init with an optional type and SDP payload.
	/// </summary>
	/// <param name="type">
	/// The SDP description type, or <see langword="null"/> to let the implementation infer the type
	/// from the current signaling state.
	/// </param>
	/// <param name="sdp">The SDP payload text. Defaults to empty string.</param>
	public RtcLocalSessionDescriptionInit(RtcSdpType? type = null, string sdp = "")
	{
		Type = type;
		Sdp = sdp;
	}

	/// <summary>
	/// Gets the SDP description type, or <see langword="null"/> if the implementation should infer it.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtclocalsessiondescriptioninit-type" />
	public RtcSdpType? Type { get; init; }

	/// <summary>
	/// Gets the SDP payload string.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtclocalsessiondescriptioninit-sdp" />
	public string Sdp { get; init; } = string.Empty;

	/// <summary>
	/// Implicitly converts an <see cref="RtcSessionDescription"/> (required type) to an
	/// <see cref="RtcLocalSessionDescriptionInit"/> (optional type).
	/// </summary>
	public static implicit operator RtcLocalSessionDescriptionInit(RtcSessionDescription description) =>
		new(description.Type, description.Sdp);
}
