#include "stdafx.h"

#include "talk\app\webrtc\peerconnectionfactory.h"
#include "talk\app\webrtc\peerconnection.h"
#include "talk\app\webrtc\mediaconstraintsinterface.h"

#include "webrtc\base\win32socketserver.h"
#include "webrtc\base\win32socketinit.h"
#include "webrtc\base\ssladapter.h"


using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

using namespace WebRtcNet;

#include "RtcPeerConnection.h"
#include "Observers\PeerConnectionObserver.h"
#include "Observers\CreateSessionDescriptionObserver.h"
#include "Marshaling\MarshalPeerConnection.h"
#include "Marshaling\MarshalRtcConfiguration.h"

namespace WebRtcInterop {

RtcPeerConnection::RtcPeerConnection(RtcConfiguration ^ configuration)
	: _observer(new webrtc_observers::PeerConnectionObserver(this))
{
	if (_signalThread == nullptr)
	{
		_signalThread = new rtc::Win32Thread();
		_signalThread->SetName("WebRtc Signal Thread", NULL);
	}

	rtc::ThreadManager::Instance()->SetCurrentThread(_signalThread);

	rtc::EnsureWinsockInit();
	rtc::InitializeSSL();

	auto nativePeerConnectionFactory = webrtc::CreatePeerConnectionFactory();
	_rpPeerConnectionFactory = new rtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>(nativePeerConnectionFactory);
		
	auto nativeConfig = marshal_as<webrtc::PeerConnectionInterface::RTCConfiguration>(configuration);
	_rpPeerConnection = new rtc::scoped_refptr<webrtc::PeerConnectionInterface>(
		nativePeerConnectionFactory->CreatePeerConnection(nativeConfig, nullptr, nullptr, 
			rtc::scoped_ptr<webrtc::DtlsIdentityStoreInterface>(nullptr), _observer));
}

RtcPeerConnection::~RtcPeerConnection()
{
	this->!RtcPeerConnection();
}

RtcPeerConnection::!RtcPeerConnection()
{
	delete _rpPeerConnection;
	_rpPeerConnection = nullptr;

	delete _rpPeerConnectionFactory;
	_rpPeerConnectionFactory = nullptr;

	delete _observer;
	_observer = nullptr;

	rtc::ThreadManager::Instance()->SetCurrentThread(nullptr);
	rtc::CleanupSSL();
}

webrtc::PeerConnectionFactoryInterface * RtcPeerConnection::GetNativePeerConnectionFactory(bool throwOnDisposed)
{
	if (_rpPeerConnectionFactory == nullptr || _rpPeerConnectionFactory->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("RtcPeerConnection");
		return nullptr;
	}

	return _rpPeerConnectionFactory->get();
}

webrtc::PeerConnectionInterface * RtcPeerConnection::GetNativePeerConnection(bool throwOnDisposed)
{
	if (_rpPeerConnection == nullptr || _rpPeerConnection->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("RtcPeerConnection");
		return nullptr;
	}

	return _rpPeerConnection->get();
}

Task<RtcSessionDescription> ^ RtcPeerConnection::CreateOffer(RtcOfferOptions ^options)
{
	auto nativeOptions = marshal_as<webrtc::PeerConnection::RTCOfferAnswerOptions>(options);
	auto tcs = gcnew TaskCompletionSource<RtcSessionDescription>();
	auto pc = GetNativePeerConnection(true);
	rtc::scoped_refptr<webrtc::CreateSessionDescriptionObserver> observer(
		dynamic_cast<webrtc::CreateSessionDescriptionObserver*>(
			new rtc::RefCountedObject<webrtc_observers::CreateSessionDescriptionObserver>(tcs)));
	
	pc->CreateOffer(observer, nativeOptions);

	return tcs->Task;
}

Task<RtcSessionDescription> ^ RtcPeerConnection::CreateAnswer()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Task ^ RtcPeerConnection::SetLocalDescription(RtcSessionDescription description)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Task ^ RtcPeerConnection::SetRemoteDescription(RtcSessionDescription description)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void RtcPeerConnection::UpdateIce(RtcConfiguration ^configuration)
{
	throw gcnew NotImplementedException();
}

Task ^ RtcPeerConnection::AddIceCandidate(RtcIceCandidate candidate)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

RtcConfiguration ^ RtcPeerConnection::GetConfiguration()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void RtcPeerConnection::SetConfiguration(RtcConfiguration ^configuration)
{
	throw gcnew NotImplementedException();
}

Collections::Generic::IEnumerable<IMediaStream ^> ^ RtcPeerConnection::GetLocalStreams()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Collections::Generic::IEnumerable<IMediaStream ^> ^ RtcPeerConnection::GetRemoteStreams()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IMediaStream ^ RtcPeerConnection::GetStreamById(String ^streamId)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void RtcPeerConnection::AddStream(IMediaStream ^stream)
{
	throw gcnew NotImplementedException();
}

void RtcPeerConnection::RemoveStream(IMediaStream ^stream)
{
	throw gcnew NotImplementedException();
}

void RtcPeerConnection::Close()
{
	throw gcnew NotImplementedException();
}

IRtcDataChannel ^ RtcPeerConnection::CreateDataChannel(String ^label, RtcDataChannelInit ^dataChannelInit)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IRtcDtmfSender ^ RtcPeerConnection::CreateRtcDtmfSender(IMediaStreamTrack ^track)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Task<IRtcStatsReport ^> ^ RtcPeerConnection::GetStats(IMediaStreamTrack ^selector)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void RtcPeerConnection::SetIdentityProvider(String ^provider, String ^protocol, String ^username)
{
	throw gcnew NotImplementedException();
}

void RtcPeerConnection::GetIdentityAssertion()
{
	throw gcnew NotImplementedException();
}


}