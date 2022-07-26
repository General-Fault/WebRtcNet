namespace WebRtcNet.Media
{
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
    /// MediaTrackCapabilities represents the Capabilities of an IMediaStreamTrack object.
    /// </summary>
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-capabilities"/>
    public struct MediaTrackCapabilities
    {
	    public ValueRange<int> Width;
        public ValueRange<int> Height;
        public ValueRange<double> AspectRatio;
        public ValueRange<double> FrameRate;
	    public VideoFacingModes? FacingMode;
        public VideResizeModes? ResizeMode;
        public ValueRange<int> SampleRate;
        public ValueRange<int> SampleSize;
        public bool[] EchoCancellation;
        public bool[] AutoGainControl;
        public bool[] NoiseSupression;
        public ValueRange<double> Latency;
        public ValueRange<double> ChannelCount;
        public string DeviceId;
        public string GroupId;
    };
}
