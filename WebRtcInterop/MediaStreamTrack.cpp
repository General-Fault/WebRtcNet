#include "stdafx.h"

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
	// TODO: insert return statement here
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
	// TODO: insert return statement here
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