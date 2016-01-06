namespace WebRtcNet
{
    /// <summary>
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#idl-def-VideoFacingModeEnum"/>
    /// </summary>
    public enum FacingModes { user, environment, left, right }

    /// <summary>
    /// MediaTrackCapabilities represents the Capabilities of an IMediaStreamTrack object.
    /// <seealso href="http://www.w3.org/TR/mediacapture-streams/#media-track-capabilities"/>
    /// </summary>
    public struct MediaTrackCapabilities
    {
	    public ValueRange<int> Width;
        public ValueRange<int> Height;
        public ValueRange<double> AspectRatio;
        public ValueRange<double> FrameRate;
	    public FacingModes? FacingMode;
        public ValueRange<double> Volume;
        public ValueRange<int> SampleRate;
        public ValueRange<int> SampleSize;
        public bool[] EchoCancellation;
        public ValueRange<double> Latency;
        public ValueRange<double> ChannelCount;
        public string DeviceId;
        public string GroupId;
    };

    public class ValueRange<T> where T :struct
    {
        public T Max;
        public T Min;

        public ValueRange(T value)
        {
            Min = Max = value;
        }

        public static implicit operator T(ValueRange<T> from)
        {
            return from.Max;
        }

        public static implicit operator ValueRange<T>(T from)
        {
            return new ValueRange<T>(from);
        }
    }
}
