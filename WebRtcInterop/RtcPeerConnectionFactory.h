#pragma once

namespace rtc
{
	template <class T> class scoped_refptr;
	class Thread;
}

namespace webrtc
{
	class PeerConnectionFactoryInterface;
}


private ref class RtcPeerConnectionFactory
{
public:
	/// <summary>
	/// Get the singleton RtcPeerConnectionFactory. Calls InitializeInstance if necessary.
	/// </summary>
	static property RtcPeerConnectionFactory ^ Instance{ RtcPeerConnectionFactory ^  get(); }

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

	rtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>* _rpPeerConnectionFactory;

	static RtcPeerConnectionFactory ^ _instance;

	static std::unique_ptr<rtc::Thread> _signalThread;

	rtc::WinsockInitializer _winsock_init;
	rtc::PhysicalSocketServer _ss;
	rtc::AutoSocketServerThread _main_thread;
};

/// <summary>
/// Thrown when unable to create the native RtcPeerConnetionFactory.
/// </summary>
public ref class CreatePeerConnectionFactoryException : System::Exception{};
