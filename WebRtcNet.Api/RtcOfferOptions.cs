namespace WebRtcNet;

/// <summary>
/// Base options dictionary used by offer/answer creation operations.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#offer-answer-options" />
public record RtcOfferAnswerOptions
{
	/// <summary>
	/// When the value of this dictionary member is true, the generated description will have ICE credentials 
	/// that are different from the current credentials (as visible in the localDescription attribute's SDP). 
	/// Applying the generated description will restart ICE.
	/// When the value of this dictionary member is false, and the localDescription attribute has valid ICE credentials, 
	/// the generated description will have the same ICE credentials as the current value from the localDescription attribute.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcofferoptions-icerestart" />
	public bool IceRestart { get; init; } = false;

	/// <summary>
	/// Many codecs and system are capable of detecting "silence" and changing their behavior in this case by doing things 
	/// such as not transmitting any media.In many cases, such as when dealing with emergency calling or sounds other than spoken 
	/// voice, it is desirable to be able to turn off this behavior.This option allows the application to provide information 
	/// about whether it wishes this type of processing enabled or disabled.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#offer-answer-options" />
	public bool VoiceActivityDetection { get; init; } = true;
}

/// <summary>
/// Used by <see cref="IRtcPeerConnection.CreateOffer"/>
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcofferoptions" />
/// <seealso cref="IRtcPeerConnection.CreateOffer"/>
public record RtcOfferOptions : RtcOfferAnswerOptions;

/// <summary>
/// Used by <see cref="IRtcPeerConnection.CreateAnswer"/>
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcansweroptions"/>
/// <seealso cref="IRtcPeerConnection.CreateAnswer"/>
public record RtcAnswerOptions : RtcOfferAnswerOptions;