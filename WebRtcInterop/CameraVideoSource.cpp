#include "pch.h"

#include "CameraVideoSource.h"

#include <api/make_ref_counted.h>
#include <modules/video_capture/video_capture_defines.h>
#include <modules/video_capture/video_capture_factory.h>
#include <vector>

namespace webrtc
{
	VideoCaptureCapability CreateCapability(const int32_t width, const int32_t height, const int32_t fps)
	{
		VideoCaptureCapability capability;
		capability.width = width;
		capability.height = height;
		capability.maxFPS = fps;
		return capability;
	}

	bool AreCapabilitiesEqual(const VideoCaptureCapability& left, const VideoCaptureCapability& right)
	{
		return left.width == right.width &&
			   left.height == right.height &&
			   left.maxFPS == right.maxFPS &&
			   left.videoType == right.videoType &&
			   left.interlaced == right.interlaced;
	}

	void AddCapabilityIfMissing(std::vector<VideoCaptureCapability>& capabilities, const VideoCaptureCapability& candidate)
	{
		for (const auto& existing : capabilities)
		{
			if (AreCapabilitiesEqual(existing, candidate))
				return;
		}

		capabilities.push_back(candidate);
	}

	std::vector<VideoCaptureCapability> BuildCaptureCapabilityCandidates(const std::string& deviceId)
	{
		std::vector<VideoCaptureCapability> candidates;
		std::vector<VideoCaptureCapability> requested = {
			CreateCapability(1280, 720, 30),
			CreateCapability(1280, 720, 15),
			CreateCapability(960, 540, 30),
			CreateCapability(640, 480, 30),
			CreateCapability(640, 480, 15),
			CreateCapability(320, 240, 30)
		};

		const std::unique_ptr<VideoCaptureModule::DeviceInfo> deviceInfo(
			VideoCaptureFactory::CreateDeviceInfo());
		if (!deviceInfo)
			return requested;

		for (const auto& capability : requested)
		{
			VideoCaptureCapability matched;
			if (deviceInfo->GetBestMatchedCapability(deviceId.c_str(), capability, matched) >= 0)
				AddCapabilityIfMissing(candidates, matched);
		}

		for (const auto& capability : requested)
			AddCapabilityIfMissing(candidates, capability);

		return candidates;
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

		capture_module_->RegisterCaptureDataCallback(this);
		const auto capabilities = webrtc::BuildCaptureCapabilityCandidates(deviceId);
		for (const auto& capability : capabilities)
		{
			if (capture_module_->StartCapture(capability) == 0)
			{
				capture_started_.store(true);
				state_.store(kLive);
				return true;
			}
		}

		capture_module_->DeRegisterCaptureDataCallback();
		state_.store(kEnded);
		return false;
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
