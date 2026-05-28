using System;
using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// Represents a media stream containing at least one audio or video track.
/// Disposable to allow management of native resources.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#mediastream"/>
public abstract class IMediaStream : IDisposable
{
	protected IMediaStream()
	{
	}

	/// <summary>
	/// The Id that the stream was initialized with.
	/// </summary>
	public abstract string Id { get; }

	/// <summary>
	/// Returns the native media stream interface used by WebRtcInterop.
	/// </summary>
	/// <param name="throwOnDisposed">True to throw when the stream has already been disposed.</param>
	internal abstract IntPtr GetNativeMediaStreamInterface(bool throwOnDisposed);

	/// <summary>
	/// Returns a sequence of IMediaStreamTrack objects representing the audio tracks in this stream.
	/// </summary>
	public abstract IEnumerable<IMediaStreamTrack> GetAudioTracks();

	/// <summary>
	/// Returns a sequence of MediaStreamTrack objects representing the video tracks in this stream.
	/// </summary>
	public abstract IEnumerable<IMediaStreamTrack> GetVideoTracks();

	/// <summary>
	/// Returns a sequence of IMediaStreamTrack objects representing all the tracks in this stream.
	/// </summary>
	public abstract IEnumerable<IMediaStreamTrack> GetTracks();

	/// <summary>
	/// Returns either an IMediaStreamTrack object from this stream's track set whose id is 
	/// equal to trackId, or null, if no such track exists.
	/// </summary>
	/// <param name="trackId">A track identifier.</param>
	public abstract IMediaStreamTrack GetTrackById(string trackId);

	/// <summary>
	/// Adds the given MediaStreamTrack to this MediaStream.
	/// </summary>
	/// <param name="track">The track to add.</param>
	public abstract void AddTrack(IMediaStreamTrack track);

	/// <summary>
	/// Returns a sequence of MediaStreamTrack objects representing the audio tracks in this stream.
	/// </summary>
	/// <param name="track">The track to remove.</param>
	public abstract void RemoveTrack(IMediaStreamTrack track);

	/// <summary>
	/// Clones the given MediaStream and all its tracks.
	/// </summary>
	public abstract IMediaStream Clone();

	/// <summary>
	/// The Returns true if this MediaStream is active and false otherwise.
	/// </summary>
	public abstract bool Active { get; }

	/// <summary>
	/// The MediaStream became active.
	/// </summary>
	public abstract event EventHandler OnActive;

	/// <summary>
	/// The MediaStream became inactive.
	/// </summary>
	public abstract event EventHandler OnInactive;

	/// <summary>
	/// A new MediaStreamTrack has been added to this stream. 
	/// Note that this event is not fired when the application directly modifies the tracks of a MediaStream.
	/// </summary>
	public abstract event EventHandler<IMediaStreamTrack> OnAddTrack;

	/// <summary>
	/// A MediaStreamTrack has been removed from this stream.
	/// Note that this event is not fired when the script directly modifies the tracks of a MediaStream.
	/// </summary>
	public abstract event EventHandler<IMediaStreamTrack> OnRemoveTrack;

	/// <summary>
	/// Disposes the media stream and releases its native resources.
	/// </summary>
	public abstract void Dispose();
}