namespace WebRtcNet
{

    public enum RtcSdpType
    {
        /// <summary>
        /// An RTCSdpType of "offer" indicates that a description must be treated as an [SDP] offer.
        /// </summary>
        Offer,

        /// <summary>
        /// An RTCSdpType of "pranswer" indicates that a description must be treated as an [SDP]
        /// answer, but not a final answer. A description used as an SDP "pranswer" may be applied as a 
        /// response to a SDP offer, or an update to a previously sent SDP "pranswer".
        /// </summary>
        PrAnswer,

        /// <summary>
        /// An RtcSdpType of "answer" indicates that a description must be treated as an [SDP]
        /// final answer, and the offer-answer exchange must be considered complete. A description used as an 
        /// SDP answer may be applied as a response to an SDP offer or as an update to a previously sent SDP "pranswer".
        /// </summary>
        Answer,
    }

    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#rtcsessiondescription-class"/>
    /// The RtcSessionDescription is used by IRtcPeerConnection to expose local and remote session descriptions. 
    /// Attributes on this struct are mutable for legacy reasons.
    /// </summary>
    public struct RtcSessionDescription
    {
        public RtcSessionDescription(RtcSdpType type, string sdp)
        {
            Type = type;
            Sdp = sdp;
        }

        RtcSdpType Type;
        string Sdp;
    }
}
