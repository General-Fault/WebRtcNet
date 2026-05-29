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

public ref class MediaStream : WebRtcNet::Media::MediaStream
{
public:
	/// Composes a new stream from the tracks of an existing stream.
	MediaStream(WebRtcNet::Media::MediaStream ^ stream);
	~MediaStream();

	/// Composes a new stream out of existing tracks
	//MediaStream(IEnumerable<MediaStreamTrack^>^ tracks);

	// Inherited via MediaStream
	virtual property String^ Id;
	virtual IEnumerable<MediaStreamTrack^>^ GetAudioTracks();
	virtual IEnumerable<MediaStreamTrack^>^ GetVideoTracks();
	virtual IEnumerable<MediaStreamTrack^>^ GetTracks();
	virtual MediaStreamTrack ^ GetTrackById(String^ trackId);
	virtual void AddTrack(MediaStreamTrack ^ track);
	virtual void RemoveTrack(MediaStreamTrack ^ track);
	virtual MediaStream ^ Clone();
	virtual property Boolean Active;
	virtual event EventHandler^ OnActive;
	virtual event EventHandler^ OnInactive;
	virtual event EventHandler<MediaStreamTrack^>^ OnAddTrack;
	virtual event EventHandler<MediaStreamTrack^>^ OnRemoveTrack;

internal:
	MediaStream(rtc::scoped_refptr <webrtc::MediaStreamInterface> stream);
	!MediaStream();
	virtual System::IntPtr GetNativeMediaStreamInterface(bool throwOnDisposed);

private:
	rtc::scoped_refptr<webrtc::MediaStreamInterface> * _rpMediaStreamInterface;
};

}

namespace WebRtcNet {

using namespace Media;

public ref class MediaDevices : WebRtcNet::Media::MediaDevices
{
public:
	static MediaStream ^ GetUserMedia(MediaStreamConstraints ^ constraints);
private:
	MediaDevices() {};
};


public ref class MediaStreamException : Exception
{
public:
	MediaStreamException(MediaStream ^ stream) : _stream(stream) {};
	MediaStreamException(MediaStream ^ stream, String^ msg) : Exception(msg), _stream(stream) {};
	MediaStreamException(String^ msg) : Exception(msg), _stream(nullptr) {};

	property MediaStream ^ Stream { MediaStream ^ get() { return _stream; } };

private:
	MediaStream ^ _stream;
};

}