namespace WebRtcNet.Media;

/// <summary>
/// MediaTrackSupportedConstraints represents the set of constraints supported by a <see cref="IMediaStreamTrack"/> object.
/// This is an out-only snapshot
/// returned by <see cref="IMediaDevices.GetSupportedConstraints"/>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksupportedconstraints"/>
public sealed record MediaTrackSupportedConstraints
{
	/// <summary>Indicates whether the media track supports the width constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width"/>
	public bool Width { get; init; }

	/// <summary>Indicates whether the media track supports the height constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height"/>
	public bool Height { get; init; }

	/// <summary>Indicates whether the media track supports the aspect-ratio constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect"/>
	public bool AspectRatio { get; init; }

	/// <summary>Indicates whether the media track supports the frame-rate constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate"/>
	public bool FrameRate { get; init; }

	/// <summary>Indicates whether the media track supports the facing-mode constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode"/>
	public bool FacingMode { get; init; }

	/// <summary>Indicates whether the media track supports the resize-mode constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode"/>
	public bool ResizeMode { get; init; }

	/// <summary>Indicates whether the media track supports the sample-rate constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate"/>
	public bool SampleRate { get; init; }

	/// <summary>Indicates whether the media track supports the sample-size constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize"/>
	public bool SampleSize { get; init; }

	/// <summary>Indicates whether the media track supports the echo-cancellation constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation"/>
	public bool EchoCancellation { get; init; }

	/// <summary>
	/// Indicates whether the media track supports the background-blur constraint.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-backgroundBlur"/>
	public bool BackgroundBlur { get; init; }

	/// <summary>Indicates whether the media track supports the auto-gain-control constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl"/>
	public bool AutoGainControl { get; init; }

	/// <summary>Indicates whether the media track supports the noise-suppression constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression"/>
	public bool NoiseSuppression { get; init; }

	/// <summary>Indicates whether the media track supports the latency constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency"/>
	public bool Latency { get; init; }

	/// <summary>Indicates whether the media track supports the channel-count constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount"/>
	public bool ChannelCount { get; init; }

	/// <summary>Indicates whether the media track supports the device-id constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId"/>
	public bool DeviceId { get; init; }

	/// <summary>Indicates whether the media track supports the group-id constraint.</summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId"/>
	public bool GroupId { get; init; }
}