using System.Collections.Generic;
using System.Linq;

namespace WebRtcNet
{
    /// <summary>
    /// Base class for <see cref="RtcRtpSendParameters"/> and <see cref="RtcRtpReceiveParameters"/>
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpparameters"/>
    public class RtcRtpParameters {
        private readonly IList<RtcRtpHeaderExtensionParameters> _headerExtensions;
        private readonly IList<RtcRtpCodecParameters> _codecs;

        public RtcRtpParameters(IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions, RtcRtcpParameters rtcp, IEnumerable<RtcRtpCodecParameters> codecs)
        {
            _headerExtensions = headerExtensions.ToList();
            Rtcp = rtcp;
            _codecs = codecs.ToList();
        }

        /// <summary>
        /// A sequence containing parameters for RTP header extensions. 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpparameters-headerextensions"/>
        public IEnumerable<RtcRtpHeaderExtensionParameters> HeaderExtensions => _headerExtensions;

        /// <summary>
        /// Parameters used for RTCP.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpparameters-rtcp"/>
        public RtcRtcpParameters Rtcp { get; }

        /// <summary>
        /// A sequence containing the media codecs that an <see cref="IRtcRtpSender">RTCRtpSender</see> will choose from, as
        /// well as entries for RTX, RED and FEC mechanisms. Corresponding to each media codec where retransmission via RTX is
        /// enabled, there will be an entry in codecs with a <see cref="RtcRtpCodecParameters.MimeType"/> property indicating
        /// retransmission via audio/rtx video/rtx, and an <see cref="RtcRtpCodecParameters.SdpFmtpLine"/> property (providing
        /// the "apt" and "rtx-time" parameters).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpparameters-codecs"/>
        public IEnumerable<RtcRtpCodecParameters> Codecs => _codecs;
    };


    /// <summary>
    /// RTP parameters for a <see cref="IRtcRtpSender">sender</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcsendrtpparameters"/>
    public class RtcRtpSendParameters : RtcRtpParameters
    {
        private readonly IList<RtcRtpEncodingParameters> _encodings;

        public RtcRtpSendParameters(
            IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions,
            RtcRtcpParameters rtcp,
            IEnumerable<RtcRtpCodecParameters> codecs, 
            string transactionId, IEnumerable<RtcRtpEncodingParameters> encodings)
            : base(headerExtensions, rtcp, codecs)
        {
            TransactionId = transactionId;
            _encodings = encodings.ToList();
        }

        /// <summary>
        /// A unique identifier for the last set of parameters applied. Ensures that setParameters can only be called based on
        /// a previous <see cref="IRtcRtpSender.GetParameters">GetParameters</see>, and that there are no intervening changes. 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsendparameters-transactionid"/>
        public string TransactionId { get; }

        /// <summary>
        /// A sequence containing parameters for RTP encodings of media.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsendparameters-encodings"/>
        public IEnumerable<RtcRtpEncodingParameters> Encodings => _encodings;
    }

    /// <summary>
    /// RTP parameters for a <see cref="IRtcRtpReceiver">receiver</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcreceivertpparameters"/>
    public class RtcRtpReceiveParameters : RtcRtpParameters
    {
        public RtcRtpReceiveParameters(
            IEnumerable<RtcRtpHeaderExtensionParameters> headerExtensions,
            RtcRtcpParameters rtcp,
            IEnumerable<RtcRtpCodecParameters> codecs)
            : base(headerExtensions, rtcp, codecs)
        { }
    }
}