#include "stdafx.h"

#include "talk\app\webrtc\peerconnectioninterface.h"
#include "talk\app\webrtc\mediastreaminterface.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace System::Threading::Tasks;

#include "PeerConnectionObserver.h"
#include "..\RtcDataChannel.h"
#include "..\RtcPeerConnection.h"
#include "..\MediaStream.h"
#include "..\Marshaling\MarshalPeerConnection.h"
#include "..\Marshaling\MarshalIceCandidate.h"


WebRtcObservers_Start

PeerConnectionObserver::PeerConnectionObserver(WebRtcInterop::RtcPeerConnection ^ peerConnection)
	: _peerConnection(peerConnection)
{}

PeerConnectionObserver::~PeerConnectionObserver()
{
	_peerConnection = nullptr;
}

void PeerConnectionObserver::OnSignalingChange(webrtc::PeerConnectionInterface::SignalingState new_state)
{
	_peerConnection->FireOnSignalingStateChange(marshal_as(new_state));
}

void PeerConnectionObserver::OnAddStream(webrtc::MediaStreamInterface * stream)
{
	_peerConnection->FireOnAddStream(gcnew WebRtcInterop::MediaStream(stream));
}

void PeerConnectionObserver::OnRemoveStream(webrtc::MediaStreamInterface * stream)
{
	_peerConnection->FireOnRemoveStream(gcnew WebRtcInterop::MediaStream(stream));
}

void PeerConnectionObserver::OnDataChannel(webrtc::DataChannelInterface* data_channel)
{
	_peerConnection->FireOnDataChannel(gcnew WebRtcInterop::RtcDataChannel(data_channel));
}

void PeerConnectionObserver::OnRenegotiationNeeded()
{
	_peerConnection->FireOnNegotiationNeeded();
}

void PeerConnectionObserver::OnIceConnectionChange(webrtc::PeerConnectionInterface::IceConnectionState new_state)
{
	_peerConnection->FireOnIceConnectionStateChange(marshal_as(new_state));
}

void PeerConnectionObserver::OnIceGatheringChange(webrtc::PeerConnectionInterface::IceGatheringState new_state)
{
	_peerConnection->FireOnGatheringStateChange(marshal_as(new_state));
}

void PeerConnectionObserver::OnIceCandidate(const webrtc::IceCandidateInterface* candidate)
{
	auto iceCandidate = marshal_as<WebRtcNet::RtcIceCandidate>(candidate);
	_peerConnection->FireOnIceCandidate(iceCandidate);
	delete iceCandidate;
}

}}
