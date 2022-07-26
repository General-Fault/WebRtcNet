namespace WebRtcNet.Media
{
    /// <summary>
    /// Contains the MediaTrackConstraints for both the audio and video tracks in a media stream.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#mediastreamconstraints"/>
    public class MediaStreamConstraints
    {
        /// <summary>
        /// If true, it requests that the returned <see cref="IMediaStream">MediaStream</see> contain a audio track. If a
        /// <see cref="MediaTrackConstraints"/> structure is provided, it further specifies the nature and settings of the
        /// audio Track. If false, the <see cref="IMediaStream">MediaStream</see> will not contain an audio Track.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints-audio"/>
        public bool Audio { get; set; }

        /// <seealso cref="Audio"/>
        public MediaTrackConstraints AudioConstraints { get; set; }

        /// <summary>
        /// If true, it requests that the returned <see cref="IMediaStream">MediaStream</see> contain a video track. If a
        /// <see cref="MediaTrackConstraints"/> structure is provided, it further specifies the nature and settings of the
        /// video Track. If false, the <see cref="IMediaStream">MediaStream</see> will not contain a video Track.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediastreamconstraints-video"/>
        public bool Video { get; set; }

        /// <seealso cref="Audio"/>

        #region Constructors
        public MediaTrackConstraints VideoConstraints { get; set; }

        public MediaStreamConstraints(bool audio, bool video)
        {
            Audio = audio;
            AudioConstraints = null;

            Video = video;
            VideoConstraints = null;
        }

        public MediaStreamConstraints(MediaTrackConstraints audio, bool video)
        {
            Audio = audio != null;
            AudioConstraints = audio;

            Video = video;
            VideoConstraints = null;
        }

        public MediaStreamConstraints(bool audio, MediaTrackConstraints video)
        {
            Audio = audio;
            AudioConstraints = null;

            Video = video != null;
            VideoConstraints = video;
        }

        public MediaStreamConstraints(MediaTrackConstraints audio, MediaTrackConstraints video)
        {
            Audio = true;
            AudioConstraints = audio;

            Video = true;
            VideoConstraints = video;
        }
        #endregion Constructors
    }
}