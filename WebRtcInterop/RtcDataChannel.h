#pragma once

#include "ManagedScopedRefPtr.h"

namespace webrtc
{
	class DataChannelInterface;
}

namespace WebRtcInterop
{
	using namespace System;
	using namespace WebRtcNet;

	public ref class RtcDataChannel : IRtcDataChannel
	{
	public:
		~RtcDataChannel();

		// Inherited via IRtcDataChannel
		virtual property String^ Label { String^ get(); }
		virtual property bool Ordered { bool get(); }
		virtual property Nullable<uint32_t> MaxPacketLifeTime { Nullable<uint32_t> get(); }
		virtual property Nullable<uint32_t> MaxRetransmits { Nullable<uint32_t> get(); }
		virtual property String^ Protocol { String^ get(); }
		virtual property bool Negotiated { bool get(); }
		virtual property UInt32 Id { UInt32 get(); }
		virtual property RtcDataChannelState ReadyState { RtcDataChannelState get(); }
		virtual property UInt64 BufferedAmount { uint64_t get(); }
		virtual property Nullable<uint64_t> BufferedAmountLowThreshold
		{
			Nullable<uint64_t> get(); void set(Nullable<uint64_t> value);
		}
		virtual property String^ BinaryType { String^ get(); void set(String^ value); }

		virtual event EventHandler^ OnOpen;
		virtual event EventHandler<RtcErrorEventArgs^>^ OnError;
		virtual event EventHandler^ OnClose;
		virtual event EventHandler<MessageEventArgs^>^ OnMessage;
		virtual event EventHandler^ OnBufferedAmountLow;

		virtual void Close();
		virtual void Send(String^ data);
		virtual void Send(Collections::Generic::IEnumerable<Byte>^ data);
		virtual void Send(array<Byte>^ data);
	internal:
		RtcDataChannel(webrtc::DataChannelInterface* data_channel_interface);
		!RtcDataChannel();
		webrtc::DataChannelInterface* GetNativeDataChannelInterface(bool throwOnDisposed);

		//Event invocation 
		void FireOnOpen() { OnOpen(this, EventArgs::Empty); }
		void FireOnError(RtcError^ error) { OnError(this, gcnew RtcErrorEventArgs(error)); }
		void FireOnClose() { OnClose(this, EventArgs::Empty); }

		void FireOnMessage(Object^ data, String^ origin, String^ lastEventId)
		{
			OnMessage(this, gcnew MessageEventArgs(data, origin, lastEventId));
		}

		void FireOnBufferAmountLow() { OnBufferedAmountLow(this, EventArgs::Empty); }

	private:
		ManagedScopedRefPtr<webrtc::DataChannelInterface> rp_data_channel_interface_;
		Nullable<uint64_t> buffered_amount_low_threshold_;
	};
}
