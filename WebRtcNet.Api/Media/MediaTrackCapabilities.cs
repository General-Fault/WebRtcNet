using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// Describes the direction a video capture source is facing relative to the user.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#idl-def-VideoFacingModeEnum"/>
public enum VideoFacingModes { user, environment, left, right }

/// <summary>
/// Describes how video from a particular video track may be resized.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-videoresizemodeenum"/>
public enum VideResizeModes { none, crop_and_scale }

/// <summary>
/// MediaTrackCapabilities represents the capabilities of an <see cref="IMediaStreamTrack"/> object as
/// reported by the platform. This is an out-only snapshot returned by
/// <see cref="IMediaStreamTrack.GetCapabilities"/>.
/// </summary>
/// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-capabilities"/>
public sealed record class MediaTrackCapabilities
{
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width"/>
    public ValueRange<int>? Width { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height"/>
    public ValueRange<int>? Height { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect"/>
    public ValueRange<double>? AspectRatio { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate"/>
    public ValueRange<double>? FrameRate { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode"/>
    public VideoFacingModes? FacingMode { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode"/>
    public VideResizeModes? ResizeMode { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate"/>
    public ValueRange<int>? SampleRate { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize"/>
    public ValueRange<int>? SampleSize { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation"/>
    public IReadOnlyList<bool> EchoCancellation { get; init; } = [];

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl"/>
    public IReadOnlyList<bool> AutoGainControl { get; init; } = [];

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression"/>
    public IReadOnlyList<bool> NoiseSuppression { get; init; } = [];

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency"/>
    public ValueRange<double>? Latency { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount"/>
    public ValueRange<double>? ChannelCount { get; init; }

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId"/>
    public string DeviceId { get; init; } = string.Empty;

    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId"/>
    public string GroupId { get; init; } = string.Empty;
}