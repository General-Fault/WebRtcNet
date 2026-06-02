#include "stdafx.h"

#include <api/peer_connection_interface.h>


using namespace System;
using namespace Collections::Generic;
using namespace Threading::Tasks;
using namespace Runtime::InteropServices;

using namespace WebRtcNet;

#include "RtcPeerConnection.h"
#include "RtcPeerConnectionFactory.h"
#include "MediaStream.h"
#include "Observers/PeerConnectionObserver.h"
#include "Observers/CreateSessionDescriptionObserver.h"
#include "Marshaling/MarshalPeerConnection.h"
#include "Marshaling/MarshalRtcConfiguration.h"
#include "Marshaling/MarshalMediaConstraints.h"

namespace WebRtcInterop
{
	RtcPeerConnection::RtcPeerConnection(RtcConfiguration^ configuration)
		: observer_(new webrtc_observers::PeerConnectionObserver(this))
		  , configuration_(configuration)
	{
		auto nativePeerConnectionFactory = RtcPeerConnectionFactory::Instance->
			GetNativePeerConnectionFactoryInterface(true);

		auto nativeConfig = marshal_as<webrtc::PeerConnectionInterface::RTCConfiguration>(configuration);
		auto nativePeerConnection = nativePeerConnectionFactory->CreatePeerConnection(
			nativeConfig, nullptr, nullptr, observer_);
		if (nativePeerConnection == nullptr) throw gcnew
			NotSupportedException("Failed to create native PeerConnection");

		rp_peer_connection_ = new rtc::scoped_refptr(nativePeerConnection);
	}

	RtcPeerConnection::~RtcPeerConnection()
	{
		this->!RtcPeerConnection();
	}

	RtcPeerConnection::!RtcPeerConnection()
	{
		delete rp_peer_connection_;
		rp_peer_connection_ = nullptr;


		delete observer_;
		observer_ = nullptr;
	}

	webrtc::PeerConnectionInterface* RtcPeerConnection::GetNativePeerConnection(bool throwOnDisposed)
	{
		if (rp_peer_connection_ == nullptr || rp_peer_connection_->get() == nullptr)
		{
			if (throwOnDisposed) throw gcnew ObjectDisposedException("RtcPeerConnection");
			return nullptr;
		}

		return rp_peer_connection_->get();
	}

	System::IntPtr RtcPeerConnection::GetNativePeerConnectionHandle(bool throwOnDisposed)
	{
		return System::IntPtr(GetNativePeerConnection(throwOnDisposed));
	}

	Task<RtcSessionDescription>^ RtcPeerConnection::CreateOffer(RtcOfferOptions^ options)
	{
		auto pc = GetNativePeerConnection(true);
		auto observer = new rtc::RefCountedObject<webrtc_observers::CreateSessionDescriptionObserver>();
		auto task = observer->CreateSessionTask();

		if (options == nullptr)
		{
			pc->CreateOffer(observer, NULL);
		}
		else
		{
			webrtc::FakeConstraints constraints;
			constraints.AddMandatory<bool>(webrtc::MediaConstraintsInterface::kVoiceActivityDetection,
			                               static_cast<const bool>(options->VoiceActivityDetection));
			constraints.AddMandatory<bool>(webrtc::MediaConstraintsInterface::kIceRestart,
			                               static_cast<const bool>(options->IceRestart));

			pc->CreateOffer(observer, &constraints);
		}

		return task;
	}


	Task<RtcSessionDescription>^ RtcPeerConnection::CreateAnswer()
	{
		auto pc = GetNativePeerConnection(true);
		auto observer = new rtc::RefCountedObject<webrtc_observers::CreateSessionDescriptionObserver>();
		auto task = observer->CreateSessionTask();

		pc->CreateAnswer(observer, NULL);

		return task;
	}

	Task^ RtcPeerConnection::SetLocalDescription(Nullable<RtcLocalSessionDescriptionInit> description)
	{
		// TODO: Implement using two native overloads on PeerConnectionInterface:
		//   - description is null, or description.Value.Type is null:
		//       -> SetLocalDescription(observer)  [native creates offer/answer from signaling state]
		//   - description.Value.Type has a value:
		//       -> SetLocalDescription(unique_ptr<SessionDescriptionInterface>, observer)
		// Add marshal_as<webrtc::SessionDescriptionInterface*>(RtcLocalSessionDescriptionInit) in
		// MarshalPeerConnection.h to support the second path.
		throw gcnew NotImplementedException();
	}

	Task^ RtcPeerConnection::SetRemoteDescription(RtcSessionDescription description)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}


	Task^ RtcPeerConnection::AddIceCandidate(RtcIceCandidate^ candidate)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	RtcConfiguration^ RtcPeerConnection::Configuration::get()
	{
		return configuration_;
	}

	void RtcPeerConnection::Configuration::set(RtcConfiguration^ configuration)
	{
		throw gcnew NotImplementedException();
	}

	IEnumerable<MediaStream^>^ RtcPeerConnection::LocalStreams::get()
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}


	IEnumerable<MediaStream^>^ RtcPeerConnection::RemoteStreams::get()
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	MediaStream^ RtcPeerConnection::GetStreamById(String^ streamId)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	void RtcPeerConnection::AddStream(MediaStream^ stream)
	{
		auto nativePeerConnection = GetNativePeerConnection(true);
		auto nativeStream = reinterpret_cast<webrtc::MediaStreamInterface*>(
			stream->GetNativeMediaStreamInterface(true).ToPointer());
		nativePeerConnection->AddStream(nativeStream);
	}

	void RtcPeerConnection::RemoveStream(MediaStream^ stream)
	{
		auto nativePeerConnection = GetNativePeerConnection(true);
		auto nativeStream = reinterpret_cast<webrtc::MediaStreamInterface*>(
			stream->GetNativeMediaStreamInterface(true).ToPointer());
		nativePeerConnection->RemoveStream(nativeStream);
	}

	void RtcPeerConnection::Close()
	{
		throw gcnew NotImplementedException();
	}

	RtcDataChannel^ RtcPeerConnection::CreateDataChannel(String^ label, RtcDataChannelInit^ dataChannelInit)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	RtcDtmfSender^ RtcPeerConnection::CreateRtcDtmfSender(MediaStreamTrack^ track)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	Task<RtcStatsReport^>^ RtcPeerConnection::GetStats(MediaStreamTrack^ selector)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	void RtcPeerConnection::SetIdentityProvider(String^ provider, String^ protocol, String^ username)
	{
		throw gcnew NotImplementedException();
	}

	void RtcPeerConnection::GetIdentityAssertion()
	{
		throw gcnew NotImplementedException();
	}
}
