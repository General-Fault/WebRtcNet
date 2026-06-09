#pragma once

#include <api/scoped_refptr.h>
#include "../NativeWrapper.h"

namespace webrtc
{
	class MediaStreamTrackInterface;
}

namespace WebRtcInterop::Media
{
	using namespace System;
	using namespace WebRtcNet::Media;

	public ref class MediaStreamTrack : WebRtcNet::Media::MediaStreamTrack
	{
	public:
		MediaStreamTrack();
		MediaStreamTrack(webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface> track);
		virtual ~MediaStreamTrack();
		!MediaStreamTrack();

		virtual property MediaStreamTrackKind Kind { MediaStreamTrackKind get() override; }
		virtual property String^ Id { String^ get() override; }
		virtual property String^ Label { String^ get() override; }
		virtual property bool Enabled { bool get() override; void set(bool value) override; }
		virtual property bool Muted { bool get() override; }
		virtual property MediaStreamTrackState ReadyState { MediaStreamTrackState get() override; }

		virtual event EventHandler^ OnMute
		{
			void add(EventHandler^ value) override { on_mute_ += value; }
			void remove(EventHandler^ value) override { on_mute_ -= value; }
		}

		virtual event EventHandler^ OnUnMute
		{
			void add(EventHandler^ value) override { on_unmute_ += value; }
			void remove(EventHandler^ value) override { on_unmute_ -= value; }
		}

		virtual event EventHandler^ OnEnded
		{
			void add(EventHandler^ value) override { on_ended_ += value; }
			void remove(EventHandler^ value) override { on_ended_ -= value; }
		}

	public:
		WebRtcNet::Media::MediaStreamTrack^ Clone() override;
		void Stop() override;
		MediaTrackCapabilities^ GetCapabilities() override;
		MediaTrackConstraints^ GetConstraints() override;
		MediaTrackSettings^ GetSettings() override;
		void ApplyConstraints(MediaTrackConstraints^ constraints) override;

	internal:
		webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface> GetNativeMediaStreamTrackInterface(bool throwOnDisposed);

	private:
		NativeWrapper<webrtc::MediaStreamTrackInterface>^ _nativeMediaStreamTrackInterface;
		MediaTrackConstraints^ applied_constraints_;
		EventHandler^ on_mute_;
		EventHandler^ on_unmute_;
		EventHandler^ on_ended_;
	};


}