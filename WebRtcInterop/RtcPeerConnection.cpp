#include "pch.h"

#include "RtcPeerConnection.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;

namespace WebRtcInterop
{
	RtcPeerConnection::RtcPeerConnection(RtcConfiguration^ configuration)
		: configuration_(configuration),
		  is_closed_(false),
		  on_negotiation_needed_(nullptr),
		  on_ice_candidate_(nullptr),
		  on_ice_candidate_error_(nullptr),
		  on_signaling_state_change_(nullptr),
		  on_ice_connection_state_change_(nullptr),
		  on_gathering_state_change_(nullptr),
		  on_connection_state_change_(nullptr),
		  on_track_(nullptr),
		  on_data_channel_(nullptr)
	{
		if (configuration == nullptr)
			throw gcnew ArgumentNullException("configuration");
	}

	void RtcPeerConnection::ThrowShimNotImplemented(String^ memberName)
	{
		throw gcnew NotImplementedException(String::Format(
			"{0} is a compile-only shim in WebRtcInterop and is not implemented yet.",
			memberName));
	}

	IntPtr RtcPeerConnection::GetNativePeerConnectionHandle(bool throwOnDisposed)
	{
		if (throwOnDisposed && is_closed_)
			throw gcnew ObjectDisposedException(NAMEOF(RtcPeerConnection));

		return IntPtr::Zero;
	}

	Nullable<RtcSessionDescription> RtcPeerConnection::LocalDescription::get() { return Nullable<RtcSessionDescription>(); }
	Nullable<RtcSessionDescription> RtcPeerConnection::CurrentLocalDescription::get() { return Nullable<RtcSessionDescription>(); }
	Nullable<RtcSessionDescription> RtcPeerConnection::PendingLocalDescription::get() { return Nullable<RtcSessionDescription>(); }
	Nullable<RtcSessionDescription> RtcPeerConnection::RemoteDescription::get() { return Nullable<RtcSessionDescription>(); }
	Nullable<RtcSessionDescription> RtcPeerConnection::CurrentRemoteDescription::get() { return Nullable<RtcSessionDescription>(); }
	Nullable<RtcSessionDescription> RtcPeerConnection::PendingRemoteDescription::get() { return Nullable<RtcSessionDescription>(); }
	RtcSignalingState RtcPeerConnection::SignalingState::get() { return is_closed_ ? RtcSignalingState::Closed : RtcSignalingState::Stable; }
	RtcIceGatheringState RtcPeerConnection::IceGatheringState::get() { return is_closed_ ? RtcIceGatheringState::Complete : RtcIceGatheringState::New; }
	RtcIceConnectionState RtcPeerConnection::IceConnectionState::get() { return is_closed_ ? RtcIceConnectionState::Closed : RtcIceConnectionState::New; }
	RtcPeerConnectionState RtcPeerConnection::ConnectionState::get() { return is_closed_ ? RtcPeerConnectionState::Closed : RtcPeerConnectionState::New; }
	Nullable<bool> RtcPeerConnection::CanTrickleIceCandidates::get() { return Nullable<bool>(); }
	RtcConfiguration^ RtcPeerConnection::Configuration::get() { return configuration_; }
	void RtcPeerConnection::Configuration::set(RtcConfiguration^ configuration)
	{
		if (configuration == nullptr)
			throw gcnew ArgumentNullException("configuration");
		configuration_ = configuration;
	}

	RtcSctpTransport^ RtcPeerConnection::Sctp::get() { return nullptr; }

	Task<RtcSessionDescription>^ RtcPeerConnection::CreateOffer(RtcOfferOptions^ options)
	{
		ThrowShimNotImplemented("RtcPeerConnection.CreateOffer");
		return nullptr;
	}

	Task<RtcSessionDescription>^ RtcPeerConnection::CreateAnswer(RtcAnswerOptions^ options)
	{
		ThrowShimNotImplemented("RtcPeerConnection.CreateAnswer");
		return nullptr;
	}

	Task^ RtcPeerConnection::SetLocalDescription(Nullable<RtcLocalSessionDescriptionInit> description)
	{
		ThrowShimNotImplemented("RtcPeerConnection.SetLocalDescription");
		return nullptr;
	}

	Task^ RtcPeerConnection::SetRemoteDescription(RtcSessionDescription description)
	{
		ThrowShimNotImplemented("RtcPeerConnection.SetRemoteDescription");
		return nullptr;
	}

	Task^ RtcPeerConnection::AddIceCandidate(RtcIceCandidate^ candidate)
	{
		ThrowShimNotImplemented("RtcPeerConnection.AddIceCandidate");
		return nullptr;
	}

	void RtcPeerConnection::RestartIce()
	{
		ThrowShimNotImplemented("RtcPeerConnection.RestartIce");
	}

	void RtcPeerConnection::Close()
	{
		is_closed_ = true;
	}

	Task<RtcStatsReport^>^ RtcPeerConnection::GetStats(WebRtcNet::Media::MediaStreamTrack^ selector)
	{
		ThrowShimNotImplemented("RtcPeerConnection.GetStats");
		return nullptr;
	}

	IEnumerable<RtcRtpSender^>^ RtcPeerConnection::GetSenders()
	{
		return gcnew List<RtcRtpSender^>();
	}

	IEnumerable<RtcRtpReceiver^>^ RtcPeerConnection::GetReceivers()
	{
		return gcnew List<RtcRtpReceiver^>();
	}

	IEnumerable<RtcRtpTransceiver^>^ RtcPeerConnection::GetTransceivers()
	{
		return gcnew List<RtcRtpTransceiver^>();
	}

	RtcRtpSender^ RtcPeerConnection::AddTrack(WebRtcNet::Media::MediaStreamTrack^ track, ... array<WebRtcNet::Media::MediaStream^>^ streams)
	{
		ThrowShimNotImplemented("RtcPeerConnection.AddTrack");
		return nullptr;
	}

	RtcRtpTransceiver^ RtcPeerConnection::AddTransceiver(WebRtcNet::Media::MediaStreamTrack^ track, RtcRtpTransceiverInit^ init)
	{
		ThrowShimNotImplemented("RtcPeerConnection.AddTransceiver(track)");
		return nullptr;
	}

	RtcRtpTransceiver^ RtcPeerConnection::AddTransceiver(WebRtcNet::Media::MediaStreamTrackKind kind, RtcRtpTransceiverInit^ init)
	{
		ThrowShimNotImplemented("RtcPeerConnection.AddTransceiver(kind)");
		return nullptr;
	}

	void RtcPeerConnection::RemoveTrack(RtcRtpSender^ sender)
	{
		ThrowShimNotImplemented("RtcPeerConnection.RemoveTrack");
	}

	RtcDataChannel^ RtcPeerConnection::CreateDataChannel(String^ label, RtcDataChannelInit^ dataChannelInit)
	{
		ThrowShimNotImplemented("RtcPeerConnection.CreateDataChannel");
		return nullptr;
	}
}
