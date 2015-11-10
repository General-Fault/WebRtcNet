#pragma once

#include "MediaTrackSupportedConstraints.h"
#include "MediaTrackConstraints.h"
#include "MediaTrackCapabilities.h"
#include "MediaTrackSettings.h"

WEBRTCNET_START

public enum MediaStreamTrackState
{
	/// The track is active (the track's underlying media source is making a best-effort attempt to provide data in real time).
	/// The output of a track in the live state can be switched on and off with the enabled attribute.
	Live,

	/// The track has ended(the track's underlying media source is no longer providing data, and will never provide 
	/// more data for this track). Once a track enters this state, it never exits it.
	/// For example, a video track in a MediaStream ends when the user unplugs the USB web camera that acts as the track's media source.
	Ended
};

public ref class MediaStreamError
{
	/// The name of the error.
	property String ^ Name { String ^ get() { throw gcnew NotImplementedException(); } }

	/// A string offering extra human-readable information about the error.
	property String ^ Message { String ^ get() { throw gcnew NotImplementedException(); } }

	/// This attribute is only used for some types of errors. For MediaStreamError with a name of 
	/// ConstraintNotSatisfiedError or of OverconstrainedError, this attribute must be set to the name of the constraint that caused 
	property String ^ ConstraintName { String ^ get() { throw gcnew NotImplementedException(); } }
};

public interface class IMediaStreamTrack
{
	/// The string "audio" if the object represents an audio track or "video" if object represents a video track.
	property String^ Kind { String^ get(); }

	/// A generated identifier for the track.
	property String^ Id { String^ get(); }

	/// The audio or video source label if available (e.g., "Internal microphone" or "External USB Webcam"). 
	/// Empty string if no label is available.
	property String^ Label { String^ get(); }

	/// Enabled controls the enabled state for the object.
	property Boolean Enabled { Boolean get(); void set(Boolean value); }

	/// Muted returns true if the track is muted, and false otherwise.
	property Boolean Muted { Boolean get(); }

	/// The MediaStreamTrack object's source is temporarily unable to provide data.
	event EventHandler^ OnMute;

	/// The MediaStreamTrack object's source is live again after having been temporarily unable to provide data.
	event EventHandler^ OnUnMute;

	/// Readonly returns true if the track (audio or video) source is a local microphone or camera that is shared 
	/// so that constraints applied to the track cannot modify the source's settings. Otherwise, it returns false.
	property Boolean ReadOnly { Boolean get(); }


	/// Remote returns true if the track is sourced by a non-local source. Otherwise it returns false.
	property Boolean Remote { Boolean get(); }

	/// ReadyState represents the state of the track.
	property MediaStreamTrackState ReadyState { MediaStreamTrackState get(); }

	/// The MediaStreamTrack object's source will no longer provide any data, either because the user 
	/// revoked the permissions, or because the source device has been ejected, 
	/// or because the remote peer permanently stopped sending data.
	event EventHandler<MediaStreamError ^> ^ OnEnded;

	/// Clones the given MediaStreamTrack.
	IMediaStreamTrack^ Clone();

	/// Stops the locally sourced track. If the track is remote, this does nothing.
	void Stop();

	/// Returns the dictionary of the names of the constrainable properties that the object supports.
	MediaTrackCapabilities ^ GetCapabilities();

	/// Returns the Constraints that were the argument to the most recent successful call of ApplyConstraints(), 
	/// maintaining the order in which they were specified. Note that some of the optional ConstraintSets returned may not 
	/// be currently satisfied. To check which ConstraintSets are currently in effect, the application should use getSettings.
	MediaTrackConstraints ^ GetConstraints();
	
	/// Returns the current settings of all the constrainable properties of the object, whether they are platform defaults or have been set by applyConstraints().
	MediaTrackSettings ^ GetSettings();

	/// Apply the supplied constraints. Use null to remove all constraints.
	void ApplyConstraints([Optional] MediaTrackConstraints ^ constraints);

	/// Fired when no longer able to satisfy the requiredConstraints from the currently valid Constraints.
	event EventHandler<MediaStreamError ^> ^ OnOverConstrained;

};

WEBRTCNET_END