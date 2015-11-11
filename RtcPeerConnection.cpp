#include "stdafx.h"

#include "talk\app\webrtc\peerconnectionfactory.h"
#include "talk\app\webrtc\peerconnection.h"

#include "webrtc\base\win32socketserver.h"
#include "webrtc\base\win32socketinit.h"
#include "webrtc\base\ssladapter.h"


using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

#include "RtcPeerConnection.h"

WEBRTCNET_START

static RtcPeerConnection::RtcPeerConnection()
{
}

RtcPeerConnection::RtcPeerConnection(RtcConfiguration ^ configuration)
{
	if (_signalThread == nullptr)
	{
		_signalThread = new rtc::Win32Thread();
		_signalThread->SetName("WebRtc Signal Thread", NULL);
	}

	rtc::ThreadManager::Instance()->SetCurrentThread(_signalThread);

	rtc::EnsureWinsockInit();
	rtc::InitializeSSL();

	_rpPeerConnectionFactory = new rtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>(
		webrtc::CreatePeerConnectionFactory());
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

	rtc::ThreadManager::Instance()->SetCurrentThread(nullptr);
	rtc::CleanupSSL();
}

webrtc::PeerConnectionFactoryInterface * RtcPeerConnection::GetNativePeerConnectionFactory()
{
	return _rpPeerConnectionFactory == nullptr ? nullptr : _rpPeerConnectionFactory->get();
}

webrtc::PeerConnectionInterface * RtcPeerConnection::GetNativePeerConnection()
{
	return _rpPeerConnection == nullptr ? nullptr : _rpPeerConnection->get();
}


Task<RtcSessionDescription^>^ RtcPeerConnection::CreateOffer(RtcOfferOptions ^ options)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Task<RtcSessionDescription^>^ RtcPeerConnection::CreateAnswer()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Task RtcPeerConnection::SetLocalDescription(RtcSessionDescription ^ description)
{
	throw gcnew NotImplementedException();
}

Task RtcPeerConnection::SetRemoteDescription(RtcSessionDescription ^ description)
{
	throw gcnew NotImplementedException();
}

void RtcPeerConnection::UpdateIce(RtcConfiguration ^ configuration)
{
	throw gcnew NotImplementedException();
}

Task RtcPeerConnection::AddIceCandidate(RtcIceCandidate ^ candidate)
{
	throw gcnew NotImplementedException();
}

RtcConfiguration ^ RtcPeerConnection::GetConfiguration()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStream^>^ RtcPeerConnection::GetLocalStreams()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStream^>^ RtcPeerConnection::GetRemoteStreams()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IMediaStream ^ RtcPeerConnection::GetStreamById(String ^ streamId)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void RtcPeerConnection::AddStream(IMediaStream ^ stream)
{
	throw gcnew NotImplementedException();
}

void RtcPeerConnection::RemoveStream(IMediaStream ^ stream)
{
	throw gcnew NotImplementedException();
}

void RtcPeerConnection::Close()
{
	throw gcnew NotImplementedException();
}

IRtcDataChannel ^ RtcPeerConnection::CreateDataChannel(String ^ label, RtcDataChannelInit dataChannelDict)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

Task<RtcStatsReport^>^ RtcPeerConnection::GetStats(IMediaStreamTrack ^ selector)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void RtcPeerConnection::SetIdentityProvider(String ^ provider, String ^ protocol, String ^ username)
{
	throw gcnew NotImplementedException();
}

Task<String^>^ RtcPeerConnection::GetIdentityAssertion()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

WEBRTCNET_END