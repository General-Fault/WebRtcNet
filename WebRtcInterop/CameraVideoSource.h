#pragma once

#include <atomic>
#include <optional>
#include <string>

#include <api/media_stream_interface.h>
#include <api/notifier.h>
#include <api/video/video_frame.h>
#include <api/video/video_sink_interface.h>
#include <media/base/video_broadcaster.h>
#include <modules/video_capture/video_capture.h>

namespace WebRtcInterop
{
	class CameraVideoSource : public webrtc::Notifier<webrtc::VideoTrackSourceInterface>,
							  public webrtc::VideoSinkInterface<webrtc::VideoFrame>
	{
	public:
		static webrtc::scoped_refptr<CameraVideoSource> Create(const std::string& deviceId);
		explicit CameraVideoSource(const webrtc::scoped_refptr<webrtc::VideoCaptureModule>& captureModule);
		~CameraVideoSource() override;

		SourceState state() const override;
		bool remote() const override { return false; }
		bool is_screencast() const override { return false; }
		std::optional<bool> needs_denoising() const override { return std::nullopt; }
		bool GetStats(Stats* /* stats */) override { return false; }

		void AddOrUpdateSink(VideoSinkInterface* sink,
			const webrtc::VideoSinkWants& wants) override;
		void RemoveSink(VideoSinkInterface* sink) override;

		bool SupportsEncodedOutput() const override { return false; }
		void GenerateKeyFrame() override {}
		void AddEncodedSink(
			VideoSinkInterface<webrtc::RecordableEncodedFrame>* /* sink */) override {}
		void RemoveEncodedSink(
			VideoSinkInterface<webrtc::RecordableEncodedFrame>* /* sink */) override {}

		void OnFrame(const webrtc::VideoFrame& frame) override;

	private:
		bool Start(const std::string& deviceId);
		void Stop();

		std::atomic<int> state_;
		std::atomic<bool> stopping_;
		std::atomic<bool> capture_started_;
		webrtc::scoped_refptr<webrtc::VideoCaptureModule> capture_module_;
		webrtc::VideoBroadcaster broadcaster_;
	};
}
