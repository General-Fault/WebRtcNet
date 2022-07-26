#include "stdafx.h"

#include "api/peer_connection_interface.h"


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
			//TODO: Once the native PeerConnection proxy implements the CreateOffer that acception options, this conversion will no longer be necessary.
			webrtc::FakeConstraints constraints;
			constraints.AddMandatory<int>(webrtc::MediaConstraintsInterface::kOfferToReceiveAudio,
			                              static_cast<const int>(options->OfferToReceiveAudio));
			constraints.AddMandatory<int>(webrtc::MediaConstraintsInterface::kOfferToReceiveVideo,
			                              static_cast<const int>(options->OfferToReceiveVideo));
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

	Task^ RtcPeerConnection::SetLocalDescription(RtcSessionDescription description)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	Task^ RtcPeerConnection::SetRemoteDescription(RtcSessionDescription description)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}


	Task^ RtcPeerConnection::AddIceCandidate(RtcIceCandidate candidate)
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

	IEnumerable<IMediaStream^>^ RtcPeerConnection::LocalStreams::get()
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}


	IEnumerable<IMediaStream^>^ RtcPeerConnection::RemoteStreams::get()
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	IMediaStream^ RtcPeerConnection::GetStreamById(String^ streamId)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	void RtcPeerConnection::AddStream(IMediaStream^ stream)
	{
		auto nativePeerConnection = GetNativePeerConnection(true);

		//TODO: This should be done with a marshaller. This marshaller would return the nativestream if the managed stream is a WebRtcInterop::MediaStream
		// and would try to create a new native stream wrapper if it is not.
		auto managedStream = dynamic_cast<MediaStream^>(stream);
		if (managedStream == nullptr) throw gcnew ArgumentException("Invalid MediaStream");

		auto nativeStream = managedStream->GetNativeMediaStreamInterface(true);
		nativePeerConnection->AddStream(nativeStream);
	}

	void RtcPeerConnection::RemoveStream(IMediaStream^ stream)
	{
		auto nativePeerConnection = GetNativePeerConnection(true);

		//TODO: This should be done with a marshaller. This marshaller would return the nativestream if the managed stream is a WebRtcInterop::MediaStream
		// and would try to create a new native stream wrapper if it is not.
		auto managedStream = dynamic_cast<MediaStream^>(stream);
		if (managedStream == nullptr) throw gcnew ArgumentException("Invalid MediaStream");

		auto nativeStream = managedStream->GetNativeMediaStreamInterface(true);
		nativePeerConnection->RemoveStream(nativeStream);
	}

	void RtcPeerConnection::Close()
	{
		throw gcnew NotImplementedException();
	}

	IRtcDataChannel^ RtcPeerConnection::CreateDataChannel(String^ label, RtcDataChannelInit^ dataChannelInit)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	IRtcDtmfSender^ RtcPeerConnection::CreateRtcDtmfSender(IMediaStreamTrack^ track)
	{
		throw gcnew NotImplementedException();
		// TODO: insert return statement here
	}

	Task<IRtcStatsReport^>^ RtcPeerConnection::GetStats(IMediaStreamTrack^ selector)
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
