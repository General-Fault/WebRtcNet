#pragma once

#include "MarshalIceServer.h"
#include "MarshalPeerConnection.h"
#include "MarshalCollections.h"

#include "talk\app\webrtc\peerconnectioninterface.h"

namespace msclr {
	namespace interop
	{
		inline webrtc::PeerConnectionInterface::IceTransportsType marshal_as(WebRtcNet::RtcIceTransportPolicy from)
		{
			switch (from)
			{
			case WebRtcNet::RtcIceTransportPolicy::Relay:
				return webrtc::PeerConnectionInterface::IceTransportsType::kRelay;
			case WebRtcNet::RtcIceTransportPolicy::All:
				return webrtc::PeerConnectionInterface::IceTransportsType::kAll;
			case WebRtcNet::RtcIceTransportPolicy::None:
			default:
				return webrtc::PeerConnectionInterface::IceTransportsType::kNone;

				//no support for NoHost in the WebRtc spec.
			}
		}

		inline webrtc::PeerConnectionInterface::BundlePolicy marshal_as(WebRtcNet::RtcBundlePolicy from)
		{
			switch (from)
			{
			case WebRtcNet::RtcBundlePolicy::Balanced:
				return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyBalanced;
			case WebRtcNet::RtcBundlePolicy::MaxBundle:
				return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxBundle;
			case WebRtcNet::RtcBundlePolicy::MaxCompat:
			default:
				return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat;
			}
		}

		template<>
		inline webrtc::PeerConnectionInterface::RTCConfiguration marshal_as(WebRtcNet::RtcConfiguration ^ const & from)
		{
			auto servers = (IEnumerable<WebRtcNet::RtcIceServer ^> ^)from->IceServers;

			webrtc::PeerConnectionInterface::RTCConfiguration to;
			to.type = marshal_as(from->IceTransportPolicy);
			to.servers = marshal_enumerable_as<webrtc::PeerConnectionInterface::IceServer, WebRtcNet::RtcIceServer^>(servers);

			to.bundle_policy = marshal_as(from->BundlePolicy);

			return to;
		};


		template <>
		inline WebRtcNet::RtcConfiguration ^ marshal_as(const webrtc::PeerConnectionInterface::RTCConfiguration & from)
		{

			auto servers = gcnew List<WebRtcNet::RtcIceServer ^>(
				marshal_vector_as<WebRtcNet::RtcIceServer ^, webrtc::PeerConnection::IceServer>(from.servers));

			auto to = gcnew WebRtcNet::RtcConfiguration(servers);

			to->IceTransportPolicy = marshal_as(from.type);
			to->BundlePolicy = marshal_as(from.bundle_policy);

			//to->RtcpMuxPolicy = marshal_as<WebRtcInterop::RtcpMuxPolicy>(from.rtcp_mux_policy);
			//to->TcpCandidatePolicy = marshal_as<WebRtcInterop::TcpCandidatePolicy>(from.tcp_candidate_policy);
			//to->AudioJitterBufferMaxPackets = from.audio_jitter_buffer_max_packets;
			//to->AudioJitterBufferFastAccelerate = from.audio_jitter_buffer_fast_accelerate;

			return to;
		}
	}
}