using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// Describes the direction a video capture source is facing relative to the user.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-videofacingmodeenum"/>
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
/// MediaTrackCapabilities represents the capabilities of an <see cref="MediaStreamTrack"/> object as
/// reported by the platform. This is an out-only snapshot returned by
/// <see cref="MediaStreamTrack.GetCapabilities"/>.
/// </summary>
/// <remarks>
/// Sequence-valued capability members are represented as collections only. Scalar compatibility aliases are not exposed.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities"/>
public sealed record MediaTrackCapabilities
{
	/// <summary>
	/// The range of video frame widths (in pixels) supported by this track's source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-width"/>
	public ValueRange<uint>? Width { get; init; }

	/// <summary>
	/// The range of video frame heights (in pixels) supported by this track's source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-height"/>
	public ValueRange<uint>? Height { get; init; }

	/// <summary>
	/// The range of video frame aspect ratios (width divided by height) supported by this track's source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-aspectratio"/>
	public ValueRange<double>? AspectRatio { get; init; }

	/// <summary>
	/// The range of frame rates (in frames per second) supported by this track's source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-framerate"/>
	public ValueRange<double>? FrameRate { get; init; }

	/// <summary>
	/// The facing modes supported by this track's video capture source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-facingmode"/>
	public IReadOnlyList<VideoFacingModes> FacingMode { get; init; } = [];

	/// <summary>
	/// The resize modes supported by the application for this track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-resizemode"/>
	public IReadOnlyList<VideoResizeModes> ResizeMode { get; init; } = [];

	/// <summary>
	/// The range of sample rates (in samples per second) supported by this track's audio source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-samplerate"/>
	public ValueRange<uint>? SampleRate { get; init; }

	/// <summary>
	/// The range of linear sample sizes (in bits per sample) supported by this track's audio source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-samplesize"/>
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
	/// The background-blur values supported by the platform for this track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-backgroundBlur"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-backgroundblur"/>
	public IReadOnlyList<bool> BackgroundBlur { get; init; } = [];

	/// <summary>
	/// Whether automatic gain control is supported by this track's audio source. A sequence of
	/// <see langword="true"/> and/or <see langword="false"/> values indicating the supported states.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-autogaincontrol"/>
	public IReadOnlyList<bool> AutoGainControl { get; init; } = [];

	/// <summary>
	/// Whether noise suppression is supported by this track's audio source. A sequence of
	/// <see langword="true"/> and/or <see langword="false"/> values indicating the supported states.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-noisesuppression"/>
	public IReadOnlyList<bool> NoiseSuppression { get; init; } = [];

	/// <summary>
	/// The range of latencies (in seconds) supported by this track's audio source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-latency"/>
	public ValueRange<double>? Latency { get; init; }

	/// <summary>
	/// The range of audio channel counts supported by this track's audio source.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-channelcount"/>
	public ValueRange<uint>? ChannelCount { get; init; }

	/// <summary>
	/// The unique identifier for the capture device that is the source of this track.
	/// Two <see cref="MediaStreamTrack"/> objects sharing the same source will have the same <see cref="DeviceId"/>.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-deviceid"/>
	public string DeviceId { get; init; } = string.Empty;

	/// <summary>
	/// The group identifier of the device that is the source of this track. Two devices share a group
	/// identifier if they belong to the same physical hardware unit (for example, the built-in camera and
	/// microphone on the same laptop).
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId"/>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackcapabilities-groupid"/>
	public string GroupId { get; init; } = string.Empty;

	/// <summary>
	/// Factory method for creating MediaTrackCapabilities instances (for interop use only).
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	internal static MediaTrackCapabilities Create(
		ValueRange<uint>? width = null,
		ValueRange<uint>? height = null,
		ValueRange<double>? aspectRatio = null,
		ValueRange<double>? frameRate = null,
		IReadOnlyList<VideoFacingModes>? facingMode = null,
		IReadOnlyList<VideoResizeModes>? resizeMode = null,
		ValueRange<uint>? sampleRate = null,
		ValueRange<uint>? sampleSize = null,
		IReadOnlyList<EchoCancellationValue>? echoCancellation = null,
		IReadOnlyList<bool>? backgroundBlur = null,
		IReadOnlyList<bool>? autoGainControl = null,
		IReadOnlyList<bool>? noiseSuppression = null,
		ValueRange<double>? latency = null,
		ValueRange<uint>? channelCount = null,
		string? deviceId = null,
		string? groupId = null)
		=> new()
		{
			Width = width,
			Height = height,
			AspectRatio = aspectRatio,
			FrameRate = frameRate,
			FacingMode = facingMode ?? [],
			ResizeMode = resizeMode ?? [],
			SampleRate = sampleRate,
			SampleSize = sampleSize,
			EchoCancellation = echoCancellation ?? [],
			BackgroundBlur = backgroundBlur ?? [],
			AutoGainControl = autoGainControl ?? [],
			NoiseSuppression = noiseSuppression ?? [],
			Latency = latency,
			ChannelCount = channelCount,
			DeviceId = deviceId ?? string.Empty,
			GroupId = groupId ?? string.Empty
		};
}