#include "stdafx.h"

#include "webrtc\base\scoped_ref_ptr.h"
#include "talk\app\webrtc\mediastreaminterface.h"

using namespace System;
using namespace System::Collections::Generic;

#include "MediaStream.h"

using namespace WebRtcNet;

namespace WebRtcInterop {

IMediaStream ^ MediaStream::GetUserMedia()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

MediaStream::MediaStream(MediaStream ^ & stream)
	: _rpMediaStreamInterface(nullptr)
{
	auto nativeStream = stream->GetNativeMediaStreamInterface(true);
	_rpMediaStreamInterface = new rtc::scoped_refptr<webrtc::MediaStreamInterface>(nativeStream);
}

MediaStream::MediaStream(webrtc::MediaStreamInterface * stream)
	: _rpMediaStreamInterface(new rtc::scoped_refptr<webrtc::MediaStreamInterface>(stream))
{
}

MediaStream::~MediaStream()
{
	this->!MediaStream();
}

MediaStream::!MediaStream()
{
	delete _rpMediaStreamInterface;
	_rpMediaStreamInterface = nullptr;
}

webrtc::MediaStreamInterface* MediaStream::GetNativeMediaStreamInterface(bool throwOnDisposed)
{
	if (_rpMediaStreamInterface == nullptr || _rpMediaStreamInterface->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("MediaStream");
		return nullptr;
	}

	return _rpMediaStreamInterface->get();
}


IEnumerable<IMediaStreamTrack^>^ MediaStream::GetAudioTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack^>^ MediaStream::GetVideoTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack^>^ MediaStream::GetTracks()
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

}