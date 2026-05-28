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
		virtual property Nullable<RtcSessionDescription> LocalDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> CurrentLocalDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> PendingLocalDescription { Nullable<RtcSessionDescription> get() override; }

		virtual property Nullable<RtcSessionDescription> RemoteDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> CurrentRemoteDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> PendingRemoteDescription { Nullable<RtcSessionDescription> get() override; }

		virtual property RtcPeerConnectionState ConnectionState { RtcPeerConnectionState get() override; }
		virtual property RtcSignalingState SignalingState { RtcSignalingState get() override; }
		virtual property RtcIceGatheringState IceGatheringState { RtcIceGatheringState get() override; }
		virtual property RtcIceConnectionState IceConnectionState { RtcIceConnectionState get() override; }
		virtual property bool CanTrickleIceCandidates { bool get() override; }
		virtual property RtcConfiguration^ Configuration { RtcConfiguration^ get() override; void set(RtcConfiguration^ configuration) override; }

		virtual event EventHandler^ OnNegotiationNeeded
		{
			void add(EventHandler^ value) override { on_negotiation_needed_ += value; }
			void remove(EventHandler^ value) override { on_negotiation_needed_ -= value; }
		}
		virtual event EventHandler<RtcIceCandidateEventArgs^>^ OnIceCandidate
		{
			void add(EventHandler<RtcIceCandidateEventArgs^>^ value) override { on_ice_candidate_ += value; }
			void remove(EventHandler<RtcIceCandidateEventArgs^>^ value) override { on_ice_candidate_ -= value; }
		}
		virtual event EventHandler^ OnSignalingStateChange
		{
			void add(EventHandler^ value) override { on_signaling_state_change_ += value; }
			void remove(EventHandler^ value) override { on_signaling_state_change_ -= value; }
		}
		virtual event EventHandler^ OnIceConnectionStateChange
		{
			void add(EventHandler^ value) override { on_ice_connection_state_change_ += value; }
			void remove(EventHandler^ value) override { on_ice_connection_state_change_ -= value; }
		}
		virtual event EventHandler^ OnGatheringStateChange
		{
			void add(EventHandler^ value) override { on_gathering_state_change_ += value; }
			void remove(EventHandler^ value) override { on_gathering_state_change_ -= value; }
		}
		virtual event EventHandler<RtcDataChannelEventArgs^>^ OnDataChannel
		{
			void add(EventHandler<RtcDataChannelEventArgs^>^ value) override { on_data_channel_ += value; }
			void remove(EventHandler<RtcDataChannelEventArgs^>^ value) override { on_data_channel_ -= value; }
		}
		virtual event EventHandler<RtcIceCandidateErrorEventArgs^>^ OnIceCandidateError
		{
			void add(EventHandler<RtcIceCandidateErrorEventArgs^>^ value) override { on_ice_candidate_error_ += value; }
			void remove(EventHandler<RtcIceCandidateErrorEventArgs^>^ value) override { on_ice_candidate_error_ -= value; }
		}
		virtual event EventHandler^ OnConnectionStateChange
		{
			void add(EventHandler^ value) override { on_connection_state_change_ += value; }
			void remove(EventHandler^ value) override { on_connection_state_change_ -= value; }
		}
		virtual event EventHandler<RtcTrackEventArgs^>^ OnTrack
		{
			void add(EventHandler<RtcTrackEventArgs^>^ value) override { on_track_ += value; }
			void remove(EventHandler<RtcTrackEventArgs^>^ value) override { on_track_ -= value; }
		}

		virtual Task<RtcSessionDescription>^ CreateOffer([System::Runtime::InteropServices::Optional] RtcOfferOptions^ options) override;
		virtual Task<RtcSessionDescription>^ CreateAnswer([System::Runtime::InteropServices::Optional] RtcAnswerOptions^ options) override;

		virtual Task^ AddIceCandidate(RtcIceCandidate^ candidate) override;
		virtual void RestartIce() override;

		virtual Task^ SetLocalDescription(RtcSessionDescription description) override;
		virtual Task^ SetRemoteDescription(RtcSessionDescription description) override;

		virtual IRtcRtpSender^ AddTrack(WebRtcNet::Media::IMediaStreamTrack^ track,
			... array<WebRtcNet::Media::IMediaStream^>^ streams) override;
		virtual void RemoveTrack(WebRtcNet::Media::IMediaStreamTrack^ track) override;

		virtual IRtcRtpTransceiver^ AddTransceiver(WebRtcNet::Media::IMediaStreamTrack^ track,
			IRtcRtpTransceiver^ transceiver) override;
		virtual IRtcRtpTransceiver^ AddTransceiver(WebRtcNet::Media::MediaStreamTrackKind kind,
			IRtcRtpTransceiver^ transceiver) override;

		virtual IEnumerable<IRtcRtpSender^>^ GetSenders() override;
		virtual IEnumerable<IRtcRtpReceiver^>^ GetReceivers() override;
		virtual IEnumerable<IRtcRtpTransceiver^>^ GetTransceivers() override;

		virtual IRtcDataChannel^ CreateDataChannel(String^ label, RtcDataChannelInit^ dataChannelInit) override;

		virtual void Close() override;

		virtual Task<IRtcStatsReport^>^ GetStats([System::Runtime::InteropServices::Optional] WebRtcNet::Media::IMediaStreamTrack^ selector) override;

	internal:
		!RtcPeerConnection();
		webrtc::PeerConnectionInterface* GetNativePeerConnection(bool throwOnDisposed);
		virtual System::IntPtr GetNativePeerConnectionHandle(bool throwOnDisposed);

		//Event invocation 
		void FireOnSignalingStateChange(RtcSignalingState newState) { if (on_signaling_state_change_ != nullptr) on_signaling_state_change_(this, EventArgs::Empty); }
		void FireOnDataChannel(IRtcDataChannel^ channel) { if (on_data_channel_ != nullptr) on_data_channel_(this, gcnew RtcDataChannelEventArgs(channel)); }
		void FireOnNegotiationNeeded() { if (on_negotiation_needed_ != nullptr) on_negotiation_needed_(this, EventArgs::Empty); }
		void FireOnIceConnectionStateChange(RtcIceConnectionState newState) { if (on_ice_connection_state_change_ != nullptr) on_ice_connection_state_change_(this, EventArgs::Empty); }
		void FireOnGatheringStateChange(RtcIceGatheringState newState) { if (on_gathering_state_change_ != nullptr) on_gathering_state_change_(this, EventArgs::Empty); }
		void FireOnConnectionStateChange() { if (on_connection_state_change_ != nullptr) on_connection_state_change_(this, EventArgs::Empty); }
		void FireOnIceCandidate(RtcIceCandidate^ candidate) { if (on_ice_candidate_ != nullptr) on_ice_candidate_(this, gcnew RtcIceCandidateEventArgs(candidate)); }

	private:
		rtc::scoped_refptr<webrtc::PeerConnectionInterface>* rp_peer_connection_;
		webrtc_observers::PeerConnectionObserver* observer_;
		RtcConfiguration^ configuration_;
		EventHandler^ on_negotiation_needed_;
		EventHandler<RtcIceCandidateEventArgs^>^ on_ice_candidate_;
		EventHandler^ on_signaling_state_change_;
		EventHandler^ on_ice_connection_state_change_;
		EventHandler^ on_gathering_state_change_;
		EventHandler<RtcDataChannelEventArgs^>^ on_data_channel_;
		EventHandler<RtcIceCandidateErrorEventArgs^>^ on_ice_candidate_error_;
		EventHandler^ on_connection_state_change_;
		EventHandler<RtcTrackEventArgs^>^ on_track_;
	};
}
