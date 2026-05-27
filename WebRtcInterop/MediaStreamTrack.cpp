#include "pch.h"

#include "MediaStreamTrack.h"

#include "api/media_stream_track.h"

using namespace System;
using namespace WebRtcNet;
using namespace WebRtcNet::Media;

namespace WebRtcInterop {
MediaStreamTrack::MediaStreamTrack()
{
	_rpMediaStreamTrackInterface = nullptr;
}

MediaStreamTrack::MediaStreamTrack(rtc::scoped_refptr<webrtc::MediaStreamTrackInterface> track)
{
	_rpMediaStreamTrackInterface = new rtc::scoped_refptr<webrtc::MediaStreamTrackInterface>(track);
}


MediaStreamTrack::~MediaStreamTrack()
{
	this->!MediaStreamTrack();
}

MediaStreamTrack::!MediaStreamTrack()
{
	delete _rpMediaStreamTrackInterface;
	_rpMediaStreamTrackInterface = nullptr;
}

IntPtr MediaStreamTrack::GetNativeMediaStreamTrackInterface(bool throwOnDisposed)
{
	if (_rpMediaStreamTrackInterface == nullptr || _rpMediaStreamTrackInterface->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("MediaStreamTrack");
		return IntPtr::Zero;
	}

	return IntPtr(_rpMediaStreamTrackInterface->get());
}

IMediaStreamTrack ^ MediaStreamTrack::Clone()
{
	throw gcnew NotImplementedException();
}

void MediaStreamTrack::Stop()
{
	throw gcnew NotImplementedException();
}

MediaTrackCapabilities MediaStreamTrack::GetCapabilities()
{
	return MediaTrackCapabilities();
}

MediaTrackConstraints ^ MediaStreamTrack::GetConstraints()
{
	throw gcnew NotImplementedException();
}

MediaTrackSettings MediaStreamTrack::GetSettings()
{
	return MediaTrackSettings();
}

void MediaStreamTrack::ApplyConstraints(MediaTrackConstraints ^constraints)
{
	throw gcnew NotImplementedException();
}


}