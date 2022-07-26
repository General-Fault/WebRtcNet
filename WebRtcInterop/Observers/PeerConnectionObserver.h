#pragma once

#include <msclr/auto_gcroot.h>

#include "../RtcPeerConnection.h"
#include "api/peer_connection_interface.h"
using namespace msclr;

WebRtcObservers_Start

private class PeerConnectionObserver : public webrtc::PeerConnectionObserver
{
public:
	// Triggered when the SignalingState changed.
	void OnSignalingChange(webrtc::PeerConnectionInterface::SignalingState new_state) override;

	// Triggered when media is received on a new stream from remote peer.
	void OnAddStream(rtc::scoped_refptr<webrtc::MediaStreamInterface> stream) override;

	// Triggered when a remote peer close a stream.
	void OnRemoveStream(rtc::scoped_refptr <webrtc::MediaStreamInterface> stream) override;

	// Triggered when a remote peer open a data channel.
	void OnDataChannel(rtc::scoped_refptr <webrtc::DataChannelInterface> data_channel) override;

	// Triggered when renegotiation is needed, for example the ICE has restarted.
	// [Obsolete] Use OnNegotiationNeededEvent
	void OnRenegotiationNeeded() override;

	// Triggered when renegotiation is needed, for example the ICE has restarted.
	void OnNegotiationNeededEvent(uint32_t event_id) override;

	// Called any time the IceConnectionState changes
	// [Deptricated]
	void OnIceConnectionChange(webrtc::PeerConnectionInterface::IceConnectionState new_state) override;

	// Called any time the standards-compliant IceConnectionState changes.
	void OnStandardizedIceConnectionChange(
		webrtc::PeerConnectionInterface::IceConnectionState new_state) override;

	// Called any time the PeerConnectionState changes.
	void OnConnectionChange(webrtc::PeerConnectionInterface::PeerConnectionState new_state) override;

	// Called any time the IceGatheringState changes
	void OnIceGatheringChange(webrtc::PeerConnectionInterface::IceGatheringState new_state) override;

	// New Ice candidate have been found.
	void OnIceCandidate(const webrtc::IceCandidateInterface* candidate) override;

	// Gathering of an ICE candidate failed.
	// See https://w3c.github.io/webrtc-pc/#event-icecandidateerror
	void OnIceCandidateError(const std::string& address,
		int port,
		const std::string& url,
		int error_code,
		const std::string& error_text) override;

	// Ice candidates have been removed.
	void OnIceCandidatesRemoved(const std::vector<cricket::Candidate>& candidates) override {}

	// Called when the ICE connection receiving status changes.
	void OnIceConnectionReceivingChange(bool receiving) override;

	// Called when the selected candidate pair for the ICE connection changes.
	void OnIceSelectedCandidatePairChanged(
		const cricket::CandidatePairChangeEvent& event) override;

	// This is called when a receiver and its track are created.
	// TODO(zhihuang): Make this pure virtual when all subclasses implement it.
	// Note: This is called with both Plan B and Unified Plan semantics. Unified
	// Plan users should prefer OnTrack, OnAddTrack is only called as backwards
	// compatibility (and is called in the exact same situations as OnTrack).
	void OnAddTrack(
		rtc::scoped_refptr<webrtc::RtpReceiverInterface> receiver,
		const std::vector<rtc::scoped_refptr<webrtc::MediaStreamInterface>>& streams) override;

	// This is called when signaling indicates a transceiver will be receiving
	// media from the remote endpoint. This is fired during a call to
	// SetRemoteDescription. The receiving track can be accessed by:
	// `transceiver->receiver()->track()` and its associated streams by
	// `transceiver->receiver()->streams()`.
	// Note: This will only be called if Unified Plan semantics are specified.
	// This behavior is specified in section 2.2.8.2.5 of the "Set the
	// RTCSessionDescription" algorithm:
	// https://w3c.github.io/webrtc-pc/#set-description
	void OnTrack(
		rtc::scoped_refptr<webrtc::RtpTransceiverInterface> transceiver) override;

	// Called when signaling indicates that media will no longer be received on a
	// track.
	// With Plan B semantics, the given receiver will have been removed from the
	// PeerConnection and the track muted.
	// With Unified Plan semantics, the receiver will remain but the transceiver
	// will have changed direction to either sendonly or inactive.
	// https://w3c.github.io/webrtc-pc/#process-remote-track-removal
	void OnRemoveTrack(
		rtc::scoped_refptr<webrtc::RtpReceiverInterface> receiver) override;

	// Called when an interesting usage is detected by WebRTC.
	// An appropriate action is to add information about the context of the
	// PeerConnection and write the event to some kind of "interesting events"
	// log function.
	// The heuristics for defining what constitutes "interesting" are
	// implementation-defined.
	void OnInterestingUsage(int usage_pattern) override;

	PeerConnectionObserver(RtcPeerConnection ^ peerConnection);

	~PeerConnectionObserver() override;

private:
	gcroot<RtcPeerConnection^> _peerConnection;
};

WebRtcObservers_End