#pragma once

namespace rtc
{
	template <class T>
	class scoped_refptr;
}

namespace webrtc
{
	class PeerConnectionInterface;
}

WebRtcObservers_Start
	class PeerConnectionObserver;
WebRtcObservers_End

namespace WebRtcInterop
{
	public ref class RtcPeerConnection : IRtcPeerConnection
	{
	public:
		RtcPeerConnection(RtcConfiguration^ configuration);
		~RtcPeerConnection();

		// Inherited via IRtcPeerConnection
		virtual property Nullable<RtcSessionDescription> LocalDescription { Nullable<RtcSessionDescription> get(); }
		virtual property Nullable<RtcSessionDescription> CurrentLocalDescription { Nullable<RtcSessionDescription> get(); }
		virtual property Nullable<RtcSessionDescription> PendingLocalDescription { Nullable<RtcSessionDescription> get(); }

		virtual property Nullable<RtcSessionDescription> RemoteDescription { Nullable<RtcSessionDescription> get(); }
		virtual property Nullable<RtcSessionDescription> CurrentRemoteDescription { Nullable<RtcSessionDescription> get(); }
		virtual property Nullable<RtcSessionDescription> PendingRemoteDescription { Nullable<RtcSessionDescription> get(); }

		virtual property RtcPeerConnectionState ConnectionState { RtcPeerConnectionState get(); }
		virtual property RtcSignalingState SignalingState { RtcSignalingState get(); }
		virtual property RtcIceGatheringState IceGatheringState { RtcIceGatheringState get(); }
		virtual property RtcIceConnectionState IceConnectionState { RtcIceConnectionState get(); }
		virtual property bool CanTrickleIceCandidates { bool get(); }
		virtual property RtcConfiguration^ Configuration { RtcConfiguration^ get(); void set(RtcConfiguration^ configuration); }

		virtual event EventHandler^ OnNegotiationNeeded;
		virtual event EventHandler<RtcIceCandidateEventArgs^>^ OnIceCandidate;
		virtual event EventHandler^ OnSignalingStateChange;
		virtual event EventHandler^ OnIceConnectionStateChange;
		virtual event EventHandler^ OnGatheringStateChange;
		virtual event EventHandler<RtcDataChannelEventArgs^>^ OnDataChannel;
		virtual event EventHandler<RtcIceCandidateErrorEventArgs^>^ OnIceCandidateError;
		virtual event EventHandler^ OnConnectionStateChange;
		virtual event EventHandler<RtcTrackEventArgs^>^ OnTrack;

		virtual Task<RtcSessionDescription>^ CreateOffer([System::Runtime::InteropServices::Optional] RtcOfferOptions^ options);
		virtual Task<RtcSessionDescription>^ CreateAnswer([System::Runtime::InteropServices::Optional] RtcAnswerOptions^ options);

		virtual Task^ AddIceCandidate(IRtcIceCandidate^ candidate);
		virtual void RestartIce();

		virtual Task^ SetLocalDescription(RtcSessionDescription description);
		virtual Task^ SetRemoteDescription(RtcSessionDescription description);

		virtual IRtcRtpSender^ AddTrack(WebRtcNet::Media::IMediaStreamTrack^ track,
			... array<WebRtcNet::Media::IMediaStream^>^ streams);
		virtual void RemoveTrack(WebRtcNet::Media::IMediaStreamTrack^ track);

		virtual IRtcRtpTransceiver^ AddTransceiver(WebRtcNet::Media::IMediaStreamTrack^ track,
			IRtcRtpTransceiver^ transceiver);
		virtual IRtcRtpTransceiver^ AddTransceiver(WebRtcNet::Media::MediaStreamTrackKind kind,
			IRtcRtpTransceiver^ transceiver);

		virtual IEnumerable<IRtcRtpSender^>^ GetSenders();
		virtual IEnumerable<IRtcRtpReceiver^>^ GetReceivers();
		virtual IEnumerable<IRtcRtpTransceiver^>^ GetTransceivers();

		virtual IRtcDataChannel^ CreateDataChannel(String^ label, RtcDataChannelInit^ dataChannelInit);

		virtual void Close();

		virtual Task<IRtcStatsReport^>^ GetStats([System::Runtime::InteropServices::Optional] WebRtcNet::Media::IMediaStreamTrack^ selector);

	internal:
		!RtcPeerConnection();
		webrtc::PeerConnectionInterface* GetNativePeerConnection(bool throwOnDisposed);

		//Event invocation 
		void FireOnSignalingStateChange(RtcSignalingState newState) { OnSignalingStateChange(this, EventArgs::Empty); }
		void FireOnDataChannel(IRtcDataChannel^ channel) { OnDataChannel(this, gcnew RtcDataChannelEventArgs(channel)); }
		void FireOnNegotiationNeeded() { OnNegotiationNeeded(this, EventArgs::Empty); }
		void FireOnIceConnectionStateChange(RtcIceConnectionState newState) { OnIceConnectionStateChange(this, EventArgs::Empty); }
		void FireOnGatheringStateChange(RtcIceGatheringState newState) { OnGatheringStateChange(this, EventArgs::Empty); }
		void FireOnIceCandidate(IRtcIceCandidate^ candidate) { OnIceCandidate(this, gcnew RtcIceCandidateEventArgs(candidate)); }

	private:
		rtc::scoped_refptr<webrtc::PeerConnectionInterface>* rp_peer_connection_;
		webrtc_observers::PeerConnectionObserver* observer_;
		RtcConfiguration^ configuration_;
	};
}
