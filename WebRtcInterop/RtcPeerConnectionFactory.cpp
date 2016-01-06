#include "stdafx.h"
#include "RtcPeerConnectionFactory.h"

#include "webrtc\base\win32socketserver.h"
#include "webrtc\base\win32socketinit.h"
#include "webrtc\base\ssladapter.h"
#include "webrtc\base\bind.h"

#include "talk\app\webrtc\peerconnectionfactory.h"

using namespace System;
using namespace System::Threading;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

RtcPeerConnectionFactory::RtcPeerConnectionFactory()
{
	if (_signalThread == nullptr)
	{
		_signalThread = new rtc::Thread();
		_signalThread->SetName("WebRtc Signal Thread", NULL);
		_signalThread->Start();
	}

	auto action = gcnew Action(this, &RtcPeerConnectionFactory::CreateNativePeerConnectionFactory);
	_signalThread->Invoke<void>(rtc::Bind((void (*)())Marshal::GetFunctionPointerForDelegate(action).ToPointer()));
}


void RtcPeerConnectionFactory::CreateNativePeerConnectionFactory()
{
	rtc::EnsureWinsockInit();
	rtc::InitializeSSL();

	auto nativePeerConnectionFactory = webrtc::CreatePeerConnectionFactory();
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