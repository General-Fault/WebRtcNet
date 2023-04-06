#include "pch.h"

#include "MediaStreamTrack.h"

#include "api/media_stream_track.h"

using namespace System;
using namespace WebRtcNet;
using namespace WebRtcNet::Media;

namespace WebRtcInterop {
MediaStreamTrack::MediaStreamTrack()
{
}


MediaStreamTrack::~MediaStreamTrack()
{
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

MediaConstraints ^ MediaStreamTrack::GetConstraints()
{
	throw gcnew NotImplementedException();
}

MediaTrackSettings MediaStreamTrack::GetSettings()
{
	return MediaTrackSettings();
}

void MediaStreamTrack::ApplyConstraints(MediaConstraints ^constraints)
{
	throw gcnew NotImplementedException();
}


}