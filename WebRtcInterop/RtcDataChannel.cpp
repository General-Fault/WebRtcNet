#include "pch.h"

#include "RtcDataChannel.h"

#include "api/data_channel_interface.h"
#include "Marshaling/MarshalCollections.h"

#include "Marshaling/MarshalDataChannel.h"
#include "Marshaling/MarshalNullable.h"
#include "Observers/DataChannelObserver.h"

using namespace System;
using namespace WebRtcNet;

namespace WebRtcInterop
{
	RtcDataChannel::RtcDataChannel(DataChannelInterface* data_channel_interface)
		: rp_data_channel_interface_(data_channel_interface),
		  buffered_amount_low_threshold_()
	{
		if (data_channel_interface == nullptr) throw gcnew ArgumentNullException(NAMEOF(data_channel_interface));

		rp_data_channel_interface_->RegisterObserver(new Observers::DataChannelObserver(this, data_channel_interface));
	}

	RtcDataChannel::~RtcDataChannel()
	{
		this->!RtcDataChannel();
	}

	RtcDataChannel::!RtcDataChannel()
	{
		rp_data_channel_interface_ = nullptr;
	}

	DataChannelInterface* RtcDataChannel::GetNativeDataChannelInterface(const bool throwOnDisposed)
	{
		const auto result = rp_data_channel_interface_.Get();
		if (result == nullptr)
		{
			if (throwOnDisposed) throw gcnew ObjectDisposedException(NAMEOF(RtcDataChannel));
			return nullptr;
		}

		return result;
	}

	String^ RtcDataChannel::Label::get()
	{
		return marshal_as<String^>(GetNativeDataChannelInterface(true)->label());
	}

	bool RtcDataChannel::Ordered::get()
	{
		return GetNativeDataChannelInterface(true)->ordered();
	}

	Nullable<uint32_t> RtcDataChannel::MaxPacketLifeTime::get()
	{
		return marshal_as<uint32_t>(GetNativeDataChannelInterface(true)->maxPacketLifeTime());
	}

	Nullable<uint32_t> RtcDataChannel::MaxRetransmits::get()
	{
		return GetNativeDataChannelInterface(true)->maxRetransmits();
	}

	String^ RtcDataChannel::Protocol::get()
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

	uint64_t RtcDataChannel::BufferedAmount::get()
	{
		return static_cast<int>(GetNativeDataChannelInterface(true)->buffered_amount());
	}

	Nullable<uint64_t> RtcDataChannel::BufferedAmountLowThreshold::get()
	{
		return buffered_amount_low_threshold_;
	}

	void RtcDataChannel::BufferedAmountLowThreshold::set(Nullable<uint64_t> value)
	{
		buffered_amount_low_threshold_ = value;
		if (buffered_amount_low_threshold_.HasValue && buffered_amount_low_threshold_.Value < BufferedAmount)
		{
			FireOnBufferAmountLow();
		}
	}

	String^ RtcDataChannel::BinaryType::get()
	{
		return "blob";
	}

	void RtcDataChannel::BinaryType::set(String^ value)
	{
		throw gcnew NotImplementedException();
	}

	void RtcDataChannel::Close()
	{
		auto native = GetNativeDataChannelInterface(false);
		if (native == nullptr) return;

		native->Close();
	}

	void RtcDataChannel::Send(String^ data)
	{
		const auto native = GetNativeDataChannelInterface(true);
		const DataBuffer buffer(marshal_as<std::string>(data));
		native->Send(buffer);
	}

	void RtcDataChannel::Send(IEnumerable<Byte>^ data)
	{
		const auto native = GetNativeDataChannelInterface(true);

		const auto vector = marshal_as<std::vector, uint8_t>(data);

		const CopyOnWriteBuffer buffer(vector);
		native->Send(DataBuffer(buffer, true));
	}

	void RtcDataChannel::Send(array<Byte>^ data)
	{
		const auto native = GetNativeDataChannelInterface(true);

		const pin_ptr<Byte> ptr = &data[0];
		const Byte* np = ptr;
		const CopyOnWriteBuffer buffer(np, data->Length);
		native->Send(DataBuffer(buffer, true));
	}
}
