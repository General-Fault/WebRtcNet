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

	public ref class RtcDataChannel : public WebRtcNet::RtcDataChannel
	{
	public:
		~RtcDataChannel();

		// Inherited via RtcDataChannel
		virtual property String^ Label { String^ get() override; }
		virtual property bool Ordered { bool get() override; }
		virtual property Nullable<uint16_t> MaxPacketLifeTime { Nullable<uint16_t> get() override; }
		virtual property Nullable<uint16_t> MaxRetransmits { Nullable<uint16_t> get() override; }
		virtual property String^ Protocol { String^ get() override; }
		virtual property bool Negotiated { bool get() override; }
		virtual property Nullable<uint16_t> Id { Nullable<uint16_t> get() override; }
		virtual property RtcDataChannelState ReadyState { RtcDataChannelState get() override; }
		virtual property UInt64 BufferedAmount { uint64_t get() override; }
		virtual property uint64_t BufferedAmountLowThreshold
		{
			uint64_t get() override; void set(uint64_t value) override;
		}
		virtual property String^ BinaryType { String^ get() override; void set(String^ value) override; }

		virtual event EventHandler^ OnOpen
		{
			void add(EventHandler^ value) override { on_open_ += value; }
			void remove(EventHandler^ value) override { on_open_ -= value; }
		}
		virtual event EventHandler<RtcErrorEventArgs^>^ OnError
		{
			void add(EventHandler<RtcErrorEventArgs^>^ value) override { on_error_ += value; }
			void remove(EventHandler<RtcErrorEventArgs^>^ value) override { on_error_ -= value; }
		}
		virtual event EventHandler^ OnClosing
		{
			void add(EventHandler^ value) override { on_closing_ += value; }
			void remove(EventHandler^ value) override { on_closing_ -= value; }
		}
		virtual event EventHandler^ OnClose
		{
			void add(EventHandler^ value) override { on_close_ += value; }
			void remove(EventHandler^ value) override { on_close_ -= value; }
		}
		virtual event EventHandler<MessageEventArgs^>^ OnMessage
		{
			void add(EventHandler<MessageEventArgs^>^ value) override { on_message_ += value; }
			void remove(EventHandler<MessageEventArgs^>^ value) override { on_message_ -= value; }
		}
		virtual event EventHandler^ OnBufferedAmountLow
		{
			void add(EventHandler^ value) override { on_buffered_amount_low_ += value; }
			void remove(EventHandler^ value) override { on_buffered_amount_low_ -= value; }
		}

		void Close() override;
		void Send(String^ data) override;
		void Send(Collections::Generic::IEnumerable<Byte>^ data) override;
		void Send(array<Byte>^ data) override;
	internal:
		RtcDataChannel(webrtc::DataChannelInterface* data_channel_interface);
		!RtcDataChannel();
		webrtc::DataChannelInterface* GetNativeDataChannelInterface(bool throwOnDisposed);

	protected:
		virtual IntPtr GetNativeDataChannelHandle(bool throwOnDisposed) override;

	internal:
		//Event invocation 
		void FireOnOpen() { if (on_open_ != nullptr) on_open_(this, EventArgs::Empty); }
		void FireOnError(RtcError^ error) { if (on_error_ != nullptr) on_error_(this, gcnew RtcErrorEventArgs(error)); }
		void FireOnClosing() { if (on_closing_ != nullptr) on_closing_(this, EventArgs::Empty); }
		void FireOnClose() { if (on_close_ != nullptr) on_close_(this, EventArgs::Empty); }

		void FireOnMessage(Object^ data, String^ origin, String^ lastEventId)
		{
			if (on_message_ != nullptr) on_message_(this, gcnew MessageEventArgs(data, origin, lastEventId));
		}

		void FireOnBufferAmountLow() { if (on_buffered_amount_low_ != nullptr) on_buffered_amount_low_(this, EventArgs::Empty); }

	private:
		ManagedScopedRefPtr<webrtc::DataChannelInterface> rp_data_channel_interface_;
		uint64_t buffered_amount_low_threshold_;
		EventHandler^ on_open_;
		EventHandler<RtcErrorEventArgs^>^ on_error_;
		EventHandler^ on_closing_;
		EventHandler^ on_close_;
		EventHandler<MessageEventArgs^>^ on_message_;
		EventHandler^ on_buffered_amount_low_;
	};
}
