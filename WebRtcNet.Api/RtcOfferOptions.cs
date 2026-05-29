namespace WebRtcNet;

/// <summary>
/// Base options dictionary used by offer/answer creation operations.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#offer-answer-options" />
public record RtcOfferAnswerOptions
{
}

/// <summary>
/// Used by <see cref="RtcPeerConnection.CreateOffer"/>
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcofferoptions" />
/// <seealso cref="RtcPeerConnection.CreateOffer"/>
public record RtcOfferOptions : RtcOfferAnswerOptions
{
	/// <summary>
	/// When true, generated offer credentials differ from the current local description, forcing an ICE restart
	/// when the offer is applied.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcofferoptions-icerestart" />
	public bool IceRestart { get; init; } = false;
}

/// <summary>
/// Used by <see cref="RtcPeerConnection.CreateAnswer"/>
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcansweroptions"/>
/// <seealso cref="RtcPeerConnection.CreateAnswer"/>
public record RtcAnswerOptions : RtcOfferAnswerOptions;