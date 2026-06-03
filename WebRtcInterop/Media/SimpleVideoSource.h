#pragma once

#include <api/media_stream_interface.h>
#include <api/notifier.h>

namespace WebRtcInterop
{
	// Minimal VideoTrackSourceInterface implementation for GetUserMedia
	// This is a stub that satisfies the interface requirements without actual video capture.
	// Real capture (DirectShow, WinRT) can be implemented later.
	class SimpleVideoSource : public webrtc::Notifier<webrtc::VideoTrackSourceInterface>
	{
	public:
		explicit SimpleVideoSource(bool remote = false);
		~SimpleVideoSource() override = default;

		// MediaSourceInterface implementation
		webrtc::MediaSourceInterface::SourceState state() const override;
		bool remote() const override { return remote_; }

		// VideoTrackSourceInterface implementation
		bool is_screencast() const override { return false; }
		std::optional<bool> needs_denoising() const override { return std::nullopt; }
		bool GetStats(Stats* /* stats */) override { return false; }

		void AddOrUpdateSink(webrtc::VideoSinkInterface<webrtc::VideoFrame>* sink,
			const webrtc::VideoSinkWants& wants) override;
		void RemoveSink(webrtc::VideoSinkInterface<webrtc::VideoFrame>* sink) override;

		bool SupportsEncodedOutput() const override { return false; }
		void GenerateKeyFrame() override {}
		void AddEncodedSink(
			webrtc::VideoSinkInterface<webrtc::RecordableEncodedFrame>* /* sink */) override {}
		void RemoveEncodedSink(
			webrtc::VideoSinkInterface<webrtc::RecordableEncodedFrame>* /* sink */) override {}

	private:
		bool remote_;
		webrtc::MediaSourceInterface::SourceState state_ = webrtc::MediaSourceInterface::kLive;
	};
}
