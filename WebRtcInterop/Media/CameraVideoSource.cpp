#include "pch.h"

#include "CameraVideoSource.h"

#include <api/make_ref_counted.h>
#include <modules/video_capture/video_capture_defines.h>
#include <modules/video_capture/video_capture_factory.h>

namespace webrtc
{
	VideoCaptureCapability SelectCapability(const std::string& deviceId)
	{
		VideoCaptureCapability requested;
		requested.width = 1280;
		requested.height = 720;
		requested.maxFPS = 30;

		VideoCaptureCapability selected = requested;
		std::unique_ptr<VideoCaptureModule::DeviceInfo> deviceInfo(
			VideoCaptureFactory::CreateDeviceInfo());
		if (!deviceInfo)
			return selected;

		VideoCaptureCapability matched;
		if (deviceInfo->GetBestMatchedCapability(deviceId.c_str(), requested, matched) >= 0)
			selected = matched;

		return selected;
	}
}

namespace WebRtcInterop
{
	CameraVideoSource::CameraVideoSource(const webrtc::scoped_refptr<webrtc::VideoCaptureModule>& captureModule)
		: state_(static_cast<int>(kInitializing)),
		  stopping_(false),
		  capture_started_(false),
		  capture_module_(captureModule)
	{
	}

	CameraVideoSource::~CameraVideoSource()
	{
		Stop();
	}

	webrtc::scoped_refptr<CameraVideoSource> CameraVideoSource::Create(const std::string& deviceId)
	{
		if (deviceId.empty())
			return nullptr;

		auto captureModule = webrtc::VideoCaptureFactory::Create(deviceId.c_str());
		if (!captureModule)
			return nullptr;

		auto source = webrtc::make_ref_counted<CameraVideoSource>(captureModule);
		if (!source->Start(deviceId))
			return nullptr;

		return source;
	}

	webrtc::MediaSourceInterface::SourceState CameraVideoSource::state() const
	{
		return static_cast<SourceState>(state_.load());
	}

	void CameraVideoSource::AddOrUpdateSink(VideoSinkInterface<webrtc::VideoFrame>* sink,
		const webrtc::VideoSinkWants& wants)
	{
		broadcaster_.AddOrUpdateSink(sink, wants);
	}

	void CameraVideoSource::RemoveSink(VideoSinkInterface<webrtc::VideoFrame>* sink)
	{
		broadcaster_.RemoveSink(sink);
	}

	void CameraVideoSource::OnFrame(const webrtc::VideoFrame& frame)
	{
		if (stopping_.load())
			return;

		broadcaster_.OnFrame(frame);
	}

	bool CameraVideoSource::Start(const std::string& deviceId)
	{
		if (!capture_module_)
			return false;

		const auto capability = webrtc::SelectCapability(deviceId);
		capture_module_->RegisterCaptureDataCallback(this);
		if (capture_module_->StartCapture(capability) != 0)
		{
			capture_module_->DeRegisterCaptureDataCallback();
			state_.store(kEnded);
			return false;
		}

		capture_started_.store(true);
		state_.store(kLive);
		return true;
	}

	void CameraVideoSource::Stop()
	{
		if (stopping_.exchange(true))
			return;

		if (capture_module_)
		{
			if (capture_started_.exchange(false))
				capture_module_->StopCapture();

			capture_module_->DeRegisterCaptureDataCallback();
			capture_module_ = nullptr;
		}

		state_.store(kEnded);
	}
}
