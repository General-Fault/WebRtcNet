namespace WebRtcNet
{
    /// <summary>
    /// This describes an ICE candidate.
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcicecandidate-type"/>
    /// </summary>
    public struct RtcIceCandidate
    {
        /// <summary>
        /// his carries the candidate-attribute as defined in section 15.1 of 
        /// <seealso href="https://tools.ietf.org/html/rfc5245">[ICE]</seealso>.
        /// </summary>
        public string Candidate;

        /// <summary>
        /// If present, this contains the identifier of the "media stream identification" as 
        /// defined in <seealso href="https://tools.ietf.org/html/rfc3388">[RFC3388]</seealso> for the media section this candidate is associated with.
        /// </summary>
        public string SdpMid;

        /// <summary>
        /// This indicates the index (starting at zero) of the m-line in the SDP this candidate is associated with.
        /// </summary>
        public ushort SdpMLineIndex;
    }
}
