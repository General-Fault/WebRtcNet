#pragma once

#include "IMediaStreamTrack.h"

WEBRTCNET_START

public ref class MediaStreamTrack : IMediaStreamTrack
{
public:
	MediaStreamTrack();
	virtual ~MediaStreamTrack();

	// Inherited via IMediaStreamTrack
	virtual property String ^ Kind;
	virtual property String ^ Id;
	virtual property String ^ Label;
	virtual property Boolean Enabled;
	virtual property Boolean Muted;
	virtual event EventHandler ^ OnMute;
	virtual event EventHandler ^ OnUnMute;
	virtual property Boolean ReadOnly;
	virtual property Boolean Remote;
	virtual property MediaStreamTrackState ReadyState;
	virtual event EventHandler<MediaStreamError ^> ^ OnEnded;
	virtual IMediaStreamTrack ^ Clone();
	virtual void Stop();
	virtual MediaTrackCapabilities ^ GetCapabilities();
	virtual MediaTrackConstraints ^ GetConstraints();
	virtual MediaTrackSettings ^ GetSettings();
	virtual void ApplyConstraints(MediaTrackConstraints ^ constraints);
	virtual event EventHandler<MediaStreamError ^> ^ OnOverConstrained;
};


WEBRTCNET_END