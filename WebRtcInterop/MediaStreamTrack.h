#pragma once

namespace webrtc
{
	class MediaStreamTrackInterface;
}
namespace rtc
{
	template <class T> class scoped_refptr;
}

namespace WebRtcInterop {

using namespace WebRtcNet::Media;

public ref class MediaStreamTrack : WebRtcNet::Media::MediaStreamTrack
{
public:
	MediaStreamTrack();
	MediaStreamTrack(rtc::scoped_refptr<webrtc::MediaStreamTrackInterface> track);
	virtual ~MediaStreamTrack();
	!MediaStreamTrack();

	// Inherited via MediaStreamTrack
	virtual property MediaStreamTrackKind Kind;
	virtual property System::String ^ Id;
	virtual property System::String ^ Label;
	virtual property bool Enabled;
	virtual property bool Muted;
	virtual property bool ReadOnly;
	virtual property bool Remote;
	virtual property MediaStreamTrackState ReadyState;
	virtual event System::EventHandler ^ OnMute;
	virtual event System::EventHandler ^ OnUnMute;
	virtual event System::EventHandler<MediaStreamError ^> ^ OnEnded;
	virtual MediaStreamTrack ^ Clone();
	virtual void Stop();
	virtual MediaTrackCapabilities GetCapabilities();
	virtual MediaTrackConstraints ^ GetConstraints();
	virtual MediaTrackSettings GetSettings();
	virtual void ApplyConstraints(MediaTrackConstraints ^constraints);

internal:
	virtual System::IntPtr GetNativeMediaStreamTrackInterface(bool throwOnDisposed);

private:
	rtc::scoped_refptr<webrtc::MediaStreamTrackInterface> * _rpMediaStreamTrackInterface;
};


}