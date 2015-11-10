#pragma once

WEBRTCNET_START

public ref class RtcOfferOptions
{
public:
	/// In some cases, an RTCPeerConnection may wish to receive video but not send any video. 
	/// The RTCPeerConnection needs to know if it should signal to the remote side whether it wishes to receive video or not. 
	/// This option allows an application to indicate its preferences for the number of video 
	/// streams to receive when creating an offer.
	property Int32 OfferToReceiveVideo { Int32 get(); void set(Int32 value); }

	/// In some cases, an RTCPeerConnection may wish to receive audio but not send any audio. 
	/// The RTCPeerConnection needs to know if it should signal to the remote side whether it wishes to receive audio. 
	/// This option allows an application to indicate its preferences for the number of 
	/// audio streams to receive when creating an offer.
	property Int32 OfferToReceiveAudio { Int32 get(); void set(Int32 value); }

	/// Many codecs and system are capable of detecting "silence" and changing their behavior in this case by doing things 
	/// such as not transmitting any media.In many cases, such as when dealing with emergency calling or sounds other than spoken 
	/// voice, it is desirable to be able to turn off this behavior.This option allows the application to provide information 
	/// about whether it wishes this type of processing enabled or disabled.
	property Boolean VoiceActivityDetection { Boolean get(); void set(Boolean value); }

	
	/// When the value of this dictionary member is true, the generated description will have ICE credentials 
	/// that are different from the current credentials (as visible in the localDescription attribute's SDP). 
	/// Applying the generated description will restart ICE.
	/// When the value of this dictionary member is false, and the localDescription attribute has valid ICE credentials, 
	/// the generated description will have the same ICE credentials as the current value from the localDescription attribute.
	property Boolean IceRestart { Boolean get(); void set(Boolean value); }

internal:
	RtcOfferOptions();
};

WEBRTCNET_END