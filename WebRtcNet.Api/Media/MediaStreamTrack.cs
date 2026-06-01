using System;

namespace WebRtcNet.Media;

/// <summary>
/// The usable state of the MediaStreamTrack
/// </summary>
/// <seealso cref="MediaStreamTrack.ReadyState" />
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrackstate" />
public enum MediaStreamTrackState
{
	/// <summary>
	/// The track is active (the track's underlying media source is making a best-effort attempt to provide data in real
	/// time).
	/// The output of a track in the live state can be switched on and off with the enabled attribute.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrackstate" />
	Live,

	/// <summary>
	/// The track has ended(the track's underlying media source is no longer providing data, and will never provide
	/// more data for this track). Once a track enters this state, it never exits it.
	/// For example, a video track in a MediaStream ends when the user unplugs the USB web camera that acts as the track's
	/// media source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrackstate" />
	Ended
}

/// <summary>
/// The kind of media stream track.
/// </summary>
/// <seealso cref="MediaStreamTrack.Kind" />
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-kind" />
public enum MediaStreamTrackKind
{
	/// <summary>
	/// This is an audio track.
	/// </summary>
	Audio,

	/// <summary>
	/// This is a video track.
	/// </summary>
	Video
}

/// <summary>
/// A <see cref="MediaStreamTrack">MediaStreamTrack</see> object represents a media source in the application. An
/// example
/// source is a device connected to the computer. Other specifications may define sources for
/// <see cref="MediaStreamTrack">MediaStreamTrack</see> that override the behavior specified here. Several
/// <see cref="MediaStreamTrack">MediaStreamTrack</see> objects can represent the same  media source, e.g., when the
/// user
/// chooses the same camera in the UI shown by two consecutive calls to <see cref="MediaDevices.GetUserMedia" />.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack" />
public abstract class MediaStreamTrack
{
	/// <summary>
	/// <see cref="MediaStreamTrackKind.Audio">Audio</see> if the object represents an audio track or
	/// <see cref="MediaStreamTrackKind.Video">Video</see> if object represents a video track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-kind" />
	public abstract MediaStreamTrackKind Kind { get; }

	/// <summary>
	/// Returns the native media stream track interface used by WebRtcInterop.
	/// </summary>
	/// <param name="throwOnDisposed">True to throw when the track has already been disposed.</param>
	protected internal abstract IntPtr GetNativeMediaStreamTrackInterface(bool throwOnDisposed);

	/// <summary>
	/// A generated identifier for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-id" />
	public abstract string Id { get; }

	/// <summary>
	/// The audio or video source label if available (e.g., "Internal microphone" or "External USB Webcam").
	/// Empty string if no label is available.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-id" />
	public abstract string Label { get; }

	/// <summary>
	/// Enabled controls the enabled state for the object.
	/// </summary>
	/// <remarks>
	/// After a MediaStreamTrack has ended, its enabled attribute still changes value when set; it just doesn't do anything
	/// with that new value.
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-enabled" />
	public abstract bool Enabled { get; set; }

	/// <summary>
	/// Muted returns true if the track is muted, and false otherwise.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-muted" />
	public abstract bool Muted { get; }

	/// <summary>
	/// ReadyState represents the usable state of the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-readystate" />
	public abstract MediaStreamTrackState ReadyState { get; }

	/// <summary>
	/// When fired, the MediaStreamTrack object's source is temporarily unable to provide data.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-onmute" />
	public abstract event EventHandler OnMute;

	/// <summary>
	/// The MediaStreamTrack object's source is live again after having been temporarily unable to provide data.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-onunmute" />
	public abstract event EventHandler OnUnMute;

	/// <summary>
	/// The MediaStreamTrack object's source will no longer provide any data, either because the user
	/// revoked the permissions, or because the source device has been ejected,
	/// or because the remote peer permanently stopped sending data.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-onended" />
	public abstract event EventHandler OnEnded;

	/// <summary>
	/// Clones the given MediaStreamTrack.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-clone" />
	public abstract MediaStreamTrack Clone();

	/// <summary>
	/// Stops the locally sourced track. If the track is remote, this does nothing.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-stop" />
	public abstract void Stop();

	/// <summary>
	/// Returns the <see cref="MediaTrackCapabilities">capabilites</see> of the source that this MediaStreamTrack, the
	/// constrainable object, represents.
	/// See
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface">ConstrainablePattern Interface</seealso>
	/// for the definition of this method.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-getcapabilities" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface" />
	/// <returns>The source capabilities for this track.</returns>
	public abstract MediaTrackCapabilities GetCapabilities();

	/// <summary>
	/// Returns the constraints supplied to the most recent successful call to
	/// <see cref="ApplyConstraints" />.
	/// </summary>
	/// <remarks>
	/// Returned <see cref="MediaTrackConstraints" /> values can include ideal or advanced entries that are not currently
	/// active. Use <see cref="GetSettings" /> to inspect effective values.
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-getconstraints" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface" />
	/// <returns>The currently applied constraints for this track.</returns>
	public abstract MediaTrackConstraints GetConstraints();

	/// <summary>
	/// Returns the current <seealso cref="MediaTrackSettings">settings</seealso> of all the constrainable properties of
	/// the object, whether they are platform defaults or have been set by <see cref="ApplyConstraints" />.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-getsettings" />
	/// <returns>The current settings snapshot for this track.</returns>
	public abstract MediaTrackSettings GetSettings();

	/// <summary>
	/// Apply the supplied <see cref="MediaTrackConstraints">constraints</see>. Use null to remove all constraints.
	/// </summary>
	/// <remarks>
	/// Required constraints (for example <c>exact</c>, <c>min</c>, and <c>max</c>) participate in acceptance checks.
	/// Advanced sets are evaluated in the order supplied and may be skipped when unsatisfied without failing the entire apply.
	/// Local reference: <c>documents/specs/mediacapture/mediacapture-idl.webidl</c>
	/// (<c>MediaStreamTrack.applyConstraints</c>, <c>MediaTrackConstraintSet</c>, and <c>MediaTrackConstraints.advanced</c>).
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamtrack-applyconstraints" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#constrainable-interface" />
	/// <param name="constraints">Constraints to apply, or <see langword="null" /> to clear constraints.</param>
	public abstract void ApplyConstraints(MediaTrackConstraints? constraints = null);
}