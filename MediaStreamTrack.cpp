#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

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
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void MediaStreamTrack::Stop()
{
	throw gcnew NotImplementedException();
}

MediaTrackCapabilities ^ MediaStreamTrack::GetCapabilities()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

MediaTrackConstraints ^ MediaStreamTrack::GetConstraints()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

MediaTrackSettings ^ MediaStreamTrack::GetSettings()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void MediaStreamTrack::ApplyConstraints(MediaTrackConstraints ^ constraints)
{
	throw gcnew NotImplementedException();
}

WEBRTCNET_END