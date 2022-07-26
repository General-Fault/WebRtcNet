#pragma once

#include <msclr/auto_gcroot.h>

#include "../RtcDataChannel.h"
#include "api/data_channel_interface.h"

using namespace msclr;
using namespace webrtc;
using namespace rtc;

namespace WebRtcInterop::Observers
{
	private class DataChannelObserver : public webrtc::DataChannelObserver
	{
	public:
		// The data channel state have changed.
		void OnStateChange() override;

		//  A data buffer was successfully received.
		void OnMessage(const DataBuffer& buffer) override;

		// The data channel's buffered_amount has changed.
		void OnBufferedAmountChange(uint64_t previous_amount) override;

		DataChannelObserver(RtcDataChannel^ data_channel, DataChannelInterface* native_data_channel);
		~DataChannelObserver() override;

	private:
		gcroot<RtcDataChannel^> data_channel_;
		scoped_refptr<DataChannelInterface> native_data_channel_;
	};

}