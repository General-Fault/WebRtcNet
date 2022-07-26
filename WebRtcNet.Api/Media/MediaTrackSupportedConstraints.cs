namespace WebRtcNet.Media
{
    /// <summary>
    /// MediaTrackSupportedConstraints represents the list of constraints recognized by WebRtcNet for controlling the
    /// Capabilities of a MediaStreamTrack object. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatracksupportedconstraints"/>
    public struct MediaTrackSupportedConstraints
    {
        /// <see cref="MediaTrackConstraints.Width"/>
        public bool Width;

        /// <see cref="MediaTrackConstraints.Height"/>
        public bool Height;

        /// <see cref="MediaTrackConstraints.AspectRatio"/>
        public bool AspectRatio;

        /// <see cref="MediaTrackConstraints.FrameRate"/>
        public bool FrameRate;

        /// <see cref="MediaTrackConstraints.FacingMode"/>
        public bool FacingMode;

        /// <see cref="MediaTrackConstraints.ResizeMode"/>
        public bool ResizeMode;

        /// <see cref="MediaTrackConstraints.SampleRate"/>
        public bool SampleRate;

        /// <see cref="MediaTrackConstraints.SampleSize"/>
        public bool SampleSize;

        /// <see cref="MediaTrackConstraints.EchoCancellation"/>
        public bool EchoCancellation;

        /// <see cref="MediaTrackConstraints.AutoGainControl"/>
        public bool AutoGainControl;

        /// <see cref="MediaTrackConstraints.NoiseSuppression"/>
        public bool NoiseSuppression;

        /// <see cref="MediaTrackConstraints.Latency"/>
        public bool Latency;

        /// <see cref="MediaTrackConstraints.ChannelCount"/>
        public bool ChannelCount;

        /// <see cref="MediaTrackConstraints.DeviceId"/>
        public bool DeviceId;

        /// <see cref="MediaTrackConstraints.GroupId"/>
        public bool GroupId;
    };
}

