namespace WebRtcNet.Media;

/// <summary>
/// MediaTrackSettings represents the current settings of all constrainable properties of an
/// <see cref="IMediaStreamTrack" /> object. This is an out-only snapshot returned by
/// <see cref="IMediaStreamTrack.GetSettings" />.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-settings" />
public sealed record MediaTrackSettings
{
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width" />
	public uint Width { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height" />
	public uint Height { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect" />
	public double AspectRatio { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate" />
	public double FrameRate { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode" />
	public string FacingMode { get; init; } = string.Empty;

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-volume" />
	public double Volume { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate" />
	public uint SampleRate { get; init; }

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

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency" />
	public double Latency { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount" />
	public uint ChannelCount { get; init; }

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId" />
	public string DeviceId { get; init; } = string.Empty;

	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId" />
	public string GroupId { get; init; } = string.Empty;
}