#pragma once

#include "IMediaStream.h"

WEBRTCNET_START

public ref class MediaStreamContraints
{
public:
	property String^ PeerIdentity 
	{ 
		String ^ get() { throw gcnew NotImplementedException(); }
		void set(String ^ value) { throw gcnew NotImplementedException(); }
	}
};

public ref class MediaStream : IMediaStream
{
public:
	/// Composes a new stream from the tracks of an existing stream.
	//MediaStream(MediaStream ^ stream);

	/// Composes a new stream out of existing tracks
	//MediaStream(IEnumerable<IMediaStreamTrack ^> ^ tracks);


	static IMediaStream^ GetUserMedia();

	// Inherited via IMediaStream
	virtual property String ^ Id;
	virtual IEnumerable<IMediaStreamTrack ^> ^ GetAudioTracks();
	virtual IEnumerable<IMediaStreamTrack ^> ^ GetVideoTracks();
	virtual IEnumerable<IMediaStreamTrack ^> ^ GetTracks();
	virtual IMediaStreamTrack ^ GetTrackById(String ^ trackId);
	virtual void AddTrack(IMediaStreamTrack ^ track);
	virtual void RemoveTrack(IMediaStreamTrack ^ track);
	virtual IMediaStream ^ Clone();
	virtual property Boolean Active;
	virtual event EventHandler ^ OnActive;
	virtual event EventHandler ^ OnInactive;
	virtual event EventHandler<IMediaStreamTrack ^> ^ OnAddTrack;
	virtual event EventHandler<IMediaStreamTrack ^> ^ OnRemoveTrack;
};

WEBRTCNET_END