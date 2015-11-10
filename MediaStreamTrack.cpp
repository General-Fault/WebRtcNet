#include "stdafx.h"
#include "MediaStreamTrack.h"


WEBRTCNET_START
MediaStreamTrack::MediaStreamTrack()
{
}


MediaStreamTrack::~MediaStreamTrack()
{
}

IMediaStreamTrack ^ MediaStreamTrack::Clone()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

void MediaStreamTrack::Stop()
{
	throw gcnew System::NotImplementedException();
}

MediaTrackCapabilities ^ MediaStreamTrack::GetCapabilities()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

MediaTrackConstraints ^ MediaStreamTrack::GetConstraints()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

MediaTrackSettings ^ MediaStreamTrack::GetSettings()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

void MediaStreamTrack::ApplyConstraints(MediaTrackConstraints ^ constraints)
{
	throw gcnew System::NotImplementedException();
}

WEBRTCNET_END