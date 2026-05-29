using System;
using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// A single media track constraint set.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraintset" />
/// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/MediaTrackConstraints" />
/// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Media_Streams_API/Constraints" />
public class MediaTrackConstraintSet
{
	/// <summary>
	/// The width or width range, in pixels. As a capability, the range should span the video source's pre-set width
	/// values with min being equal to 1 and max being the largest width.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width" />
	public MediaTrackConstraints.PositiveUIntRangeConstraint? Width { get; set; }

	/// <summary>
	/// The width or width range, in pixels. As a capability, the range should span the video source's pre-set width
	/// values with min being equal to 1 and max being the largest width.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height" />
	public MediaTrackConstraints.PositiveUIntRangeConstraint? Height { get; set; }

	/// <summary>
	/// The exact aspect ratio (width in pixels divided by height in pixels, represented as a double rounded to the tenth
	/// decimal place)
	/// or aspect ratio range.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect" />
	public MediaTrackConstraints.PositiveDoubleRangeConstraint? AspectRatio { get; set; }

	/// <summary>
	/// The exact frame rate (frames per second) or frame rate range. If video source's pre-set can determine frame rate
	/// values, the range, as a capacity, should span the video source's pre-set frame rate values with min being equal to
	/// 0 and max being the largest frame rate.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate" />
	public MediaTrackConstraints.NonNegativeDoubleRangeConstraint? FrameRate { get; set; }

	/// <summary>
	/// The directions that the camera can face, as seen from the user's perspective.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode" />
	public MediaTrackConstraints.Constraint<VideoFacingModes>? FacingMode { get; set; }

	/// <summary>
	/// The  means by which the resolution can be derived by the application. In other words, whether the application is
	/// allowed to use cropping and down-scaling on the camera output.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode" />
	public MediaTrackConstraints.Constraint<VideoResizeModes>? ResizeMode { get; set; }

	/// <summary>
	/// The sample rate in samples per second for the audio data.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate" />
	public MediaTrackConstraints.PositiveUIntRangeConstraint? SampleRate { get; set; }

	/// <summary>
	/// The linear sample size in bits. This constraint can only be satisfied for audio devices that produce linear
	/// samples.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize" />
	public MediaTrackConstraints.PositiveUIntRangeConstraint? SampleSize { get; set; }

	/// <summary>
	/// Indicates whether the implementation should attempt to blur the captured background, when supported.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-backgroundBlur" />
	public MediaTrackConstraints.Constraint<bool>? BackgroundBlur { get; set; }

	/// <summary>
	/// Controls echo cancellation using either a boolean toggle or a named mode value.
	/// Use <see langword="true" />/<see langword="false" /> for basic enable/disable behavior,
	/// or use <see cref="EchoCancellationMode" /> for mode-specific behavior when supported.
	/// </summary>
	/// <remarks>
	/// This corresponds to <c>ConstrainBooleanOrDOMString</c> in the spec.
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainbooleanordomstring" />
	public EchoCancellationConstraint? EchoCancellation { get; set; }

	/// <summary>
	/// Automatic gain control is often desirable on the input signal recorded by the microphone. There are cases where it
	/// is not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
	/// control this behavior.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl" />
	public MediaTrackConstraints.Constraint<bool>? AutoGainControl { get; set; }

	/// <summary>
	/// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is
	/// not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
	/// control this behavior.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression" />
	public MediaTrackConstraints.Constraint<bool>? NoiseSuppression { get; set; }

	/// <summary>
	/// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is
	/// not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
	/// control this behavior.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency" />
	public MediaTrackConstraints.NonNegativeDoubleRangeConstraint? Latency { get; set; }

	/// <summary>
	/// The number of independent channels of sound that the audio data contains, i.e. the number of audio samples per
	/// sample frame.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount" />
	public MediaTrackConstraints.PositiveUIntRangeConstraint? ChannelCount { get; set; }

	/// <summary>
	/// The identifier of the device generating the content of the <see cref="IMediaStreamTrack">MediaStreamTrack</see>.
	/// It conforms with the definition of <see cref="MediaDeviceInfo.DeviceId">MediaDeviceInfo.DeviceId</see>. Note that
	/// the setting of this property is uniquely determined by the source that is attached to the
	/// <see cref="IMediaStreamTrack">MediaStreamTrack</see>. In particular,
	/// <see cref="IMediaStreamTrack.GetCapabilities">GetCapabilities()</see> will return only a single value for
	/// DeviceId. This property can therefore be used for initial media selection with
	/// <see cref="IMediaDevices.GetUserMedia">GetUserMedia()</see>. However, it is not useful for subsequent media
	/// control with <see cref="IMediaStreamTrack.ApplyConstraints">ApplyConstraints()</see>, since any attempt to set a
	/// different value will result in an unsatisfiable ConstraintSet. If a string of length 0 is used as a DeviceId value
	/// constraint with <see cref="IMediaDevices.GetUserMedia">GetUserMedia()</see>, it MAY be interpreted as if the
	/// constraint is not specified.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId" />
	public MediaTrackConstraints.StringConstraint? DeviceId { get; set; }

	/// <summary>
	/// The application-unique group identifier for the device generating the content of the
	/// <see cref="IMediaStreamTrack">MediaStreamTrack</see>. It conforms with the definition of
	/// <see cref="MediaDeviceInfo.GroupId">MediaDeviceInfo.GroupId</see>. Note that the setting of this property is
	/// uniquely determined by the source that is attached to the <see cref="IMediaStreamTrack">MediaStreamTrack</see>.
	/// In particular, <see cref="IMediaStreamTrack.GetCapabilities">GetCapabilities()</see> will return only a single
	/// value for groupId. Since this property is not stable between browsing sessions, its usefulness for initial media
	/// selection with <see cref="IMediaDevices.GetUserMedia">GetUserMedia()</see> is limited. It is not useful for
	/// subsequent media control with <see cref="IMediaStreamTrack.ApplyConstraints">ApplyConstraints()</see>, since any
	/// attempt to set a different value will result in an unsatisfiable ConstraintSet.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId" />
	public MediaTrackConstraints.StringConstraint? GroupId { get; set; }

	/// <summary>
	/// Gets whether this set contains any required constraints (for example <c>exact</c>, <c>min</c>, or <c>max</c>).
	/// </summary>
	/// <remarks>
	/// Local reference: <c>documents/specs/mediacapture/mediacapture-idl.webidl</c>
	/// (<c>MediaTrackConstraintSet</c> and <c>Constrain*</c> definitions).
	/// </remarks>
	public bool HasRequiredConstraints =>
		(Width?.IsRequired ?? false) ||
		(Height?.IsRequired ?? false) ||
		(AspectRatio?.IsRequired ?? false) ||
		(FrameRate?.IsRequired ?? false) ||
		(FacingMode?.IsRequired ?? false) ||
		(ResizeMode?.IsRequired ?? false) ||
		(SampleRate?.IsRequired ?? false) ||
		(SampleSize?.IsRequired ?? false) ||
		(BackgroundBlur?.IsRequired ?? false) ||
		(EchoCancellation?.Exact.HasValue ?? false) ||
		(AutoGainControl?.IsRequired ?? false) ||
		(NoiseSuppression?.IsRequired ?? false) ||
		(Latency?.IsRequired ?? false) ||
		(ChannelCount?.IsRequired ?? false) ||
		(DeviceId?.IsRequired ?? false) ||
		(GroupId?.IsRequired ?? false);
}

/// <summary>
/// Constraints for the MediaTrack.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraints" />
public partial class MediaTrackConstraints : MediaTrackConstraintSet
{
	private IList<MediaTrackConstraintSet>? _advanced;

	/// <summary>
	/// A sequence of additional constraint sets to apply in the order supplied by the caller.
	/// </summary>
	/// <remarks>
	/// This corresponds to <c>MediaTrackConstraints.advanced</c> in the specification.
	/// Local reference: <c>documents/specs/mediacapture/mediacapture-idl.webidl</c>
	/// (<c>MediaTrackConstraints</c>, <c>MediaTrackConstraintSet</c>, and
	/// <c>MediaStreamTrack.applyConstraints(optional MediaTrackConstraints constraints = {})</c>).
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraints-advanced" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraintset" />
	public IList<MediaTrackConstraintSet>? Advanced
	{
		get => _advanced;
		set
		{
			if (value is not null)
			{
				for (var i = 0; i < value.Count; i++)
				{
					if (value[i] is null)
						throw new ArgumentException("Advanced constraint sets cannot contain null entries.", nameof(value));
				}
			}

			_advanced = value;
		}
	}

	/// <summary>
	/// Enumerates constraints in processing order: base set first, then <see cref="Advanced" /> entries in list order.
	/// </summary>
	/// <remarks>
	/// Local reference: <c>documents/specs/mediacapture/mediacapture-idl.webidl</c>
	/// (<c>MediaTrackConstraints</c> and <c>MediaTrackConstraints.advanced</c> sequence ordering).
	/// </remarks>
	public IEnumerable<MediaTrackConstraintSet> EnumerateConstraintSetsInProcessingOrder()
	{
		yield return this;

		if (_advanced is null)
			yield break;

		foreach (var constraintSet in _advanced)
			yield return constraintSet;
	}
}