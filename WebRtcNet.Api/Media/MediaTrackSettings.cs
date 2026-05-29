namespace WebRtcNet.Media;

/// <summary>
/// MediaTrackSettings represents the current settings of all constrainable properties of an
/// <see cref="MediaStreamTrack" /> object. This is an out-only snapshot returned by
/// <see cref="MediaStreamTrack.GetSettings" />.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-settings" />
public sealed record MediaTrackSettings
{
	/// <summary>
	/// Current width setting of the track in pixels.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width" />
	public uint Width { get; init; }

	/// <summary>
	/// Current height setting of the track in pixels.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height" />
	public uint Height { get; init; }

	/// <summary>
	/// Current aspect ratio setting of the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect" />
	public double AspectRatio { get; init; }

	/// <summary>
	/// Current frame rate setting of the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate" />
	public double FrameRate { get; init; }

	/// <summary>
	/// Current facing mode setting for the track, when exposed by the platform.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings-facingmode" />
	public VideoFacingModes? FacingMode { get; init; }

	/// <summary>
	/// Current resize mode for the track, when exposed by the platform.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings-resizemode" />
	public VideoResizeModes? ResizeMode { get; init; }

	/// <summary>
	/// Current volume setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-volume" />
	public double Volume { get; init; }

	/// <summary>
	/// Current audio sample rate setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate" />
	public uint SampleRate { get; init; }

	/// <summary>
	/// Current audio sample size setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize" />
	public uint SampleSize { get; init; }

	/// <summary>
	/// Current echo-cancellation setting for the track, represented as either a boolean value
	/// or a mode string.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings-echocancellation" />
	public EchoCancellationValue EchoCancellation { get; init; } = new(false);

	/// <summary>
	/// Indicates whether background blur is enabled for the current track settings.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-backgroundBlur" />
	public bool BackgroundBlur { get; init; }

	/// <summary>
	/// Indicates whether automatic gain control is enabled for the current track settings.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings-autogaincontrol" />
	public bool? AutoGainControl { get; init; }

	/// <summary>
	/// Indicates whether noise suppression is enabled for the current track settings.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksettings-noisesuppression" />
	public bool? NoiseSuppression { get; init; }

	/// <summary>
	/// Current latency setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency" />
	public double Latency { get; init; }

	/// <summary>
	/// Current channel count setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount" />
	public uint ChannelCount { get; init; }

	/// <summary>
	/// Current device identifier setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId" />
	public string DeviceId { get; init; } = string.Empty;

	/// <summary>
	/// Current group identifier setting for the track.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId" />
	public string GroupId { get; init; } = string.Empty;
}