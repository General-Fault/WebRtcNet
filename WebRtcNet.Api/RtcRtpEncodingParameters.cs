using WebRtcNet.Media;

namespace WebRtcNet
{
    /// <summary>
    /// RTP encoding parameters.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters"/>
    public class RtcRtpEncodingParameters
    {
        public RtcRtpEncodingParameters(bool active, ulong? maxBitrate, double scaleResolutionDownBy)
        {
            Active = active;
            MaxBitrate = maxBitrate;
            ScaleResolutionDownBy = scaleResolutionDownBy;
        }

        /// <summary>
        /// Indicates that this encoding is actively being sent. Setting it to false causes this encoding to no longer be sent.
        /// Setting it to true causes this encoding to be sent. Since setting the value to false does not cause the SSRC to be
        /// removed, an RTCP BYE is not sent.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-active"/>
        public bool Active { get; }

        /// <summary>
        /// When present, indicates the maximum bitrate that can be used to send this encoding. The user agent is free to
        /// allocate bandwidth between the encodings, as long as the maxBitrate value is not exceeded. The encoding may also
        /// be further constrained by other limits (such as per-transport or per-session bandwidth limits) below the maximum
        /// specified here. MaxBitrate is computed the same way as the Transport Independent Application Specific Maximum
        /// (TIAS) bandwidth defined in
        /// <see href="https://datatracker.ietf.org/doc/html/rfc3890#section-6.2.2">[RFC3890] Section 6.2.2</see>, which is
        /// the maximum bandwidth needed without counting IP or other transport layers like TCP or UDP. The unit of maxBitrate
        /// is bits per second.
        /// <para>NOTE: How the bitrate is achieved is media and encoding dependent.For video, a frame will always be sent as
        /// fast as possible, but frames may be dropped until bitrate is low enough. Thus, even a bitrate of zero will allow
        /// sending one frame. For audio, it might be necessary to stop playing if the bitrate does not allow the chosen
        /// encoding enough bandwidth to be sent.</para>
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpencodingparameters-maxbitrate"/>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3890"/>
        public ulong? MaxBitrate { get; }

        /// <summary>
        /// This member is only present if the sender's <see cref="IMediaStreamTrack.Kind">kind</see> is
        /// <see cref="MediaStreamTrackKind.Video">video</see>. The video's resolution will be scaled down in each dimension
        /// by the given value before sending. For example, if the value is 2.0, the video will be scaled down by a factor of
        /// 2 in each dimension, resulting in sending a video of one quarter the size. If the value is 1.0, the video will not
        /// be affected. The value must be greater than or equal to 1.0. By default, scaling is applied by a factor of two to
        /// the power of the layer's number, in order of smaller to higher resolutions, e.g. 4:2:1. If there is only one layer,
        /// the sender will by default not apply any scaling, (i.e. ScaleResolutionDownBy will be 1.0).
        /// </summary>
        public double ScaleResolutionDownBy { get; }
    }
}