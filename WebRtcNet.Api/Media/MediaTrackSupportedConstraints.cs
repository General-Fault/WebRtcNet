namespace WebRtcNet.Media;

/// <summary>
/// MediaTrackSupportedConstraints represents the set of constraints recognized by the user agent for
/// controlling the capabilities of a <see cref="IMediaStreamTrack"/> object. This is an out-only snapshot
/// returned by <see cref="IMediaDevices.GetSupportedConstraints"/>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksupportedconstraints"/>
public sealed record class MediaTrackSupportedConstraints
{
    /// <see cref="MediaTrackConstraints.Width"/>
    public bool Width { get; init; }

    /// <see cref="MediaTrackConstraints.Height"/>
    public bool Height { get; init; }

    /// <see cref="MediaTrackConstraints.AspectRatio"/>
    public bool AspectRatio { get; init; }

    /// <see cref="MediaTrackConstraints.FrameRate"/>
    public bool FrameRate { get; init; }

    /// <see cref="MediaTrackConstraints.FacingMode"/>
    public bool FacingMode { get; init; }

    /// <see cref="MediaTrackConstraints.ResizeMode"/>
    public bool ResizeMode { get; init; }

    /// <see cref="MediaTrackConstraints.SampleRate"/>
    public bool SampleRate { get; init; }

    /// <see cref="MediaTrackConstraints.SampleSize"/>
    public bool SampleSize { get; init; }

    /// <see cref="MediaTrackConstraints.EchoCancellation"/>
    public bool EchoCancellation { get; init; }

    /// <see cref="MediaTrackConstraints.AutoGainControl"/>
    public bool AutoGainControl { get; init; }

    /// <see cref="MediaTrackConstraints.NoiseSuppression"/>
    public bool NoiseSuppression { get; init; }

    /// <see cref="MediaTrackConstraints.Latency"/>
    public bool Latency { get; init; }

    /// <see cref="MediaTrackConstraints.ChannelCount"/>
    public bool ChannelCount { get; init; }

    /// <see cref="MediaTrackConstraints.DeviceId"/>
    public bool DeviceId { get; init; }

    /// <see cref="MediaTrackConstraints.GroupId"/>
    public bool GroupId { get; init; }
}