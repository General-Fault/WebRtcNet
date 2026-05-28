namespace WebRtcNet.Media;

/// <summary>
/// MediaTrackSettings represents the current settings of all constrainable properties of an
/// <see cref="IMediaStreamTrack"/> object. This is an out-only snapshot returned by
/// <see cref="IMediaStreamTrack.GetSettings"/>.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-settings"/>
public sealed record class MediaTrackSettings
{
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width"/>
    public int Width { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height"/>
    public int Height { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect"/>
    public double AspectRatio { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate"/>
    public double FrameRate { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode"/>
    public string FacingMode { get; init; } = string.Empty;

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-volume"/>
    public double Volume { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate"/>
    public int SampleRate { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize"/>
    public int SampleSize { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation"/>
    public bool EchoCancellation { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency"/>
    public double Latency { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount"/>
    public int ChannelCount { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId"/>
    public string DeviceId { get; init; } = string.Empty;

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId"/>
    public string GroupId { get; init; } = string.Empty;
}