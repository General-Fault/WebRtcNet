#include "pch.h"

#include "RtcDataChannel.h"

#include <api/data_channel_interface.h>
#include "Marshaling/MarshalCollections.h"

#include "Marshaling/MarshalDataChannel.h"
#include "Marshaling/MarshalNullable.h"
#include "Observers/DataChannelObserver.h"
#include <limits>

using namespace System;
using namespace WebRtcNet;

namespace WebRtcInterop
{
	RtcDataChannel::RtcDataChannel(webrtc::DataChannelInterface* data_channel_interface)
		: rp_data_channel_interface_(data_channel_interface),
		  buffered_amount_low_threshold_(0),
		  on_open_(nullptr),
		  on_error_(nullptr),
		  on_closing_(nullptr),
		  on_close_(nullptr),
		  on_message_(nullptr),
		  on_buffered_amount_low_(nullptr)
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

	webrtc::scoped_refptr<webrtc::DataChannelInterface> RtcDataChannel::GetNativeDataChannelInterface(const bool throwOnDisposed)
	{
		const auto result = rp_data_channel_interface_.Get();
		if (result == nullptr)
		{
			if (throwOnDisposed) throw gcnew ObjectDisposedException(NAMEOF(RtcDataChannel));
			return webrtc::scoped_refptr<webrtc::DataChannelInterface>();
		}

		return webrtc::scoped_refptr<webrtc::DataChannelInterface>(result);
	}

	String^ RtcDataChannel::Label::get()
	{
		return marshal_as<String^>(GetNativeDataChannelInterface(true)->label());
	}

	bool RtcDataChannel::Ordered::get()
	{
		return GetNativeDataChannelInterface(true)->ordered();
	}

	Nullable<uint16_t> RtcDataChannel::MaxPacketLifeTime::get()
	{
		return marshal_as<uint16_t>(GetNativeDataChannelInterface(true)->maxPacketLifeTime());
	}

	Nullable<uint16_t> RtcDataChannel::MaxRetransmits::get()
	{
		return marshal_as<uint16_t>(GetNativeDataChannelInterface(true)->maxRetransmitsOpt());
	}

	String^ RtcDataChannel::Protocol::get()
	{
		return marshal_as<String^>(GetNativeDataChannelInterface(true)->protocol());
	}

	bool RtcDataChannel::Negotiated::get()
	{
		return GetNativeDataChannelInterface(true)->negotiated();
	}

	Nullable<uint16_t> RtcDataChannel::Id::get()
	{
		const auto id = GetNativeDataChannelInterface(true)->id();
		if (id < 0 || id > std::numeric_limits<uint16_t>::max())
		{
			return Nullable<uint16_t>();
		}

		return static_cast<uint16_t>(id);
	}

	RtcDataChannelState RtcDataChannel::ReadyState::get()
	{
		return marshal_as<RtcDataChannelState>(GetNativeDataChannelInterface(true)->state());
	}

	uint64_t RtcDataChannel::BufferedAmount::get()
	{
		return GetNativeDataChannelInterface(true)->buffered_amount();
	}

	uint64_t RtcDataChannel::BufferedAmountLowThreshold::get()
	{
		return buffered_amount_low_threshold_;
	}

	void RtcDataChannel::BufferedAmountLowThreshold::set(uint64_t value)
	{
		buffered_amount_low_threshold_ = value;
		if (buffered_amount_low_threshold_ < BufferedAmount)
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
		const webrtc::DataBuffer buffer(marshal_as<std::string>(data));
		native->Send(buffer);
	}

	void RtcDataChannel::Send(IEnumerable<Byte>^ data)
	{
		const auto native = GetNativeDataChannelInterface(true);

		const auto vector = marshal_as<std::vector, uint8_t>(data);

		const webrtc::CopyOnWriteBuffer buffer(vector);
		native->Send(webrtc::DataBuffer(buffer, true));
	}

	void RtcDataChannel::Send(array<Byte>^ data)
	{
		const auto native = GetNativeDataChannelInterface(true);

		const pin_ptr<Byte> ptr = &data[0];
		const Byte* np = ptr;
		const webrtc::CopyOnWriteBuffer buffer(np, data->Length);
		native->Send(webrtc::DataBuffer(buffer, true));
	}
}
