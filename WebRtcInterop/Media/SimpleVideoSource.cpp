#include "pch.h"
#include "SimpleVideoSource.h"

namespace WebRtcInterop
{
	SimpleVideoSource::SimpleVideoSource(bool remote)
		: remote_(remote), state_(webrtc::MediaSourceInterface::kLive)
	{
	}

	webrtc::MediaSourceInterface::SourceState SimpleVideoSource::state() const
	{
		return state_;
	}

	void SimpleVideoSource::AddOrUpdateSink(webrtc::VideoSinkInterface<webrtc::VideoFrame>* sink,
		const webrtc::VideoSinkWants& wants)
	{
		// Stub: no-op for MVP. Can be extended to route frames to sink if capturing is implemented.
	}

	void SimpleVideoSource::RemoveSink(webrtc::VideoSinkInterface<webrtc::VideoFrame>* sink)
	{
		// Stub: no-op for MVP.
	}
}
