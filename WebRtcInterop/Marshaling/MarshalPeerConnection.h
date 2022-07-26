#pragma once

#include <msclr/marshal.h>

#include "MarshalCollections.h"
#include "api/peer_connection_interface.h"

namespace msclr::interop
{
	template <>
	inline webrtc::PeerConnectionInterface::RTCOfferAnswerOptions marshal_as(WebRtcNet::RtcOfferOptions^ const & from)
	{
		webrtc::PeerConnectionInterface::RTCOfferAnswerOptions to;
		to.offer_to_receive_video = from->OfferToReceiveVideo;
		to.offer_to_receive_audio = from->OfferToReceiveAudio;
		to.voice_activity_detection = from->VoiceActivityDetection;
		to.ice_restart = from->IceRestart;
		return to;
	};

	template <>
	inline WebRtcNet::RtcOfferOptions^ marshal_as(const webrtc::PeerConnectionInterface::RTCOfferAnswerOptions& from)
	{
		auto to = gcnew WebRtcNet::RtcOfferOptions();
		to->OfferToReceiveVideo = from.offer_to_receive_video;
		to->OfferToReceiveAudio = from.offer_to_receive_audio;
		to->VoiceActivityDetection = from.voice_activity_detection;
		to->IceRestart = from.ice_restart;
		return to;
	}

	template <>
	inline WebRtcNet::RtcSignalingState marshal_as(const webrtc::PeerConnectionInterface::SignalingState& from)
	{
		switch (from)
		{
		case webrtc::PeerConnectionInterface::SignalingState::kStable:
			return WebRtcNet::RtcSignalingState::Stable;
		case webrtc::PeerConnectionInterface::SignalingState::kHaveLocalOffer:
			return WebRtcNet::RtcSignalingState::HaveLocalOffer;
		case webrtc::PeerConnectionInterface::SignalingState::kHaveLocalPrAnswer:
			return WebRtcNet::RtcSignalingState::HaveLocalPrAnswer;
		case webrtc::PeerConnectionInterface::SignalingState::kHaveRemoteOffer:
			return WebRtcNet::RtcSignalingState::HaveRemoteOffer;
		case webrtc::PeerConnectionInterface::SignalingState::kHaveRemotePrAnswer:
			return WebRtcNet::RtcSignalingState::HaveRemotePrAnswer;
		case webrtc::PeerConnectionInterface::SignalingState::kClosed:
			return WebRtcNet::RtcSignalingState::Closed;
		}

		throw gcnew InvalidCastException("Invalid PeerConnectionInterface::RtcSignalingState value.");
	}

	template <>
	inline WebRtcNet::RtcIceConnectionState marshal_as(const webrtc::PeerConnectionInterface::IceConnectionState& from)
	{
		switch (from)
		{
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionNew:
			return WebRtcNet::RtcIceConnectionState::New;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionChecking:
			return WebRtcNet::RtcIceConnectionState::Checking;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionConnected:
			return WebRtcNet::RtcIceConnectionState::Connected;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionCompleted:
			return WebRtcNet::RtcIceConnectionState::Completed;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionFailed:
			return WebRtcNet::RtcIceConnectionState::Failed;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionDisconnected:
			return WebRtcNet::RtcIceConnectionState::Disconnected;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionClosed:
			return WebRtcNet::RtcIceConnectionState::Closed;
		case webrtc::PeerConnectionInterface::IceConnectionState::kIceConnectionMax: //Currently unused.
			throw gcnew NotSupportedException("IceConnectionMax is currently not supported.");
		}

		throw gcnew InvalidCastException(
			String::Format("Invalid PeerConnectionInterface::IceConnectionState value - {0}.", static_cast<int>(from)));
	}

	template <>
	inline WebRtcNet::RtcIceGatheringState marshal_as(const webrtc::PeerConnectionInterface::IceGatheringState& from)
	{
		switch (from)
		{
		case webrtc::PeerConnectionInterface::IceGatheringState::kIceGatheringNew:
			return WebRtcNet::RtcIceGatheringState::New;
		case webrtc::PeerConnectionInterface::IceGatheringState::kIceGatheringGathering:
			return WebRtcNet::RtcIceGatheringState::Gathering;
		case webrtc::PeerConnectionInterface::IceGatheringState::kIceGatheringComplete:
			return WebRtcNet::RtcIceGatheringState::Complete;
		}

		throw gcnew InvalidCastException(
			String::Format("Invalid PeerConnectionInterface::IceGatheringState value - {0}.", static_cast<int>(from)));
	}

	template <>
	inline WebRtcNet::RtcIceTransportPolicy marshal_as(const webrtc::PeerConnectionInterface::IceTransportsType& from)
	{
		switch (from)
		{
		case webrtc::PeerConnectionInterface::IceTransportsType::kNone:
			return WebRtcNet::RtcIceTransportPolicy::None;
		case webrtc::PeerConnectionInterface::IceTransportsType::kNoHost: //not supported
			throw gcnew NotSupportedException("The NoHost Ice Transport Policy is currently not supported.");
		case webrtc::PeerConnectionInterface::IceTransportsType::kRelay:
			return WebRtcNet::RtcIceTransportPolicy::Relay;
		case webrtc::PeerConnectionInterface::IceTransportsType::kAll:
			return WebRtcNet::RtcIceTransportPolicy::All;
		}

		throw gcnew InvalidCastException(
			String::Format("Invalid PeerConnectionInterface::IceTransportsType value - {0}.", static_cast<int>(from)));
	}

	template <>
	inline WebRtcNet::RtcBundlePolicy marshal_as(const webrtc::PeerConnectionInterface::BundlePolicy& from)
	{
		//note that the int value assignment is different between the managed and native versions. The manageed enum 
		//folows the order specified in http://www.w3.org/TR/webrtc/#rtcbundlepolicy-enum, and the native enum does not.
		switch (from)
		{
		case webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyBalanced:
			return WebRtcNet::RtcBundlePolicy::Balanced;
		case webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxBundle:
			return WebRtcNet::RtcBundlePolicy::MaxBundle;
		case webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat:
			return WebRtcNet::RtcBundlePolicy::MaxCompat;
		}

		throw gcnew InvalidCastException(String::Format("Invalid PeerConnectionInterface::BundlePolicy value - {0}.",
		                                                static_cast<int>(from)));
	}

	template <>
	inline WebRtcNet::RtcSdpType marshal_as(const std::string& from)
	{
		if (from.compare(webrtc::SessionDescriptionInterface::kOffer) == 0)
		{
			return WebRtcNet::RtcSdpType::Offer;
		}

		if (from.compare(webrtc::SessionDescriptionInterface::kAnswer) == 0)
		{
			return WebRtcNet::RtcSdpType::Answer;
		}

		if (from.compare(webrtc::SessionDescriptionInterface::kPrAnswer) == 0)
		{
			return WebRtcNet::RtcSdpType::PrAnswer;
		}

		if (from.compare(webrtc::SessionDescriptionInterface::kRollback) == 0)
		{
			return WebRtcNet::RtcSdpType::Rollback;
		}

		throw gcnew InvalidCastException(String::Format("Invalid RtcSdpType value '{0}'.", marshal_as<String^>(from)));
	}

	template <>
	inline WebRtcNet::RtcSessionDescription marshal_as(webrtc::SessionDescriptionInterface* const & from)
	{
		auto from_type = marshal_as<WebRtcNet::RtcSdpType>(from->type());
		std::string from_sdp;
		if (!from->ToString(&from_sdp))
		{
			throw gcnew InvalidCastException("SessionDescription has invalid SDP");
		}
		auto sdp = marshal_as<String^>(from_sdp);

		return WebRtcNet::RtcSessionDescription(from_type, sdp);
	}
}
