#pragma once

#include <api/scoped_refptr.h>

namespace webrtc
{
	class MediaStreamTrackInterface;
}

namespace WebRtcInterop::Media
{

using namespace WebRtcNet::Media;

public ref class MediaStreamTrack : WebRtcNet::Media::MediaStreamTrack
{
public:
	MediaStreamTrack();
	MediaStreamTrack(webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface> track);
	virtual ~MediaStreamTrack();
	!MediaStreamTrack();

	virtual property MediaStreamTrackKind Kind { MediaStreamTrackKind get() override; }
	virtual property System::String^ Id { System::String^ get() override; }
	virtual property System::String^ Label { System::String^ get() override; }
	virtual property bool Enabled { bool get() override; void set(bool value) override; }
	virtual property bool Muted { bool get() override; }
	virtual property MediaStreamTrackState ReadyState { MediaStreamTrackState get() override; }

	virtual event System::EventHandler^ OnMute
	{
		void add(System::EventHandler^ value) override { on_mute_ += value; }
		void remove(System::EventHandler^ value) override { on_mute_ -= value; }
	}

	virtual event System::EventHandler^ OnUnMute
	{
		void add(System::EventHandler^ value) override { on_unmute_ += value; }
		void remove(System::EventHandler^ value) override { on_unmute_ -= value; }
	}

	virtual event System::EventHandler^ OnEnded
	{
		void add(System::EventHandler^ value) override { on_ended_ += value; }
		void remove(System::EventHandler^ value) override { on_ended_ -= value; }
	}

	WebRtcNet::Media::MediaStreamTrack^ Clone() override;
	void Stop() override;
	MediaTrackCapabilities^ GetCapabilities() override;
	MediaTrackConstraints^ GetConstraints() override;
	MediaTrackSettings^ GetSettings() override;
	void ApplyConstraints(MediaTrackConstraints^ constraints) override;

public:
	virtual System::IntPtr GetNativeMediaStreamTrackInterface(bool throwOnDisposed) override;

private:
	webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface>* _rpMediaStreamTrackInterface;
	MediaTrackConstraints^ applied_constraints_;
	System::EventHandler^ on_mute_;
	System::EventHandler^ on_unmute_;
	System::EventHandler^ on_ended_;
};


}