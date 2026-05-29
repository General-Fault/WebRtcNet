using System;

namespace WebRtcNet.Media;

/// <summary>
/// Constraints for the MediaTrack.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-constraints" />
/// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/MediaTrackConstraints" />
/// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Media_Streams_API/Constraints" />
public partial class MediaTrackConstraints
{
    /// <summary>
    /// The width or width range, in pixels. As a capability, the range should span the video source's pre-set width
    /// values with min being equal to 1 and max being the largest width.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width" />
    public PositiveUIntRangeConstraint Width { get; set; }

    /// <summary>
    /// The width or width range, in pixels. As a capability, the range should span the video source's pre-set width
    /// values with min being equal to 1 and max being the largest width.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height" />
    public PositiveUIntRangeConstraint Height { get; set; }

    /// <summary>
    /// The exact aspect ratio (width in pixels divided by height in pixels, represented as a double rounded to the tenth
    /// decimal place)
    /// or aspect ratio range.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect" />
    public PositiveDoubleRangeConstraint AspectRatio { get; set; }

    /// <summary>
    /// The exact frame rate (frames per second) or frame rate range. If video source's pre-set can determine frame rate
    /// values, the range, as a capacity, should span the video source's pre-set frame rate values with min being equal to
    /// 0 and max being the largest frame rate.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate" />
    public NonNegativeDoubleRangeConstraint FrameRate { get; set; }

    /// <summary>
    /// The directions that the camera can face, as seen from the user's perspective.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode" />
    public Constraint<VideoFacingModes> FacingMode { get; set; }

    /// <summary>
    /// The  means by which the resolution can be derived by the application. In other words, whether the application is
    /// allowed to use cropping and down-scaling on the camera output.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode" />
    public Constraint<VideoResizeModes> ResizeMode { get; set; }

    /// <summary>
    /// The sample rate in samples per second for the audio data.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate" />
    public PositiveUIntRangeConstraint SampleRate { get; set; }

    /// <summary>
    /// The linear sample size in bits. This constraint can only be satisfied for audio devices that produce linear
    /// samples.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize" />
    public PositiveUIntRangeConstraint SampleSize { get; set; }

    /// <summary>
    /// Indicates whether the user agent should attempt to blur the captured background, when supported.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-backgroundBlur" />
    public Constraint<bool> BackgroundBlur { get; set; }

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
    public EchoCancellationConstraint EchoCancellation { get; set; }

    /// <summary>
    /// Automatic gain control is often desirable on the input signal recorded by the microphone. There are cases where it
    /// is not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
    /// control this behavior.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl" />
    public Constraint<bool> AutoGainControl { get; set; }

    /// <summary>
    /// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is
    /// not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
    /// control this behavior.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression" />
    public Constraint<bool> NoiseSuppression { get; set; }

    /// <summary>
    /// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is
    /// not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
    /// control this behavior.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency" />
    public NonNegativeDoubleRangeConstraint Latency { get; set; }

    /// <summary>
    /// The number of independent channels of sound that the audio data contains, i.e. the number of audio samples per
    /// sample frame.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount" />
    public PositiveUIntRangeConstraint ChannelCount { get; set; }

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
    public StringConstraint DeviceId { get; set; }

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
    public StringConstraint GroupId { get; set; }
}