#include "stdafx.h"

#include "api/peer_connection_interface.h"
#include "api/media_stream_interface.h"

using namespace System;
using namespace Collections::Generic;
using namespace Runtime::InteropServices;
using namespace Threading::Tasks;

#include "PeerConnectionObserver.h"
#include "../RtcDataChannel.h"
#include "../RtcPeerConnection.h"
#include "../MediaStream.h"
#include "../Marshaling/MarshalPeerConnection.h"
#include "../Marshaling/MarshalIceCandidate.h"


WebRtcObservers_Start

PeerConnectionObserver::PeerConnectionObserver(RtcPeerConnection^ peerConnection)
	: _peerConnection(peerConnection)
{
	if (peerConnection == nullptr) throw gcnew ArgumentNullException("peerConnection");
}

PeerConnectionObserver::~PeerConnectionObserver()
{
	_peerConnection = nullptr;
}

void PeerConnectionObserver::OnSignalingChange(webrtc::PeerConnectionInterface::SignalingState new_state)
{
	_peerConnection->FireOnSignalingStateChange(marshal_as<WebRtcNet::RtcSignalingState>(new_state));
}

void PeerConnectionObserver::OnAddStream(rtc::scoped_refptr<webrtc::MediaStreamInterface> stream)
{
	_peerConnection->FireOnAddStream(gcnew MediaStream(stream));
}

void PeerConnectionObserver::OnRemoveStream(rtc::scoped_refptr<webrtc::MediaStreamInterface> stream)
{
	_peerConnection->FireOnRemoveStream(gcnew MediaStream(stream));
}

void PeerConnectionObserver::OnDataChannel(rtc::scoped_refptr<webrtc::DataChannelInterface> data_channel)
{
	_peerConnection->FireOnDataChannel(gcnew RtcDataChannel(data_channel));
}

void PeerConnectionObserver::OnRenegotiationNeeded()
{
	_peerConnection->FireOnNegotiationNeeded();
}

void PeerConnectionObserver::OnIceConnectionChange(webrtc::PeerConnectionInterface::IceConnectionState new_state)
{
	_peerConnection->FireOnIceConnectionStateChange(marshal_as<WebRtcNet::RtcIceConnectionState>(new_state));
}

void PeerConnectionObserver::OnIceGatheringChange(webrtc::PeerConnectionInterface::IceGatheringState new_state)
{
	_peerConnection->FireOnGatheringStateChange(marshal_as<WebRtcNet::RtcGatheringState>(new_state));
}

void PeerConnectionObserver::OnIceCandidate(const webrtc::IceCandidateInterface* candidate)
{
	auto iceCandidate = marshal_as<WebRtcNet::RtcIceCandidate>(candidate);
	_peerConnection->FireOnIceCandidate(iceCandidate);
	delete iceCandidate;
}

}}
