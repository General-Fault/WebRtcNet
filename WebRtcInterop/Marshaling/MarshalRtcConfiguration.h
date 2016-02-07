#pragma once

#include "MarshalIceServer.h"
#include "MarshalPeerConnection.h"
#include "MarshalCollections.h"

#include "talk\app\webrtc\peerconnectioninterface.h"

namespace msclr {
	namespace interop
	{
		template<>
		inline webrtc::PeerConnectionInterface::IceTransportsType marshal_as(WebRtcNet::RtcIceTransportPolicy const & from)
		{
			switch (from)
			{
			case WebRtcNet::RtcIceTransportPolicy::Relay:
				return webrtc::PeerConnectionInterface::IceTransportsType::kRelay;
			case WebRtcNet::RtcIceTransportPolicy::All:
				return webrtc::PeerConnectionInterface::IceTransportsType::kAll;
			case WebRtcNet::RtcIceTransportPolicy::None:
				return webrtc::PeerConnectionInterface::IceTransportsType::kNone;
			//case WebRtcNet::RtcIceTransportPolicy::NoHost: // No support for nohost at the moment.
			//	return webrtc::PeerConnectionInterface::IceTransportsType::kNoHost;
			}

			throw gcnew System::InvalidCastException(System::String::Format("Invalid RtcIceTransportPolicy - '{0}'.", from.ToString()));
		}

		template<>
		inline webrtc::PeerConnectionInterface::BundlePolicy marshal_as(WebRtcNet::RtcBundlePolicy const & from)
		{
			switch (from)
			{
			case WebRtcNet::RtcBundlePolicy::Balanced:
				return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyBalanced;
			case WebRtcNet::RtcBundlePolicy::MaxBundle:
				return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxBundle;
			case WebRtcNet::RtcBundlePolicy::MaxCompat:
				return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat;
			}

			throw gcnew System::InvalidCastException(System::String::Format("Invalid RtcBundlePolicy - '{0}'.", from.ToString()));
		}

		template<>
		inline webrtc::PeerConnectionInterface::RTCConfiguration marshal_as(WebRtcNet::RtcConfiguration ^ const & from)
		{
			auto servers = (System::Collections::Generic::IEnumerable<WebRtcNet::RtcIceServer ^> ^)from->IceServers;

			webrtc::PeerConnectionInterface::RTCConfiguration to;
			to.type = marshal_as<webrtc::PeerConnectionInterface::IceTransportsType>((WebRtcNet::RtcIceTransportPolicy)from->IceTransportPolicy);
			to.servers = marshal_enumerable_as<webrtc::PeerConnectionInterface::IceServer, WebRtcNet::RtcIceServer^>(servers);

			to.bundle_policy = marshal_as<webrtc::PeerConnectionInterface::BundlePolicy>((WebRtcNet::RtcBundlePolicy)from->BundlePolicy);

			return to;
		};


		template <>
		inline WebRtcNet::RtcConfiguration ^ marshal_as(const webrtc::PeerConnectionInterface::RTCConfiguration & from)
		{

			auto servers = gcnew System::Collections::Generic::List<WebRtcNet::RtcIceServer ^>(
				marshal_vector_as<WebRtcNet::RtcIceServer ^, webrtc::PeerConnectionInterface::IceServer>(from.servers));

			auto to = gcnew WebRtcNet::RtcConfiguration(servers);

			to->IceTransportPolicy = marshal_as<WebRtcNet::RtcIceTransportPolicy>(from.type);
			to->BundlePolicy = marshal_as<WebRtcNet::RtcBundlePolicy>(from.bundle_policy);

			//to->RtcpMuxPolicy = marshal<WebRtcInterop::RtcpMuxPolicy>(from.rtcp_mux_policy);
			//to->TcpCandidatePolicy = marshal<WebRtcInterop::TcpCandidatePolicy>(from.tcp_candidate_policy);
			//to->AudioJitterBufferMaxPackets = from.audio_jitter_buffer_max_packets;
			//to->AudioJitterBufferFastAccelerate = from.audio_jitter_buffer_fast_accelerate;

			return to;
		}
	}
}