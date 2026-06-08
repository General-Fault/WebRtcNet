#pragma once

namespace WebRtcInterop
{
	using namespace System;
	using namespace System::Collections::Generic;
	using namespace System::Threading::Tasks;
	using namespace WebRtcNet;

	public ref class RtcPeerConnection : WebRtcNet::RtcPeerConnection
	{
	public:
		RtcPeerConnection(RtcConfiguration^ configuration);

		virtual property Nullable<RtcSessionDescription> LocalDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> CurrentLocalDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> PendingLocalDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> RemoteDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> CurrentRemoteDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property Nullable<RtcSessionDescription> PendingRemoteDescription { Nullable<RtcSessionDescription> get() override; }
		virtual property RtcSignalingState SignalingState { RtcSignalingState get() override; }
		virtual property RtcIceGatheringState IceGatheringState { RtcIceGatheringState get() override; }
		virtual property RtcIceConnectionState IceConnectionState { RtcIceConnectionState get() override; }
		virtual property RtcPeerConnectionState ConnectionState { RtcPeerConnectionState get() override; }
		virtual property Nullable<bool> CanTrickleIceCandidates { Nullable<bool> get() override; }
		virtual property RtcConfiguration^ Configuration { RtcConfiguration^ get() override; void set(RtcConfiguration^ configuration) override; }
		virtual property RtcSctpTransport^ Sctp { RtcSctpTransport^ get() override; }

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
		virtual event EventHandler<RtcIceCandidateErrorEventArgs^>^ OnIceCandidateError
		{
			void add(EventHandler<RtcIceCandidateErrorEventArgs^>^ value) override { on_ice_candidate_error_ += value; }
			void remove(EventHandler<RtcIceCandidateErrorEventArgs^>^ value) override { on_ice_candidate_error_ -= value; }
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
		virtual event EventHandler<RtcDataChannelEventArgs^>^ OnDataChannel
		{
			void add(EventHandler<RtcDataChannelEventArgs^>^ value) override { on_data_channel_ += value; }
			void remove(EventHandler<RtcDataChannelEventArgs^>^ value) override { on_data_channel_ -= value; }
		}

		Task<RtcSessionDescription>^ CreateOffer([System::Runtime::InteropServices::Optional] RtcOfferOptions^ options) override;
		Task<RtcSessionDescription>^ CreateAnswer([System::Runtime::InteropServices::Optional] RtcAnswerOptions^ options) override;
		Task^ SetLocalDescription([System::Runtime::InteropServices::Optional] Nullable<RtcLocalSessionDescriptionInit> description) override;
		Task^ SetRemoteDescription(RtcSessionDescription description) override;
		Task^ AddIceCandidate([System::Runtime::InteropServices::Optional] RtcIceCandidate^ candidate) override;
		void RestartIce() override;
		void Close() override;
		Task<RtcStatsReport^>^ GetStats([System::Runtime::InteropServices::Optional] WebRtcNet::Media::MediaStreamTrack^ selector) override;

		IEnumerable<RtcRtpSender^>^ GetSenders() override;
		IEnumerable<RtcRtpReceiver^>^ GetReceivers() override;
		IEnumerable<RtcRtpTransceiver^>^ GetTransceivers() override;
		RtcRtpSender^ AddTrack(WebRtcNet::Media::MediaStreamTrack^ track, ... array<WebRtcNet::Media::MediaStream^>^ streams) override;
		RtcRtpTransceiver^ AddTransceiver(WebRtcNet::Media::MediaStreamTrack^ track, [System::Runtime::InteropServices::Optional] RtcRtpTransceiverInit^ init) override;
		RtcRtpTransceiver^ AddTransceiver(WebRtcNet::Media::MediaStreamTrackKind kind, [System::Runtime::InteropServices::Optional] RtcRtpTransceiverInit^ init) override;
		void RemoveTrack(RtcRtpSender^ sender) override;
		RtcDataChannel^ CreateDataChannel(String^ label, [System::Runtime::InteropServices::Optional] RtcDataChannelInit^ dataChannelInit) override;

	protected:
		IntPtr GetNativePeerConnectionHandle(bool throwOnDisposed) override;

	private:
		void ThrowShimNotImplemented(String^ memberName);

		RtcConfiguration^ configuration_;
		bool is_closed_;

		EventHandler^ on_negotiation_needed_;
		EventHandler<RtcIceCandidateEventArgs^>^ on_ice_candidate_;
		EventHandler<RtcIceCandidateErrorEventArgs^>^ on_ice_candidate_error_;
		EventHandler^ on_signaling_state_change_;
		EventHandler^ on_ice_connection_state_change_;
		EventHandler^ on_gathering_state_change_;
		EventHandler^ on_connection_state_change_;
		EventHandler<RtcTrackEventArgs^>^ on_track_;
		EventHandler<RtcDataChannelEventArgs^>^ on_data_channel_;
	};
}
