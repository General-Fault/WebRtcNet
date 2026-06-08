#include "pch.h"

#include "RtcPeerConnectionFactory.h"
#include "RtcPeerConnection.h"

#include <api/create_modular_peer_connection_factory.h>
#include <rtc_base/thread.h>
#include <rtc_base/win32_socket_init.h>
#include "Logging/WebRtcLogSink.h"

using namespace System;

namespace WebRtcInterop
{
	namespace
	{
		std::unique_ptr<webrtc::Thread> network_thread_;
		std::unique_ptr<webrtc::Thread> worker_thread_;
		std::unique_ptr<webrtc::Thread> signaling_thread_;
		static WebRtcInterop::Logging::WebRtcLogSink* log_sink_;
	}

	RtcPeerConnectionFactory::RtcPeerConnectionFactory()
	: _rpPeerConnectionFactory(nullptr)
	{
		if (network_thread_ == nullptr)
		{
			network_thread_ = webrtc::Thread::CreateWithSocketServer();
			network_thread_->SetName("WebRtc Network Thread", nullptr);
			network_thread_->Start();
		}

		if (worker_thread_ == nullptr)
		{
			worker_thread_ = webrtc::Thread::Create();
			worker_thread_->SetName("WebRtc Worker Thread", nullptr);
			worker_thread_->Start();
		}

		if (signaling_thread_ == nullptr)
		{
			signaling_thread_ = webrtc::Thread::Create();
			signaling_thread_->SetName("WebRtc Signaling Thread", nullptr);
			signaling_thread_->Start();
		}

		CreateNativePeerConnectionFactory();

		// Register logging sink for WebRTC diagnostics
		log_sink_ = new WebRtcInterop::Logging::WebRtcLogSink();
		rtc::LogMessage::AddLogToStream(log_sink_, rtc::LS_VERBOSE);
	}

	RtcPeerConnectionFactory::~RtcPeerConnectionFactory()
	{
		this->!RtcPeerConnectionFactory();
	}

	RtcPeerConnectionFactory::!RtcPeerConnectionFactory()
	{
		// Unregister logging sink
		if (log_sink_ != nullptr)
		{
			rtc::LogMessage::RemoveLogToStream(log_sink_);
			delete log_sink_;
			log_sink_ = nullptr;
		}

		DestroyNativePeerConnectionFactory();
	}

	WebRtcNet::RtcPeerConnection^ RtcPeerConnectionFactory::CreatePeerConnection(RtcConfiguration^ configuration)
	{
		if (configuration == nullptr)
			throw gcnew ArgumentNullException("configuration");
		return gcnew RtcPeerConnection(configuration);
	}

	void RtcPeerConnectionFactory::CreateNativePeerConnectionFactory()
	{
		webrtc::PeerConnectionFactoryDependencies dependencies;
		dependencies.network_thread = network_thread_.get();
		dependencies.worker_thread = worker_thread_.get();
		dependencies.signaling_thread = signaling_thread_.get();

		auto nativeFactory = webrtc::CreateModularPeerConnectionFactory(std::move(dependencies));
		if (nativeFactory == nullptr)
			throw gcnew NotSupportedException("Failed to create native PeerConnectionFactory");

		_rpPeerConnectionFactory =
			new webrtc::scoped_refptr<webrtc::PeerConnectionFactoryInterface>(nativeFactory);
	}

	void RtcPeerConnectionFactory::DestroyNativePeerConnectionFactory()
	{
		delete _rpPeerConnectionFactory;
		_rpPeerConnectionFactory = nullptr;
	}

	webrtc::PeerConnectionFactoryInterface* RtcPeerConnectionFactory::GetNativePeerConnectionFactoryInterface(bool throwOnDisposed)
	{
		if (_rpPeerConnectionFactory == nullptr || _rpPeerConnectionFactory->get() == nullptr)
		{
			if (throwOnDisposed)
				throw gcnew ObjectDisposedException("RtcPeerConnectionFactory");
			return nullptr;
		}

		return _rpPeerConnectionFactory->get();
	}

	RtcPeerConnectionFactory^ RtcPeerConnectionFactory::Instance::get()
	{
		if (_instance == nullptr)
			InitializeInstance();
		return _instance;
	}

	void RtcPeerConnectionFactory::InitializeInstance()
	{
		if (_instance != nullptr)
			return;
		_instance = gcnew RtcPeerConnectionFactory();
	}

	void RtcPeerConnectionFactory::DestroyInstance()
	{
		delete _instance;
		_instance = nullptr;

		if (signaling_thread_ != nullptr)
			signaling_thread_->Stop();
		if (worker_thread_ != nullptr)
			worker_thread_->Stop();
		if (network_thread_ != nullptr)
			network_thread_->Stop();

		signaling_thread_.reset();
		worker_thread_.reset();
		network_thread_.reset();
	}
}

