#pragma once

namespace webrtc
{
	class MediaStreamInterface;
}
namespace rtc
{
	template <class T> class scoped_refptr;
}

namespace WebRtcInterop {

using namespace WebRtcNet::Media;

public ref class MediaStream : IMediaStream
{
public:
	/// Composes a new stream from the tracks of an existing stream.
	MediaStream(MediaStream ^ & stream);
	~MediaStream();

	/// Composes a new stream out of existing tracks
	//MediaStream(IEnumerable<IMediaStreamTrack^>^ tracks);

	// Inherited via IMediaStream
	virtual property String^ Id;
	virtual IEnumerable<IMediaStreamTrack^>^ GetAudioTracks();
	virtual IEnumerable<IMediaStreamTrack^>^ GetVideoTracks();
	virtual IEnumerable<IMediaStreamTrack^>^ GetTracks();
	virtual IMediaStreamTrack ^ GetTrackById(String^ trackId);
	virtual void AddTrack(IMediaStreamTrack ^ track);
	virtual void RemoveTrack(IMediaStreamTrack ^ track);
	virtual IMediaStream ^ Clone();
	virtual property Boolean Active;
	virtual event EventHandler^ OnActive;
	virtual event EventHandler^ OnInactive;
	virtual event EventHandler<IMediaStreamTrack^>^ OnAddTrack;
	virtual event EventHandler<IMediaStreamTrack^>^ OnRemoveTrack;

internal:
	MediaStream(rtc::scoped_refptr <webrtc::MediaStreamInterface> stream);
	!MediaStream();
	webrtc::MediaStreamInterface * GetNativeMediaStreamInterface(bool throwOnDisposed);

private:
	rtc::scoped_refptr<webrtc::MediaStreamInterface> * _rpMediaStreamInterface;
};

}

namespace WebRtcNet {

using namespace Media;

public ref class MediaDevices : IMediaDevices
{
public:
	static IMediaStream ^ GetUserMedia(MediaStreamConstraints ^ constraints);
private:
	MediaDevices() {};
};


public ref class MediaStreamException : Exception
{
public:
	MediaStreamException(IMediaStream ^ stream) : _stream(stream) {};
	MediaStreamException(IMediaStream ^ stream, String^ msg) : Exception(msg), _stream(stream) {};
	MediaStreamException(String^ msg) : Exception(msg), _stream(nullptr) {};

	property IMediaStream ^ Stream { IMediaStream ^ get() { return _stream; } };

private:
	IMediaStream ^ _stream;
};

}