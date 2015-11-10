#include "stdafx.h"
#include "MediaStream.h"


WEBRTCNET_START

IMediaStream ^ MediaStream::GetUserMedia()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

//MediaStream::MediaStream(MediaStream ^ stream)
//{
//	throw gcnew System::NotImplementedException();
//}
//
//MediaStream::MediaStream(IEnumerable<IMediaStreamTrack ^> ^ tracks)
//{
//	throw gcnew System::NotImplementedException();
//}

IEnumerable<IMediaStreamTrack ^> ^ MediaStream::GetAudioTracks()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack ^> ^ MediaStream::GetVideoTracks()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack ^> ^ MediaStream::GetTracks()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

IMediaStreamTrack ^ MediaStream::GetTrackById(String ^ trackId)
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

void MediaStream::AddTrack(IMediaStreamTrack ^ track)
{
	throw gcnew System::NotImplementedException();
}

void MediaStream::RemoveTrack(IMediaStreamTrack ^ track)
{
	throw gcnew System::NotImplementedException();
}

IMediaStream ^ MediaStream::Clone()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

WEBRTCNET_END