#pragma once

namespace WebRtcInterop {

public ref class MediaStreamTrack : WebRtcNet::IMediaStreamTrack
{
public:
	MediaStreamTrack();
	virtual ~MediaStreamTrack();

	// Inherited via IMediaStreamTrack
	virtual property System::String ^ Kind;
	virtual property System::String ^ Id;
	virtual property System::String ^ Label;
	virtual property bool Enabled;
	virtual property bool Muted;
	virtual property bool ReadOnly;
	virtual property bool Remote;
	virtual property WebRtcNet::MediaStreamTrackState ReadyState;
	virtual event System::EventHandler ^ OnMute;
	virtual event System::EventHandler ^ OnUnMute;
	virtual event System::EventHandler<WebRtcNet::MediaStreamError ^> ^ OnEnded;
	virtual event System::EventHandler<WebRtcNet::MediaStreamError ^> ^ OnOverConstrained;
	virtual WebRtcNet::IMediaStreamTrack ^ Clone();
	virtual void Stop();
	virtual WebRtcNet::MediaTrackCapabilities GetCapabilities();
	virtual WebRtcNet::MediaConstraints ^ GetConstraints();
	virtual WebRtcNet::MediaTrackSettings GetSettings();
	virtual void ApplyConstraints(WebRtcNet::MediaConstraints ^constraints);
};


}