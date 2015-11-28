#include "stdafx.h"

#include "DataChannelObserver.h"

using namespace System;
using namespace System::Runtime::InteropServices;

WebRtcObservers_Start

DataChannelObserver::DataChannelObserver(WebRtcInterop::RtcDataChannel ^ dataChannel, webrtc::DataChannelInterface * nativeDataChannel)
	: _dataChannel(dataChannel)
	, _nativeDataChannel(nativeDataChannel)
{
}

DataChannelObserver::~DataChannelObserver()
{
	_dataChannel = nullptr;
}

void DataChannelObserver::OnStateChange()
{
	auto state = _nativeDataChannel->state();
	if (state == webrtc::DataChannelInterface::DataState::kOpen)
	{
		_dataChannel->FireOnOpen();
	}
	else if (state == webrtc::DataChannelInterface::DataState::kClosed)
	{
		_dataChannel->FireOnClose();
	}
}

void DataChannelObserver::OnMessage(const webrtc::DataBuffer & buffer)
{
	if (buffer.binary)
	{
		auto data = buffer.data.data<unsigned char>();
		
		int size = buffer.data.size();
		auto managedData = gcnew array<unsigned char>(size);

		{
			pin_ptr<unsigned char> ptrBuffer = &managedData[managedData->GetLowerBound(0)];
			memccpy(ptrBuffer, data, 0, size);
		}

		_dataChannel->FireOnMessage(managedData, "N/A", "N/A");
	}
	else
	{
		auto data = buffer.data.data<char>();
		auto str = marshal_as<System::String ^>(data);
		_dataChannel->FireOnMessage(str, "N/A", "N/A");
	}
}

void DataChannelObserver::OnBufferedAmountChange(uint64_t previous_amount)
{
	//Ignore.
}

WebRtcObservers_End
