namespace WebRtcNet
{
    /// <summary>
    /// RTCP Parameters. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtcpparameterss"/>
    /// <seealso cref="RtcRtpParameters"/>
    public class RtcRtcpParameters
    {
        public RtcRtcpParameters(string cName, bool reducedSize)
        {
            CName = cName;
            ReducedSize = reducedSize;
        }

        /// <summary>
        /// The Canonical Name (CNAME) used by RTCP (e.g. in SDES messages).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtcpparameters-cname"/>
        private string CName { get; }

        /// <summary>
        /// Whether reduced size RTCP <see href="https://tools.ietf.org/html/rfc5506">[RFC5506]</see> is configured (if true)
        /// or compound RTCP as specified in  <see href="https://tools.ietf.org/html/rfc3550">[RFC3550]</see> (if false). 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtcpparameters-reducedsize"/>
        /// <seealso href="https://tools.ietf.org/html/rfc5506"/>
        /// <seealso href="https://tools.ietf.org/html/rfc3550"/>
        private bool ReducedSize { get; }
    }
}