#pragma once

#include "..\RtcPeerConnection.h"

#include <msclr\auto_gcroot.h>
using namespace msclr;

WebRtcObservers_Start

private class PeerConnectionObserver : public webrtc::PeerConnectionObserver
{
public:
	// Triggered when the SignalingState changed.
	virtual void OnSignalingChange(webrtc::PeerConnectionInterface::SignalingState new_state);

	virtual void OnStateChange(webrtc::PeerConnectionObserver::StateType state_changed) { /* Obsolete. Ignore. */ }

	// Triggered when media is received on a new stream from remote peer.
	virtual void OnAddStream(webrtc::MediaStreamInterface* stream);

	// Triggered when a remote peer close a stream.
	virtual void OnRemoveStream(webrtc::MediaStreamInterface* stream);

	// Triggered when a remote peer open a data channel.
	virtual void OnDataChannel(webrtc::DataChannelInterface* data_channel);

	// Triggered when renegotiation is needed, for example the ICE has restarted.
	virtual void OnRenegotiationNeeded();

	// Called any time the IceConnectionState changes
	virtual void OnIceConnectionChange(webrtc::PeerConnectionInterface::IceConnectionState new_state);

	// Called any time the IceGatheringState changes
	virtual void OnIceGatheringChange(webrtc::PeerConnectionInterface::IceGatheringState new_state);

	// New Ice candidate have been found.
	virtual void OnIceCandidate(const webrtc::IceCandidateInterface* candidate);

	virtual void OnIceComplete() { /* Obsolete. Ignore. */ } 

	// Called when the ICE connection receiving status changes.
	virtual void OnIceConnectionReceivingChange(bool receiving) { /* Not Implemented */ };

	PeerConnectionObserver(RtcPeerConnection ^ peerConnection);

	~PeerConnectionObserver();

private:
	gcroot<RtcPeerConnection^> _peerConnection;
};

WebRtcObservers_End