#include "stdafx.h"
#include "RtcPeerConnectionFactory.h"

#include "rtc_base/win32_socket_init.h"
#include "rtc_base/openssl_adapter.h"
#include "rtc_base/thread.h"

#include "api/audio_codecs/builtin_audio_decoder_factory.h"
#include "api/audio_codecs/builtin_audio_encoder_factory.h"
#include "api/video_codecs/builtin_video_decoder_factory.h"
#include "api/video_codecs/builtin_video_encoder_factory.h"
#include "api/create_peerconnection_factory.h"

using namespace System;
using namespace System::Threading;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

RtcPeerConnectionFactory::RtcPeerConnectionFactory()
	: _main_thread(&_ss)
{
	if (_signalThread == nullptr)
	{
		_signalThread = rtc::Thread::CreateWithSocketServer();
		_signalThread->SetName("WebRtc Signal Thread", NULL);
		_signalThread->Start();
	}

	auto action = gcnew Action(this, &RtcPeerConnectionFactory::CreateNativePeerConnectionFactory);
	_signalThread->Invoke<void>(rtc::Bind((void (*)())Marshal::GetFunctionPointerForDelegate(action).ToPointer()));
}


void RtcPeerConnectionFactory::CreateNativePeerConnectionFactory()
{
	rtc::InitializeSSL();

	auto nativePeerConnectionFactory = webrtc::CreatePeerConnectionFactory(
		nullptr /* network_thread */, 
		nullptr /* worker_thread */,
		_signalThread.get(), 
		nullptr /* default_adm */,
		webrtc::CreateBuiltinAudioEncoderFactory(),
		webrtc::CreateBuiltinAudioDecoderFactory(),
		webrtc::CreateBuiltinVideoEncoderFactory(),
		webrtc::CreateBuiltinVideoDecoderFactory(), 
		nullptr /* audio_mixer */,
		nullptr /* audio_processing */);

	if (nativePeerConnectionFactory == nullptr) throw gcnew System::NotSupportedException("Failed to create native PeerConnectionFactory");
	_rpPeerConnectionFactory = new rtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>(nativePeerConnectionFactory);
}

void RtcPeerConnectionFactory::DestroyNativePeerConnectionFactory()
{
	delete _rpPeerConnectionFactory;
	_rpPeerConnectionFactory = nullptr;

	rtc::CleanupSSL();
}


RtcPeerConnectionFactory::~RtcPeerConnectionFactory()
{
	this->!RtcPeerConnectionFactory();
}

RtcPeerConnectionFactory::!RtcPeerConnectionFactory()
{
	auto action = gcnew Action(this, &RtcPeerConnectionFactory::DestroyNativePeerConnectionFactory);
	_signalThread->Invoke<void>(rtc::Bind((void(*)())Marshal::GetFunctionPointerForDelegate(action).ToPointer()));
}

webrtc::PeerConnectionFactoryInterface* RtcPeerConnectionFactory::GetNativePeerConnectionFactoryInterface(bool throwOnDisposed)
{
	if (_rpPeerConnectionFactory == nullptr || _rpPeerConnectionFactory->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("RtcPeerConnection");
		return nullptr;
	}

	return _rpPeerConnectionFactory->get();
}

RtcPeerConnectionFactory ^ RtcPeerConnectionFactory::Instance::get()
{
	if (_instance == nullptr)
	{
		InitializeInstance();
	}

	return _instance;
}

void RtcPeerConnectionFactory::InitializeInstance()
{
	if (_instance != nullptr) return;
	_instance = gcnew RtcPeerConnectionFactory();
}

void RtcPeerConnectionFactory::DestroyInstance()
{
	delete _instance;
	_instance = nullptr;
	delete _signalThread;
	_signalThread = nullptr;
}