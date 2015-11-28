#pragma once

namespace webrtc
{
	class DataChannelInterface;
}
namespace rtc
{
	template <class T> class scoped_refptr;
}

namespace WebRtcInterop {

	public ref class RtcDataChannel : WebRtcNet::IRtcDataChannel
	{
	public:
		~RtcDataChannel();

		// Inherited via IRtcDataChannel
		virtual property System::String ^ Label { System::String ^ get(); }
		virtual property bool Ordered { bool get(); }
		virtual property System::Nullable<unsigned int> MaxPacketLifeTime { System::Nullable<unsigned int> get();  }
		virtual property System::Nullable<unsigned int> MaxRetransmits { System::Nullable<unsigned int> get();  }
		virtual property System::String ^ Protocol { System::String ^ get(); }
		virtual property bool Negotiated { bool get(); }
		virtual property unsigned int Id { unsigned int get(); }
		virtual property WebRtcNet::RtcDataChannelState ReadyState { WebRtcNet::RtcDataChannelState get(); }
		virtual property unsigned int BufferedAmount { unsigned int get(); }
		virtual property System::String ^ BinaryType { System::String ^ get(); void set(System::String ^ value); }
		virtual event System::EventHandler ^ OnOpen;
		virtual event System::EventHandler ^ OnError;
		virtual event System::EventHandler ^ OnClose;
		virtual event System::EventHandler<WebRtcNet::MessageEventArgs ^> ^ OnMessage;
		virtual void Close();
		virtual void Send(System::String ^data);
		virtual void Send(array<unsigned char, 1> ^ data);
	internal:
		RtcDataChannel(webrtc::DataChannelInterface* dataChannelInterface);
		!RtcDataChannel();
		webrtc::DataChannelInterface* GetNativeDataChannelInterface(bool throwOnDisposed);

		//Event invocation 
		void FireOnOpen() { OnOpen(this, System::EventArgs::Empty); }
		void FireOnError() { OnError(this, System::EventArgs::Empty); }
		void FireOnClose() { OnClose(this, System::EventArgs::Empty); }
		void FireOnMessage(System::Object ^ data, System::String ^ origin, System::String ^ lastEventId) 
		{ OnMessage(this, gcnew WebRtcNet::MessageEventArgs(data, origin, lastEventId)); }

	private:
		rtc::scoped_refptr<webrtc::DataChannelInterface> * _rpDataChannelInterface;
	};

}