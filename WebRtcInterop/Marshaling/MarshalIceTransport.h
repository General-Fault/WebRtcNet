#pragma once

#include "p2p/base/transport_description.h"
#include "api/transport/enums.h"

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <map>

#include "MarshalEnums.h"

namespace msclr::interop
{
	using namespace System;

	static const std::map<const cricket::IceRole, const WebRtcNet::RtcIceRole> ice_role_map {
		{cricket::IceRole::ICEROLE_CONTROLLED, WebRtcNet::RtcIceRole::Controlled},
		{cricket::IceRole::ICEROLE_CONTROLLING, WebRtcNet::RtcIceRole::Controlling},
		{cricket::IceRole::ICEROLE_UNKNOWN, WebRtcNet::RtcIceRole::Unknown},
	};

	template<>
	inline WebRtcNet::RtcIceRole marshal_as(const cricket::IceRole& from)
	{
		return marshal_mapped_native_type(ice_role_map, from);
	}

	template<>
	inline cricket::IceRole marshal_as<cricket::IceRole>(const WebRtcNet::RtcIceRole& from)
	{
		return marshal_mapped_managed_type(ice_role_map, from);
	}

	static const std::map<const int, const WebRtcNet::RtcIceComponent> ice_component_map {
		{0, WebRtcNet::RtcIceComponent::Rtp},
		{1, WebRtcNet::RtcIceComponent::Rtcp}
	};

	template<>
	inline WebRtcNet::RtcIceComponent marshal_as(const int& from)
	{
		return marshal_mapped_native_type(ice_component_map,from);
	}

	static const std::map<const webrtc::IceTransportState, const WebRtcNet::RtcIceTransportState> ice_transport_state_map{
		{ webrtc::IceTransportState::kNew, WebRtcNet::RtcIceTransportState::New },
		{ webrtc::IceTransportState::kChecking, WebRtcNet::RtcIceTransportState::Checking },
		{ webrtc::IceTransportState::kConnected, WebRtcNet::RtcIceTransportState::Connected },
		{ webrtc::IceTransportState::kCompleted, WebRtcNet::RtcIceTransportState::Completed },
		{ webrtc::IceTransportState::kFailed, WebRtcNet::RtcIceTransportState::Failed },
		{ webrtc::IceTransportState::kDisconnected, WebRtcNet::RtcIceTransportState::Disconnected },
		{ webrtc::IceTransportState::kClosed, WebRtcNet::RtcIceTransportState::Closed },
	};

	template<>
	inline WebRtcNet::RtcIceTransportState marshal_as(const webrtc::IceTransportState& from)
	{
		return marshal_mapped_native_type(ice_transport_state_map, from);
	}
}


