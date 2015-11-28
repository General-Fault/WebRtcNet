#pragma once

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

WebRtcObservers_Start
class PeerConnectionObserver;
WebRtcObservers_End

namespace WebRtcInterop {

public ref class RtcPeerConnection : WebRtcNet::IRtcPeerConnection
{
public:
	RtcPeerConnection(WebRtcNet::RtcConfiguration^ configuration);
	~RtcPeerConnection();
	
	// Inherited via IRtcPeerConnection
	virtual property WebRtcNet::RtcSessionDescription LocalDescription;
	virtual property WebRtcNet::RtcSessionDescription RemoteDescription;
	virtual property WebRtcNet::RtcSignalingState SignalingState;
	virtual property WebRtcNet::RtcGatheringState IceGatheringState;
	virtual property WebRtcNet::RtcIceConnectionState IceConnectionState;
	virtual property bool CanTrickleIceCandidates;
	virtual property WebRtcNet::RtcIdentityAssertion PeerIdentity;
	virtual event System::EventHandler<WebRtcNet::MediaStreamEventArgs ^> ^ OnAddStream;
	virtual event System::EventHandler<WebRtcNet::MediaStreamEventArgs ^> ^ OnRemoveStream;
	virtual event System::EventHandler ^ OnNegotiationNeeded;
	virtual event System::EventHandler<WebRtcNet::RtcIceCandidateEventArgs ^> ^ OnIceCandidate;
	virtual event System::EventHandler ^ OnSignalingStateChange;
	virtual event System::EventHandler ^ OnIceConnectionStateChange;
	virtual event System::EventHandler ^ OnGatheringStateChange;
	virtual event System::EventHandler<WebRtcNet::RtcDataChannelEventArgs ^> ^ OnDataChannel;
	virtual event System::EventHandler<WebRtcNet::RtcIdentityEventArgs ^> ^ OnIdentityResult;
	virtual event System::EventHandler ^ OnPeerIdentity;
	virtual event System::EventHandler<WebRtcNet::RtcIdentityErrorEventArgs ^> ^ OnIdpAssertiOnError;
	virtual event System::EventHandler<WebRtcNet::RtcIdentityErrorEventArgs ^> ^ OnIdpValidatiOnError;
	virtual System::Threading::Tasks::Task<WebRtcNet::RtcSessionDescription> ^ CreateOffer(WebRtcNet::RtcOfferOptions ^options);
	virtual System::Threading::Tasks::Task<WebRtcNet::RtcSessionDescription> ^ CreateAnswer();
	virtual System::Threading::Tasks::Task ^ SetLocalDescription(WebRtcNet::RtcSessionDescription description);
	virtual System::Threading::Tasks::Task ^ SetRemoteDescription(WebRtcNet::RtcSessionDescription description);
	virtual void UpdateIce(WebRtcNet::RtcConfiguration ^configuration);
	virtual System::Threading::Tasks::Task ^ AddIceCandidate(WebRtcNet::RtcIceCandidate candidate);
	virtual WebRtcNet::RtcConfiguration ^ GetConfiguration();
	virtual void SetConfiguration(WebRtcNet::RtcConfiguration ^configuration);
	virtual System::Collections::Generic::IEnumerable<WebRtcNet::IMediaStream ^> ^ GetLocalStreams();
	virtual System::Collections::Generic::IEnumerable<WebRtcNet::IMediaStream ^> ^ GetRemoteStreams();
	virtual WebRtcNet::IMediaStream ^ GetStreamById(System::String ^streamId);
	virtual void AddStream(WebRtcNet::IMediaStream ^stream);
	virtual void RemoveStream(WebRtcNet::IMediaStream ^stream);
	virtual void Close();
	virtual WebRtcNet::IRtcDataChannel ^ CreateDataChannel(System::String ^label, WebRtcNet::RtcDataChannelInit ^dataChannelInit);
	virtual WebRtcNet::IRtcDtmfSender ^ CreateRtcDtmfSender(WebRtcNet::IMediaStreamTrack ^track);
	virtual System::Threading::Tasks::Task<WebRtcNet::IRtcStatsReport ^> ^ GetStats(WebRtcNet::IMediaStreamTrack ^selector);
	virtual void SetIdentityProvider(System::String ^provider, System::String ^protocol, System::String ^username);
	virtual void GetIdentityAssertion();

internal:
	!RtcPeerConnection();
	webrtc::PeerConnectionFactoryInterface* GetNativePeerConnectionFactory(bool throwOnDisposed);
	webrtc::PeerConnectionInterface* GetNativePeerConnection(bool throwOnDisposed);

	//Event invocation 
	void FireOnSignalingStateChange(WebRtcNet::RtcSignalingState newState) { OnSignalingStateChange(this, System::EventArgs::Empty); }
	void FireOnAddStream(WebRtcNet::IMediaStream ^ stream) { OnAddStream(this, gcnew WebRtcNet::MediaStreamEventArgs(stream)); }
	void FireOnRemoveStream(WebRtcNet::IMediaStream ^ stream) { OnRemoveStream(this, gcnew WebRtcNet::MediaStreamEventArgs(stream)); }
	void FireOnDataChannel(WebRtcNet::IRtcDataChannel ^ channel) { OnDataChannel(this, gcnew WebRtcNet::RtcDataChannelEventArgs(channel)); }
	void FireOnNegotiationNeeded() { OnNegotiationNeeded(this, System::EventArgs::Empty); }
	void FireOnIceConnectionStateChange(WebRtcNet::RtcIceConnectionState newState) { OnIceConnectionStateChange(this, System::EventArgs::Empty); }
	void FireOnGatheringStateChange(WebRtcNet::RtcGatheringState newState) { OnGatheringStateChange(this, System::EventArgs::Empty); }
	void FireOnIceCandidate(WebRtcNet::RtcIceCandidate candidate) { OnIceCandidate(this, gcnew WebRtcNet::RtcIceCandidateEventArgs(candidate)); }

private:
	static rtc::Thread* _signalThread;
	static rtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>* _rpPeerConnectionFactory;
	rtc::scoped_refptr<webrtc::PeerConnectionInterface>* _rpPeerConnection;
	webrtc_observers::PeerConnectionObserver* _observer;

};

}
