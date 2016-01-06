using System;
using System.Collections.Generic;

namespace WebRtcNet
{
    /// <summary>
    /// Represents a media stream containing at least one audio or video track.
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#mediastream"/>
    /// Disposable to allow management of native resources.
    /// </summary>
    public interface IMediaStream : IDisposable
    {
        /// <summary>
        /// The Id that the stream was initialized with.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns a sequence of IMediaStreamTrack objects representing the audio tracks in this stream.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMediaStreamTrack> GetAudioTracks();

        /// <summary>
        /// Returns a sequence of MediaStreamTrack objects representing the video tracks in this stream.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMediaStreamTrack> GetVideoTracks();

        /// <summary>
        /// Returns a sequence of IMediaStreamTrack objects representing all the tracks in this stream.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMediaStreamTrack> GetTracks();

        /// <summary>
        /// Returns either an IMediaStreamTrack object from this stream's track set whose id is 
        /// equal to trackId, or null, if no such track exists.
        /// </summary>
        /// <param name="trackId">A track identifier.</param>
        /// <returns></returns>
        IMediaStreamTrack GetTrackById(string trackId);

        /// <summary>
        /// Adds the given MediaStreamTrack to this MediaStream.
        /// </summary>
        /// <param name="track">The track to add.</param>
        void AddTrack(IMediaStreamTrack track);

        /// <summary>
        /// Returns a sequence of MediaStreamTrack objects representing the audio tracks in this stream.
        /// </summary>
        /// <param name="track">The track to remove.</param>
        void RemoveTrack(IMediaStreamTrack track);

        /// <summary>
        /// Clones the given MediaStream and all its tracks.
        /// </summary>
        /// <returns></returns>
        IMediaStream Clone();

        /// <summary>
        /// The Returns true if this MediaStream is active and false otherwise.
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// The MediaStream became active.
        /// </summary>
        event EventHandler OnActive;

        /// <summary>
        /// The MediaStream became inactive.
        /// </summary>
        event EventHandler OnInactive;

        /// <summary>
        /// A new MediaStreamTrack has been added to this stream. 
        /// Note that this event is not fired when the application directly modifies the tracks of a MediaStream.
        /// </summary>
        event EventHandler<IMediaStreamTrack> OnAddTrack;

        /// <summary>
        /// A MediaStreamTrack has been removed from this stream.
        /// Note that this event is not fired when the script directly modifies the tracks of a MediaStream.
        /// </summary>
        event EventHandler<IMediaStreamTrack> OnRemoveTrack;
    }

}
