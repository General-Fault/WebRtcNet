#pragma once

namespace webrtc
{
	template <class T>
	class scoped_refptr;
	class Thread;
	class PeerConnectionFactoryInterface;
}

namespace WebRtcInterop
{
	public ref class RtcPeerConnectionFactory
	{
	public:
		static WebRtcNet::RtcPeerConnection^ CreatePeerConnection(WebRtcNet::RtcConfiguration^ configuration);

		static property RtcPeerConnectionFactory^ Instance
		{
			RtcPeerConnectionFactory^ get();
		}

	internal:
		RtcPeerConnectionFactory();
		~RtcPeerConnectionFactory();
		!RtcPeerConnectionFactory();

		static void InitializeInstance();
		static void DestroyInstance();

		webrtc::PeerConnectionFactoryInterface* GetNativePeerConnectionFactoryInterface(bool throwOnDisposed);

	private:
		void CreateNativePeerConnectionFactory();
		void DestroyNativePeerConnectionFactory();

		webrtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>* _rpPeerConnectionFactory;

		static RtcPeerConnectionFactory^ _instance = nullptr;
	};
}
