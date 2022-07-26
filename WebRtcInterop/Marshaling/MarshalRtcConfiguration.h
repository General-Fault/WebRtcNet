#pragma once

#include "MarshalCollections.h"
#include "MarshalIceServer.h"
#include "MarshalPeerConnection.h"
#include "api/peer_connection_interface.h"

namespace msclr { namespace interop
{
	template<>
	inline webrtc::PeerConnectionInterface::IceTransportsType marshal_as(RtcIceTransportPolicy const & from)
	{
		switch (from)
		{
		case RtcIceTransportPolicy::Relay:
			return webrtc::PeerConnectionInterface::IceTransportsType::kRelay;
		case RtcIceTransportPolicy::All:
			return webrtc::PeerConnectionInterface::IceTransportsType::kAll;
		case RtcIceTransportPolicy::None:
			return webrtc::PeerConnectionInterface::IceTransportsType::kNone;
		//case WebRtcNet::RtcIceTransportPolicy::NoHost: // No support for nohost at the moment.
		//	return webrtc::PeerConnectionInterface::IceTransportsType::kNoHost;
		}

		throw gcnew InvalidCastException(String::Format("Invalid RtcIceTransportPolicy - '{0}'.", from.ToString()));
	}

	template<>
	inline webrtc::PeerConnectionInterface::BundlePolicy marshal_as(RtcBundlePolicy const & from)
	{
		switch (from)
		{
		case RtcBundlePolicy::Balanced:
			return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyBalanced;
		case RtcBundlePolicy::MaxBundle:
			return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxBundle;
		case RtcBundlePolicy::MaxCompat:
			return webrtc::PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat;
		}

		throw gcnew InvalidCastException(String::Format("Invalid RtcBundlePolicy - '{0}'.", from.ToString()));
	}

	template<>
	inline webrtc::PeerConnectionInterface::RTCConfiguration marshal_as(RtcConfiguration^ const & from)
	{
		auto servers = safe_cast<IEnumerable<RtcIceServer^>^>(from->IceServers);

		webrtc::PeerConnectionInterface::RTCConfiguration to;
		to.type = marshal_as<webrtc::PeerConnectionInterface::IceTransportsType>((RtcIceTransportPolicy)from->IceTransportPolicy);
		to.servers = marshal_enumerable_as<webrtc::PeerConnectionInterface::IceServer, RtcIceServer^>(servers);

		to.bundle_policy = marshal_as<webrtc::PeerConnectionInterface::BundlePolicy>((RtcBundlePolicy)from->BundlePolicy);

		return to;
	};


	template <>
	inline RtcConfiguration^ marshal_as(const webrtc::PeerConnectionInterface::RTCConfiguration & from)
	{

		auto servers = gcnew List<RtcIceServer^>(
			marshal_vector_as<RtcIceServer^, webrtc::PeerConnectionInterface::IceServer>(from.servers));

		auto to = gcnew RtcConfiguration(servers);

		to->IceTransportPolicy = marshal_as<RtcIceTransportPolicy>(from.type);
		to->BundlePolicy = marshal_as<RtcBundlePolicy>(from.bundle_policy);

		//to->RtcpMuxPolicy = marshal<WebRtcInterop::RtcpMuxPolicy>(from.rtcp_mux_policy);
		//to->TcpCandidatePolicy = marshal<WebRtcInterop::TcpCandidatePolicy>(from.tcp_candidate_policy);
		//to->AudioJitterBufferMaxPackets = from.audio_jitter_buffer_max_packets;
		//to->AudioJitterBufferFastAccelerate = from.audio_jitter_buffer_fast_accelerate;

		return to;
	}
}}
