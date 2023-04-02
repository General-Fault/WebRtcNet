#pragma once

#include "api/data_channel_interface.h"

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

#include <map>

#include "MarshalEnums.h"

namespace msclr::interop
{
	using namespace System;

	static const std::map<const webrtc::DataChannelInterface::DataState, const WebRtcNet::RtcDataChannelState> data_channel_state_map {
		{webrtc::DataChannelInterface::DataState::kConnecting, WebRtcNet::RtcDataChannelState::Connecting},
		{webrtc::DataChannelInterface::DataState::kOpen, WebRtcNet::RtcDataChannelState::Open},
		{webrtc::DataChannelInterface::DataState::kClosing, WebRtcNet::RtcDataChannelState::Closing},
		{webrtc::DataChannelInterface::DataState::kClosed, WebRtcNet::RtcDataChannelState::Closed},
	};

	template<>
	inline WebRtcNet::RtcDataChannelState marshal_as(const webrtc::DataChannelInterface::DataState& from)
	{
		return marshal_mapped_native_type(data_channel_state_map, from);
	}

	template<>
	inline webrtc::DataChannelInterface::DataState marshal_as(const WebRtcNet::RtcDataChannelState& from)
	{
		return marshal_mapped_managed_type(data_channel_state_map, from);
	}
}
