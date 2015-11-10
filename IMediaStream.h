#pragma once

#include "IMediaStreamTrack.h"

WEBRTCNET_START

public interface class IMediaStream
{
	/// The Id that the stream was initialized with.
	property String ^ Id { String ^ get(); }

	/// Returns a sequence of IMediaStreamTrack objects representing the audio tracks in this stream.
	IEnumerable<IMediaStreamTrack ^> ^ GetAudioTracks();

	/// Returns a sequence of MediaStreamTrack objects representing the video tracks in this stream.
	IEnumerable<IMediaStreamTrack ^> ^ GetVideoTracks();

	/// Returns a sequence of IMediaStreamTrack objects representing all the tracks in this stream.
	IEnumerable<IMediaStreamTrack ^> ^ GetTracks();

	/// Returns either an IMediaStreamTrack object from this stream's track set whose id is 
	/// equal to trackId, or null, if no such track exists.
	IMediaStreamTrack^ GetTrackById(String ^ trackId);

	/// Adds the given MediaStreamTrack to this MediaStream.
	void AddTrack(IMediaStreamTrack ^ track);

	/// Returns a sequence of MediaStreamTrack objects representing the audio tracks in this stream.
	void RemoveTrack(IMediaStreamTrack ^ track);

	/// Clones the given MediaStream and all its tracks.
	IMediaStream ^ Clone();

	/// The Returns true if this MediaStream is active and false otherwise.
	property Boolean Active { Boolean get(); }

	/// The MediaStream became active.
	event EventHandler ^ OnActive;

	/// The MediaStream became inactive.
	event EventHandler ^ OnInactive;

	/// A new MediaStreamTrack has been added to this stream. 
	///Note that this event is not fired when the application directly modifies the tracks of a MediaStream.
	event EventHandler<IMediaStreamTrack ^> ^ OnAddTrack;

	/// A MediaStreamTrack has been removed from this stream.
	/// Note that this event is not fired when the script directly modifies the tracks of a MediaStream.
	event EventHandler<IMediaStreamTrack ^> ^ OnRemoveTrack;

};

WEBRTCNET_END
