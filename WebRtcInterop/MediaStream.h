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

public ref class MediaStream : WebRtcNet::IMediaStream
{
public:
	/// Composes a new stream from the tracks of an existing stream.
	MediaStream(MediaStream ^ & stream);
	~MediaStream();

	/// Composes a new stream out of existing tracks
	//MediaStream(IEnumerable<IMediaStreamTrack^>^ tracks);

	// Inherited via IMediaStream
	virtual property System::String ^ Id;
	virtual IEnumerable<WebRtcNet::IMediaStreamTrack^>^ GetAudioTracks();
	virtual IEnumerable<WebRtcNet::IMediaStreamTrack^>^ GetVideoTracks();
	virtual IEnumerable<WebRtcNet::IMediaStreamTrack^>^ GetTracks();
	virtual WebRtcNet::IMediaStreamTrack ^ GetTrackById(System::String ^ trackId);
	virtual void AddTrack(WebRtcNet::IMediaStreamTrack ^ track);
	virtual void RemoveTrack(WebRtcNet::IMediaStreamTrack ^ track);
	virtual WebRtcNet::IMediaStream ^ Clone();
	virtual property System::Boolean Active;
	virtual event System::EventHandler ^ OnActive;
	virtual event System::EventHandler ^ OnInactive;
	virtual event System::EventHandler<WebRtcNet::IMediaStreamTrack^>^ OnAddTrack;
	virtual event System::EventHandler<WebRtcNet::IMediaStreamTrack^>^ OnRemoveTrack;

internal:
	MediaStream(webrtc::MediaStreamInterface * stream);
	!MediaStream();
	webrtc::MediaStreamInterface * GetNativeMediaStreamInterface(bool throwOnDisposed);

private:
	rtc::scoped_refptr<webrtc::MediaStreamInterface> * _rpMediaStreamInterface;
};

}

namespace WebRtcNet {

public ref class Media
{
public:
	static IMediaStream ^ GetUserMedia(MediaConstraints ^ constraints);
private:
	Media() {};
};


public ref class MediaStreamException : System::Exception 
{
public:
	MediaStreamException(IMediaStream ^ stream) : _stream(stream) {};
	MediaStreamException(IMediaStream ^ stream, System::String ^ msg) : System::Exception(msg), _stream(stream) {};
	MediaStreamException(System::String ^ msg) : System::Exception(msg), _stream(nullptr) {};

	property IMediaStream ^ Stream { IMediaStream ^ get() { return _stream; } };

private:
	IMediaStream ^ _stream;
};

}