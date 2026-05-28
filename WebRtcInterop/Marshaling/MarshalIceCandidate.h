#pragma once

#include "api/jsep.h"
#include "api/candidate.h"
#include "rtc_base/socket_address.h"

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <map>
#include <string>

#include "MarshalEnums.h"

namespace msclr { namespace interop
{
	using namespace System;

	static const std::map<const std::string, const WebRtcNet::RtcIceProtocol> ice_protocol_map {
		{"udp", WebRtcNet::RtcIceProtocol::Udp},
		{"tcp", WebRtcNet::RtcIceProtocol::Tcp},
	};

	static const std::map<const std::string, const WebRtcNet::RtcIceServerTransportProtocol> ice_server_transport_protocol_map {
		{"udp", WebRtcNet::RtcIceServerTransportProtocol::Udp},
		{"tcp", WebRtcNet::RtcIceServerTransportProtocol::Tcp},
		{"tls", WebRtcNet::RtcIceServerTransportProtocol::Tls},
	};

	static const std::map<const webrtc::IceCandidateType, const WebRtcNet::RtcIceCandidateType> ice_candidate_type_map {
		{webrtc::IceCandidateType::kHost, WebRtcNet::RtcIceCandidateType::Host},
		{webrtc::IceCandidateType::kSrflx, WebRtcNet::RtcIceCandidateType::Srflx},
		{webrtc::IceCandidateType::kPrflx, WebRtcNet::RtcIceCandidateType::Prflx},
		{webrtc::IceCandidateType::kRelay, WebRtcNet::RtcIceCandidateType::Relay},
	};

	static const std::map<const std::string, const WebRtcNet::RtcIceTcpCandidateType> ice_tcp_candidate_type_map {
		{webrtc::TCPTYPE_ACTIVE_STR, WebRtcNet::RtcIceTcpCandidateType::Active},
		{webrtc::TCPTYPE_PASSIVE_STR, WebRtcNet::RtcIceTcpCandidateType::Passive},
		{webrtc::TCPTYPE_SIMOPEN_STR, WebRtcNet::RtcIceTcpCandidateType::So},
	};

	static const std::map<const int, const WebRtcNet::RtcIceComponent> ice_component_map {
		{1, WebRtcNet::RtcIceComponent::Rtp},
		{2, WebRtcNet::RtcIceComponent::Rtcp},
	};

	namespace
	{
		inline Nullable<WebRtcNet::RtcIceProtocol> marshal_ice_protocol(const std::string& protocol)
		{
			const auto mapped = ice_protocol_map.find(protocol);
			if (mapped == ice_protocol_map.end())
			{
				throw gcnew InvalidCastException(String::Format(
					"Unable to convert native ICE protocol value '{0}' to {1}",
					marshal_as<String^>(protocol),
					WebRtcNet::RtcIceProtocol::typeid->FullName));
			}

			return Nullable<WebRtcNet::RtcIceProtocol>(mapped->second);
		}

		inline Nullable<WebRtcNet::RtcIceCandidateType> marshal_ice_candidate_type(webrtc::IceCandidateType type)
		{
			return Nullable<WebRtcNet::RtcIceCandidateType>(
				marshal_mapped_native_type(ice_candidate_type_map, type));
		}

		inline Nullable<WebRtcNet::RtcIceTcpCandidateType> marshal_ice_tcp_candidate_type(const std::string& tcptype)
		{
			if (tcptype.empty()) return Nullable<WebRtcNet::RtcIceTcpCandidateType>();

			const auto mapped = ice_tcp_candidate_type_map.find(tcptype);
			if (mapped == ice_tcp_candidate_type_map.end())
			{
				throw gcnew InvalidCastException(String::Format(
					"Unable to convert native ICE TCP candidate type value '{0}' to {1}",
					marshal_as<String^>(tcptype),
					WebRtcNet::RtcIceTcpCandidateType::typeid->FullName));
			}

			return Nullable<WebRtcNet::RtcIceTcpCandidateType>(mapped->second);
		}

		inline Nullable<WebRtcNet::RtcIceServerTransportProtocol> marshal_ice_server_transport_protocol(
			const std::string& protocol)
		{
			if (protocol.empty()) return Nullable<WebRtcNet::RtcIceServerTransportProtocol>();

			const auto mapped = ice_server_transport_protocol_map.find(protocol);
			if (mapped == ice_server_transport_protocol_map.end())
			{
				throw gcnew InvalidCastException(String::Format(
					"Unable to convert native ICE relay protocol value '{0}' to {1}",
					marshal_as<String^>(protocol),
					WebRtcNet::RtcIceServerTransportProtocol::typeid->FullName));
			}

			return Nullable<WebRtcNet::RtcIceServerTransportProtocol>(mapped->second);
		}

		// Per RFC 5245: ICE component IDs are 1 (RTP) and 2 (RTCP).
		inline Nullable<WebRtcNet::RtcIceComponent> marshal_ice_component(int component)
		{
			return Nullable<WebRtcNet::RtcIceComponent>(
				marshal_mapped_native_type(ice_component_map, component));
		}

		inline String^ marshal_socket_address_host(const webrtc::SocketAddress& addr)
		{
			const auto host = addr.HostAsURIString();
			return host.empty() ? String::Empty : marshal_as<String^>(host);
		}

		inline Nullable<System::UInt16> marshal_socket_address_port(const webrtc::SocketAddress& addr)
		{
			if (addr.HostAsURIString().empty()) return Nullable<System::UInt16>();
			return Nullable<System::UInt16>((System::UInt16)addr.port());
		}
	}

	template<>
	inline WebRtcNet::RtcIceCandidate^ marshal_as(const webrtc::IceCandidateInterface* const& from)
	{
		if (from == nullptr) throw gcnew ArgumentNullException("from");

		const auto& cand = from->candidate();

		std::string candidate_str;
		from->ToString(&candidate_str);

		const auto& addr = cand.address();
		const auto& related_addr = cand.related_address();

		return gcnew WebRtcNet::RtcIceCandidate(
			marshal_as<String^>(candidate_str),
			marshal_as<String^>(from->sdp_mid()),
			Nullable<System::UInt16>((System::UInt16)from->sdp_mline_index()),
			marshal_as<String^>(cand.username()),
			marshal_as<String^>(cand.foundation()),
			marshal_ice_component(cand.component()),
			Nullable<System::UInt32>((System::UInt32)cand.priority()),
			marshal_socket_address_host(addr),
			marshal_ice_protocol(cand.protocol()),
			marshal_socket_address_port(addr),
			marshal_ice_candidate_type(cand.type()),
			marshal_ice_tcp_candidate_type(cand.tcptype()),
			marshal_socket_address_host(related_addr),
			marshal_socket_address_port(related_addr),
			marshal_ice_server_transport_protocol(cand.relay_protocol()),
			marshal_as<String^>(cand.url())
		);
	}
}} 