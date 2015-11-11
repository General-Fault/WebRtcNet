#pragma once

#include "IRtcPeerConnection.h"

namespace rtc
{
	template <class T> class scoped_refptr;
	class Thread;
}

namespace webrtc
{
	class PeerConnectionFactoryInterface;
	class PeerConnectionInterface;
}

WEBRTCNET_START

public ref class RtcPeerConnection : IRtcPeerConnection
{
public:
	RtcPeerConnection(RtcConfiguration^ configuration);

	// Inherited via IRtcPeerConnection
	virtual Task<RtcSessionDescription^>^ CreateOffer(RtcOfferOptions ^ options);
	virtual Task<RtcSessionDescription^>^ CreateAnswer();
	virtual Task SetLocalDescription(RtcSessionDescription ^ description);
	virtual property RtcSessionDescription ^ LocalDescription;
	virtual Task SetRemoteDescription(RtcSessionDescription ^ description);
	virtual property RtcSessionDescription ^ RemoteDescription;
	virtual property RtcSignalingState SignalingState;
	virtual void UpdateIce(RtcConfiguration ^ configuration);
	virtual Task AddIceCandidate(RtcIceCandidate ^ candidate);
	virtual property RtcGatheringState IceGatheringState;
	virtual property RtcIceConnectionState IceConnectionState;
	virtual property Boolean CanTrickleIceCandidates;
	virtual RtcConfiguration ^ GetConfiguration();
	virtual IEnumerable<IMediaStream^>^ GetLocalStreams();
	virtual IEnumerable<IMediaStream^>^ GetRemoteStreams();
	virtual IMediaStream ^ GetStreamById(String ^ streamId);
	virtual void AddStream(IMediaStream ^ stream);
	virtual void RemoveStream(IMediaStream ^ stream);
	virtual void Close();
	virtual event EventHandler ^ OnNegotiationNeeded;
	virtual event EventHandler<RtcIceCandidate^>^ OnIceCandidate;
	virtual event EventHandler ^ OnSignalingStateChange;
	virtual event EventHandler<IMediaStream^>^ OnAddStream;
	virtual event EventHandler<IMediaStream^>^ OnRemoveStream;
	virtual event EventHandler ^ OnIceConnectionStateChange;
	virtual event EventHandler ^ OnGatheringStateChange;
	virtual IRtcDataChannel ^ CreateDataChannel(String ^ label, RtcDataChannelInit dataChannelDict);
	virtual event EventHandler<IRtcDataChannel^>^ OnDataChannel;
	virtual Task<RtcStatsReport^>^ GetStats(IMediaStreamTrack ^ selector);
	virtual void SetIdentityProvider(String ^ provider, String ^ protocol, String ^ username);
	virtual Task<String^>^ GetIdentityAssertion();
	virtual property Task<RtcIdentityAssertion^>^ PeerIdentity;
	virtual property String ^ IdpLoginUrl;

	~RtcPeerConnection();

internal:
	!RtcPeerConnection();
	webrtc::PeerConnectionFactoryInterface* GetNativePeerConnectionFactory();
	webrtc::PeerConnectionInterface* GetNativePeerConnection();

private:
	static RtcPeerConnection();

	static rtc::Thread* _signalThread;
	static rtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>* _rpPeerConnectionFactory;
	rtc::scoped_refptr<webrtc::PeerConnectionInterface>* _rpPeerConnection;
};
WEBRTCNET_END
