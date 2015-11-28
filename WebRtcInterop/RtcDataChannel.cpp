#include "stdafx.h"
#include "RtcDataChannel.h"

#include "webrtc\base\scoped_ref_ptr.h"
#include "talk\app\webrtc\datachannelinterface.h"

#include "Marshaling\MarshalDataChannel.h"
#include "Observers\DataChannelObserver.h"

using namespace System;
using namespace WebRtcNet;

namespace WebRtcInterop {

RtcDataChannel::RtcDataChannel(webrtc::DataChannelInterface* dataChannelInterface)
	:_rpDataChannelInterface(new rtc::scoped_refptr<webrtc::DataChannelInterface>(dataChannelInterface))
{
	dataChannelInterface->RegisterObserver(new WebRtcInterop::Observers::DataChannelObserver(this, dataChannelInterface));
}

RtcDataChannel::~RtcDataChannel()
{
	this->!RtcDataChannel();
}

RtcDataChannel::!RtcDataChannel()
{
	delete _rpDataChannelInterface;
	_rpDataChannelInterface = nullptr;
}

webrtc::DataChannelInterface* RtcDataChannel::GetNativeDataChannelInterface(bool throwOnDisposed)
{
	if (_rpDataChannelInterface == nullptr || _rpDataChannelInterface->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("RtcDataChannel");
		return nullptr;
	}

	return _rpDataChannelInterface->get();
}

String ^ RtcDataChannel::Label::get()
{
	return marshal_as<String ^>(GetNativeDataChannelInterface(true)->label());
}

bool RtcDataChannel::Ordered::get()
{
	return GetNativeDataChannelInterface(true)->ordered();
}

Nullable<unsigned int> RtcDataChannel::MaxPacketLifeTime::get()
{ 
	return GetNativeDataChannelInterface(true)->maxRetransmitTime();
}

Nullable<unsigned int> RtcDataChannel::MaxRetransmits::get()
{
	return GetNativeDataChannelInterface(true)->maxRetransmits();
}

String ^ RtcDataChannel::Protocol::get()
{
	return marshal_as<String^>(GetNativeDataChannelInterface(true)->protocol());
}

bool RtcDataChannel::Negotiated::get()
{
	return GetNativeDataChannelInterface(true)->negotiated();
}

unsigned int RtcDataChannel::Id::get()
{
	return GetNativeDataChannelInterface(true)->id();
}

RtcDataChannelState RtcDataChannel::ReadyState::get()
{
	return marshal_as<RtcDataChannelState>(GetNativeDataChannelInterface(true)->state());
}

unsigned int RtcDataChannel::BufferedAmount::get()
{
	return static_cast<int>(GetNativeDataChannelInterface(true)->buffered_amount());
}

String ^ RtcDataChannel::BinaryType::get()
{
	return "blob";
}

void RtcDataChannel::BinaryType::set(String ^ value)
{
	throw gcnew NotImplementedException();
}

void RtcDataChannel::Close()
{
	auto native = GetNativeDataChannelInterface(false);
	if (native == nullptr) return;

	native->Close();
}

void RtcDataChannel::Send(String ^data)
{
	auto native = GetNativeDataChannelInterface(true);
	webrtc::DataBuffer buffer(marshal_as<std::string>(data));
	native->Send(buffer);
}

void RtcDataChannel::Send(array<unsigned char, 1> ^data)
{
	auto native = GetNativeDataChannelInterface(true);

	pin_ptr<unsigned char> dataPtr = &data[data->GetLowerBound(0)];

	rtc::Buffer buffer((const unsigned char *)dataPtr, data->Length);
	native->Send(webrtc::DataBuffer(buffer, true));
}

}