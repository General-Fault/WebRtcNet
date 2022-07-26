#include "pch.h"

#include "DataChannelObserver.h"

using namespace WebRtcInterop;
using namespace System;
using namespace Runtime::InteropServices;

namespace WebRtcInterop::Observers
{
	DataChannelObserver::DataChannelObserver(RtcDataChannel^ data_channel,
		DataChannelInterface* native_data_channel)
		: data_channel_(data_channel)
		, native_data_channel_(native_data_channel)
	{
		if (data_channel == nullptr) throw gcnew ArgumentNullException("dataChannel");
		if (native_data_channel == nullptr) throw gcnew ArgumentNullException("nativeDataChannel");
	}

	DataChannelObserver::~DataChannelObserver()
	{
		data_channel_ = nullptr;
	}

	void DataChannelObserver::OnStateChange()
	{
		const auto state = native_data_channel_->state();
		if (state == DataChannelInterface::DataState::kOpen)
		{
			data_channel_->FireOnOpen();
		}
		else if (state == DataChannelInterface::DataState::kClosed)
		{
			data_channel_->FireOnClose();
		}
	}

	void DataChannelObserver::OnMessage(const DataBuffer& buffer)
	{
		if (buffer.binary)
		{
			const auto data = buffer.data.data<unsigned char>();

			const auto size = buffer.data.size();
			auto managedData = gcnew array<unsigned char>(static_cast<int>(size));

			{
				const pin_ptr<unsigned char> ptrBuffer = &managedData[managedData->GetLowerBound(0)];
				_memccpy(ptrBuffer, data, 0, size);
			}

			data_channel_->FireOnMessage(managedData, "N/A", "N/A");
		}
		else
		{
			const auto data = buffer.data.data<char>();
			auto str = marshal_as<String^>(data);
			data_channel_->FireOnMessage(str, "N/A", "N/A");
		}
	}

	void DataChannelObserver::OnBufferedAmountChange(uint64_t previous_amount)
	{
		if (!data_channel_->BufferedAmountLowThreshold.HasValue) return;

		const auto threshold = data_channel_->BufferedAmountLowThreshold.Value;
		if (previous_amount > threshold && native_data_channel_->buffered_amount() < threshold)
		{
			data_channel_->FireOnBufferAmountLow();
		}
	}

}