#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

#include "MediaStream.h"

WEBRTCNET_START

IMediaStream ^ MediaStream::GetUserMedia()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

//MediaStream::MediaStream(MediaStream ^ stream)
//{
//	throw gcnew NotImplementedException();
//}
//
//MediaStream::MediaStream(IEnumerable<IMediaStreamTrack ^> ^ tracks)
//{
//	throw gcnew NotImplementedException();
//}

IEnumerable<IMediaStreamTrack ^> ^ MediaStream::GetAudioTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack ^> ^ MediaStream::GetVideoTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack ^> ^ MediaStream::GetTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IMediaStreamTrack ^ MediaStream::GetTrackById(String ^ trackId)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void MediaStream::AddTrack(IMediaStreamTrack ^ track)
{
	throw gcnew NotImplementedException();
}

void MediaStream::RemoveTrack(IMediaStreamTrack ^ track)
{
	throw gcnew NotImplementedException();
}

IMediaStream ^ MediaStream::Clone()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

WEBRTCNET_END