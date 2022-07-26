using System;

namespace WebRtcNet.Media
{
    /// <summary>
    /// Constraints for the MediaTrack. 
    /// </summary>
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-constraints"/>
    /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/MediaTrackConstraints"/>
    /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Media_Streams_API/Constraints"/>
    public class MediaTrackConstraints
    {
        #region ConstraintTypes
        public class Constraint<T> where T : struct
        {
            public T? Ideal;
            public T? Exact;

            public Constraint(T value)
            {
                Exact = value;
            }

            public static implicit operator T(Constraint<T> from)
            {
                return from.Exact ?? from.Ideal.GetValueOrDefault();
            }

            public static implicit operator Constraint<T>(T from)
            {
                return new Constraint<T>(from);
            }
        }

        public class StringConstraint
        {
            public string Ideal;
            public string Exact;

            public StringConstraint(string value)
            {
                Exact = value;
            }

            public static implicit operator string (StringConstraint from)
            {
                return from.Exact ?? from.Ideal;
            }

            public static implicit operator StringConstraint(string from)
            {
                return new StringConstraint(from);
            }
        }

        public class RangeConstraint<T> where T : struct
        {
            public T? Ideal { get; set; }
            public T? Exact { get; set; }
            public T? Min { get; set; }
            public T? Max { get; set; }

            public RangeConstraint(T value)
            {
                Exact = value;
            }

            public RangeConstraint(Constraint<T> value)
            {
                Exact = value.Exact;
                Ideal = value.Ideal;
            }

            public RangeConstraint(T min, T max, T ideal)
            {
                Min = min;
                Max = max;
                Ideal = ideal;
            }

            public RangeConstraint(ValueRange<T> valueRange)
            {
                Min = valueRange.Min;
                Max = valueRange.Max;
                if (Max.Equals(Min))
                {
                    Exact = Max;
                }
            }

            public static implicit operator T(RangeConstraint<T> from)
            {
                return from.Exact ?? from.Ideal ?? from.Min ?? from.Max ?? default(T);
            }

            public static implicit operator RangeConstraint<T>(T from)
            {
                return new RangeConstraint<T>(from);
            }

        }
        #endregion ConstraintTypes

        /// <summary>
        /// The width or width range, in pixels. As a capability, the range should span the video source's pre-set width
        /// values with min being equal to 1 and max being the largest width.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-width"/>
        public RangeConstraint<ulong> Width { get; set; }

        /// <summary>
        /// The width or width range, in pixels. As a capability, the range should span the video source's pre-set width
        /// values with min being equal to 1 and max being the largest width.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-height"/>
        public RangeConstraint<ulong>  Height { get; set; }

        /// <summary>
        /// The exact aspect ratio (width in pixels divided by height in pixels, represented as a double rounded to the tenth decimal place)
        /// or aspect ratio range.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-aspect"/>
        public RangeConstraint<double>  AspectRatio { get; set; }

        /// <summary>
        /// The exact frame rate (frames per second) or frame rate range. If video source's pre-set can determine frame rate
        /// values, the range, as a capacity, should span the video source's pre-set frame rate values with min being equal to
        /// 0 and max being the largest frame rate.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-frameRate"/>
        public RangeConstraint<double>  FrameRate { get; set; }

        /// <summary>
        /// The directions that the camera can face, as seen from the user's perspective. 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-facingMode"/>
        public Constraint<VideoFacingModes> FacingMode { get; set; }

        /// <summary>
        /// The  means by which the resolution can be derived by the application. In other words, whether the application is
        /// allowed to use cropping and down-scaling on the camera output.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-resizeMode"/>
        public Constraint<VideResizeModes> ResizeMode { get; set; }

        /// <summary>
        /// The sample rate in samples per second for the audio data.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleRate"/>
        public RangeConstraint<ulong>  SampleRate { get; set; }

        /// <summary>
        /// The linear sample size in bits. This constraint can only be satisfied for audio devices that produce linear
        /// samples.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-sampleSize"/>
        public RangeConstraint<ulong>  SampleSize { get; set; }

        /// <summary>
        /// When one or more audio streams is being played in the processes of various microphones, it is often desirable to
        /// attempt to remove all the sound being played from the input signals recorded by the microphones. This is referred
        /// to as echo cancellation. There are cases where it is not needed and it is desirable to turn it off so that no
        /// audio artifacts are introduced. This allows applications to control this behavior.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation"/>
        public Constraint<bool> EchoCancellation { get; set; }

        /// <summary>
        /// Automatic gain control is often desirable on the input signal recorded by the microphone. There are cases where it
        /// is not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
        /// control this behavior.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-autoGainControl"/>
        public Constraint<bool> AutoGainControl { get; set; }

        /// <summary>
        /// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is
        /// not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
        /// control this behavior.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-noiseSuppression"/>
        public Constraint<bool> NoiseSuppression { get; set; }

        /// <summary>
        /// Noise suppression is often desirable on the input signal recorded by the microphone. There are cases where it is
        /// not needed and it is desirable to turn it off so that the audio is not altered. This allows applications to
        /// control this behavior.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-latency"/>
        public RangeConstraint<double>  Latency { get; set; }

        /// <summary>
        /// The number of independent channels of sound that the audio data contains, i.e. the number of audio samples per
        /// sample frame.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-channelCount"/>
        public RangeConstraint<ulong>  ChannelCount { get; set; }

        /// <summary>
        /// The identifier of the device generating the content of the <see cref="IMediaStreamTrack">MediaStreamTrack</see>.
        /// It conforms with the definition of <see cref="MediaDeviceInfo.DeviceId">MediaDeviceInfo.DeviceId</see>. Note that
        /// the setting of this property is uniquely determined by the source that is attached to the
        /// <see cref="IMediaStreamTrack">MediaStreamTrack</see>. In particular,
        /// <see cref="IMediaStreamTrack.GetCapabilities">GetCapabilities()</see> will return only a single value for
        /// DeviceId. This property can therefore be used for initial media selection with
        /// <see cref="IMediaDevices.GetUserMedia">GetUserMedia()</see>. However, it is not useful for subsequent media
        /// control with <see cref="IMediaStreamTrack.ApplyConstraints">ApplyConstraints()</see>, since any attempt to set a
        /// different value will result in an unsatisfiable ConstraintSet. If a string of length 0 is used as a DeviceId value
        /// constraint with <see cref="IMediaDevices.GetUserMedia">GetUserMedia()</see>, it MAY be interpreted as if the
        /// constraint is not specified.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-deviceId"/>
        public StringConstraint DeviceId { get; set; }

        /// <summary>
        /// The application-unique group identifier for the device generating the content of the
        /// <see cref="IMediaStreamTrack">MediaStreamTrack</see>. It conforms with the definition of
        /// <see cref="MediaDeviceInfo.GroupId">MediaDeviceInfo.GroupId</see>. Note that the setting of this property is
        /// uniquely determined by the source that is attached to the <see cref="IMediaStreamTrack">MediaStreamTrack</see>.
        /// In particular, <see cref="IMediaStreamTrack.GetCapabilities">GetCapabilities()</see> will return only a single
        /// value for groupId. Since this property is not stable between browsing sessions, its usefulness for initial media
        /// selection with <see cref="IMediaDevices.GetUserMedia">GetUserMedia()</see> is limited. It is not useful for
        /// subsequent media control with <see cref="IMediaStreamTrack.ApplyConstraints">ApplyConstraints()</see>, since any
        /// attempt to set a different value will result in an unsatisfiable ConstraintSet.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-groupId"/>
        public StringConstraint GroupId { get; set; }
    };
}
