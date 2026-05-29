namespace WebRtcNet.Media;

/// <summary>
/// Contains the MediaTrackConstraints for both the audio and video tracks in a media stream.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints" />
public class MediaStreamConstraints
{
    /// <summary>
    /// If true, it requests that the returned <see cref="IMediaStream">MediaStream</see> contain a audio track. If a
    /// <see cref="MediaTrackConstraints"/> structure is provided, it further specifies the nature and settings of the
    /// audio Track. If false, the <see cref="IMediaStream">MediaStream</see> will not contain an audio Track.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints-audio" />
    public bool Audio { get; set; }

    /// <summary>
    /// Optional detailed audio constraints when <see cref="Audio" /> is enabled.
    /// </summary>
    public MediaTrackConstraints? AudioConstraints { get; set; }

    /// <summary>
    /// If true, it requests that the returned <see cref="IMediaStream">MediaStream</see> contain a video track. If a
    /// <see cref="MediaTrackConstraints"/> structure is provided, it further specifies the nature and settings of the
    /// video Track. If false, the <see cref="IMediaStream">MediaStream</see> will not contain a video Track.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints-video" />
    public bool Video { get; set; }

    /// <summary>
    /// Optional detailed video constraints when <see cref="Video" /> is enabled.
    /// </summary>
    public MediaTrackConstraints? VideoConstraints { get; set; }

    /// <summary>
    /// Initializes stream constraints using boolean audio and video toggles.
    /// </summary>
    /// <param name="audio"><see langword="true" /> to request an audio track.</param>
    /// <param name="video"><see langword="true" /> to request a video track.</param>
    public MediaStreamConstraints(bool audio, bool video)
    {
        Audio = audio;
        AudioConstraints = null;

        Video = video;
        VideoConstraints = null;
    }

    /// <summary>
    /// Initializes stream constraints with detailed audio constraints and a video toggle.
    /// </summary>
    /// <param name="audio">Detailed audio constraints, or <see langword="null" /> for none.</param>
    /// <param name="video"><see langword="true" /> to request a video track.</param>
    public MediaStreamConstraints(MediaTrackConstraints? audio, bool video)
    {
        Audio = audio != null;
        AudioConstraints = audio;

        Video = video;
        VideoConstraints = null;
    }

    /// <summary>
    /// Initializes stream constraints with an audio toggle and detailed video constraints.
    /// </summary>
    /// <param name="audio"><see langword="true" /> to request an audio track.</param>
    /// <param name="video">Detailed video constraints, or <see langword="null" /> for none.</param>
    public MediaStreamConstraints(bool audio, MediaTrackConstraints? video)
    {
        Audio = audio;
        AudioConstraints = null;

        Video = video != null;
        VideoConstraints = video;
    }

    /// <summary>
    /// Initializes stream constraints with detailed audio and video constraints.
    /// </summary>
    /// <param name="audio">Detailed audio constraints.</param>
    /// <param name="video">Detailed video constraints.</param>
    public MediaStreamConstraints(MediaTrackConstraints? audio, MediaTrackConstraints? video)
    {
        Audio = audio != null;
        AudioConstraints = audio;

        Video = video != null;
        VideoConstraints = video;
    }
}