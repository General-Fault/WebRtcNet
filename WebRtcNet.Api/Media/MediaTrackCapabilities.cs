using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// Describes the direction a video capture source is facing relative to the user.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#idl-def-VideoFacingModeEnum"/>
public enum VideoFacingModes
{
	/// <summary>
	/// The source faces toward the user.
	/// </summary>
	User,

	/// <summary>
	/// The source faces away from the user toward the environment.
	/// </summary>
	Environment,

	/// <summary>
	/// The source faces to the user's left.
	/// </summary>
	Left,

	/// <summary>
	/// The source faces to the user's right.
	/// </summary>
	Right
}

/// <summary>
/// Describes how video from a particular video track may be resized.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-videoresizemodeenum"/>
public enum VideoResizeModes
{
	/// <summary>
	/// No resizing is applied.
	/// </summary>
	None,

	/// <summary>
	/// Cropping and down-scaling may be applied.
	/// </summary>
	CropAndScale
}

/// <summary>
/// MediaTrackCapabilities represents the capabilities of an <see cref="IMediaStreamTrack"/> object as
/// reported by the platform. This is an out-only snapshot returned by
/// <see cref="IMediaStreamTrack.GetCapabilities"/>.
/// </summary>
/// <remarks>
/// Sequence-valued capability members are represented as collections only. Scalar compatibility aliases are not exposed.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities"/>
public sealed record MediaTrackCapabilities
{
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width"/>
	public ValueRange<uint>? Width { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height"/>
	public ValueRange<uint>? Height { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect"/>
	public ValueRange<double>? AspectRatio { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate"/>
	public ValueRange<double>? FrameRate { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-facingmode"/>
	public IReadOnlyList<VideoFacingModes> FacingMode { get; init; } = [];

	/// <summary>
	/// The resize modes supported by the user agent for this track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-resizemode"/>
	public IReadOnlyList<VideoResizeModes> ResizeMode { get; init; } = [];

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate"/>
	public ValueRange<uint>? SampleRate { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize"/>
	public ValueRange<uint>? SampleSize { get; init; }

	/// <summary>
	/// Supported echo-cancellation values. Boolean values appear first when present, followed
	/// by any mode values.
	/// </summary>
	/// <remarks>
	/// Mode entries are represented as <see cref="EchoCancellationValue"/> values backed by
	/// raw mode strings for forward compatibility.
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-echocancellation"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-echocancellationmodeenum"/>
	public IReadOnlyList<EchoCancellationValue> EchoCancellation { get; init; } = [];

	/// <summary>
	/// The background-blur values supported by the user agent for this track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-backgroundBlur"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-backgroundblur"/>
	public IReadOnlyList<bool> BackgroundBlur { get; init; } = [];

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-autogaincontrol"/>
	public IReadOnlyList<bool> AutoGainControl { get; init; } = [];

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-noisesuppression"/>
	public IReadOnlyList<bool> NoiseSuppression { get; init; } = [];

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency"/>
	public ValueRange<double>? Latency { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount"/>
	public ValueRange<uint>? ChannelCount { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId"/>
	public string DeviceId { get; init; } = string.Empty;

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId"/>
	public string GroupId { get; init; } = string.Empty;
}