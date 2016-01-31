#pragma once

#include "talk\app\webrtc\datachannelinterface.h"

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

namespace msclr {
	namespace interop
	{
		template<>
		inline WebRtcNet::RtcDataChannelState marshal_as(webrtc::DataChannelInterface::DataState const & from)
		{
			switch (from)
			{
			case webrtc::DataChannelInterface::DataState::kConnecting:
				return WebRtcNet::RtcDataChannelState::Connecting;
			case webrtc::DataChannelInterface::DataState::kOpen:
				return WebRtcNet::RtcDataChannelState::Open;
			case webrtc::DataChannelInterface::DataState::kClosing:
				return WebRtcNet::RtcDataChannelState::Closing;
			case webrtc::DataChannelInterface::DataState::kClosed:
				return WebRtcNet::RtcDataChannelState::Closed;
			}
			throw gcnew System::InvalidCastException("Invalid DataChannelInterface::DataState value.");
		}
	}
}
