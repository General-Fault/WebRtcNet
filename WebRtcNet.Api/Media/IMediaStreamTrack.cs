using System;

namespace WebRtcNet.Media
{
    /// <summary>
    ///     The usable state of the MediaStreamTrack
    /// </summary>
    /// <seealso cref="IMediaStreamTrack.ReadyState" />
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrackstate" />
    public enum MediaStreamTrackState
    {
        /// <summary>
        ///     The track is active (the track's underlying media source is making a best-effort attempt to provide data in real
        ///     time).
        ///     The output of a track in the live state can be switched on and off with the enabled attribute.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#idl-def-MediaStreamTrackState.live" />
        Live,

        /// <summary>
        ///     The track has ended(the track's underlying media source is no longer providing data, and will never provide
        ///     more data for this track). Once a track enters this state, it never exits it.
        ///     For example, a video track in a MediaStream ends when the user unplugs the USB web camera that acts as the track's
        ///     media source.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#idl-def-MediaStreamTrackState.ended" />
        Ended
    }

    /// <summary>
    ///     The kind of media stream track.
    /// </summary>
    /// <seealso cref="IMediaStreamTrack.Kind" />
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-kind" />
    public enum MediaStreamTrackKind
    {
        /// <summary>
        ///     This is an audio track.
        /// </summary>
        Audio,

        /// <summary>
        ///     This is a video track.
        /// </summary>
        Video
    }


    /// <summary>
    ///     A <see cref="IMediaStreamTrack">MediaStreamTrack</see> object represents a media source in the application. An
    ///     example
    ///     source is a device connected to the computer. Other specifications may define sources for
    ///     <see cref="IMediaStreamTrack">MediaStreamTrack</see> that override the behavior specified here. Several
    ///     <see cref="IMediaStreamTrack">MediaStreamTrack</see> objects can represent the same  media source, e.g., when the
    ///     user
    ///     chooses the same camera in the UI shown by two consecutive calls to <see cref="IMediaDevices.GetUserMedia" />.
    /// </summary>
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#mediastreamtrack" />
    public interface IMediaStreamTrack
    {
        /// <summary>
        ///     <see cref="MediaStreamTrackKind.Audio">Audio</see> if the object represents an audio track or
        ///     <see cref="MediaStreamTrackKind.Video">Video</see> if object represents a video track.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-kind" />
        MediaStreamTrackKind Kind { get; }

        /// <summary>
        ///     A generated identifier for the track.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-id" />
        string Id { get; }

        /// <summary>
        ///     The audio or video source label if available (e.g., "Internal microphone" or "External USB Webcam").
        ///     Empty string if no label is available.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-id" />
        string Label { get; }

        /// <summary>
        ///     Enabled controls the enabled state for the object.
        /// </summary>
        /// <remarks>
        ///     After a MediaStreamTrack has ended, its enabled attribute still changes value when set; it just doesn't do anything
        ///     with that new value.
        /// </remarks>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-enabled" />
        bool Enabled { get; set; }

        /// <summary>
        ///     Muted returns true if the track is muted, and false otherwise.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-muted" />
        bool Muted { get; }

        /// <summary>
        ///     ReadyState represents the usable state of the track.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-readystate" />
        MediaStreamTrackState ReadyState { get; }

        /// <summary>
        ///     When fired, the MediaStreamTrack object's source is temporarily unable to provide data.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-onmute" />
        event EventHandler OnMute;

        /// <summary>
        ///     The MediaStreamTrack object's source is live again after having been temporarily unable to provide data.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-onunmute" />
        event EventHandler OnUnMute;

        /// <summary>
        ///     The MediaStreamTrack object's source will no longer provide any data, either because the user
        ///     revoked the permissions, or because the source device has been ejected,
        ///     or because the remote peer permanently stopped sending data.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-onended" />
        event EventHandler<MediaStreamError> OnEnded;

        /// <summary>
        ///     Clones the given MediaStreamTrack.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-clone" />
        IMediaStreamTrack Clone();

        /// <summary>
        ///     Stops the locally sourced track. If the track is remote, this does nothing.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-stop" />
        void Stop();

        /// <summary>
        ///     Returns the <see cref="MediaTrackCapabilities">capabilites</see> of the source that this MediaStreamTrack, the
        ///     constrainable object, represents.
        ///     See
        ///     <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface">ConstrainablePattern Interface</seealso>
        ///     for the definition of this method.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-getcapabilities" />
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface" />
        MediaTrackCapabilities GetCapabilities();

        /// <summary>
        ///     Returns the Constraints that were the argument to the most recent successful call of
        ///     <see cref="ApplyConstraints" />, maintaining the order in which they were specified. Note that some of the optional
        ///     <see cref="MediaStreamConstraints" />ConstraintSets returned may not be currently satisfied. To check which
        ///     ConstraintSets are
        ///     currently in effect, the application should use <see cref="GetSettings" />.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-getconstraints" />
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface" />
        MediaTrackConstraints GetConstraints();

        /// <summary>
        ///     Returns the current <seealso cref="MediaTrackSettings">settings</seealso> of all the constrainable properties of
        ///     the object, whether they are platform defaults or have been set by <see cref="ApplyConstraints" />.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-getsettings" />
        MediaTrackSettings GetSettings();

        /// <summary>
        ///     Apply the supplied <see cref="MediaTrackConstraints">constraints</see>. Use null to remove all constraints.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-applyconstraints" />
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface" />
        void ApplyConstraints(MediaTrackConstraints constraints = null);
    }
}