namespace WebRtcNet
{
    /// <summary>
    /// Describes a codec.
    /// </summary>
    /// <seealso cref="RtcRtpParameters.Codecs"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpcodecparameters"/>
    public class RtcRtpCodecParameters
    {
        public RtcRtpCodecParameters(byte payloadType, string mimeType, ulong clockRate, short channels, string sdpFmtpLine)
        {
            PayloadType = payloadType;
            MimeType = mimeType;
            ClockRate = clockRate;
            Channels = channels;
            SdpFmtpLine = sdpFmtpLine;
        }

        /// <summary>
        /// The RTP payload type used to identify this codec.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodecparameters-payloadtype"/>
        public byte PayloadType { get; }

        /// <summary>
        /// The codec MIME media type/subtype. Valid media types and subtypes are listed in [IANA-RTP-2].
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodecparameters-mimetype"/>
        public string MimeType { get; }

        /// <summary>
        /// The codec clock rate expressed in Hertz.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodecparameters-clockrate"/>
        public ulong ClockRate { get; }

        /// <summary>
        /// When present, indicates the number of channels (mono=1, stereo=2).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodecparameters-channels"/>
        public short Channels { get; }

        /// <summary>
        /// The "format specific parameters" field from the a=fmtp line in the SDP corresponding to the codec, if one exists,
        /// as defined by <see href="https://tools.ietf.org/html/rfc8829#section-5.8">[RFC8829] (section 5.8.)</see>.
        /// For an <see cref="IRtcRtpSender">RTCRtpSender</see>, these parameters come from the remote description, and for an
        /// <see cref="IRtcRtpReceiver">RTCRtpReceiver</see>, they come from the local description. 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodecparameters-sdpfmtpline"/>
        /// <seealso href="https://tools.ietf.org/html/rfc8829#section-5.8"/>
        public string SdpFmtpLine { get; }
    }
}