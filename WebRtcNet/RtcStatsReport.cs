using System;
using System.Collections.Generic;

namespace WebRtcNet
{

    public enum RtcStatsType
    {
        /// <summary>
        /// Inbound RTP.
        /// </summary>
        InboundRtp,

        /// <summary>
        /// Outbound RTP.
        /// </summary>
        OutboundRtp
    };

    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#rtcstats-dictionary"/>
    /// </summary>
    public abstract class RtcStats
    {
        /// The timestamp, of type DOMHiResTimeStamp[HIGHRES - TIME], associated with this object.
        /// The time is relative to the UNIX epoch(Jan 1, 1970, UTC).
        public virtual TimeSpan Timestamp { get; }


        /// The type of this object.
        public virtual RtcStatsType Type { get; }


        /// A unique id that is associated with the object that was inspected to produce this RTCStats object. 
        public virtual string Id { get; }
    }

    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#dictionary-rtcrtpstreamstats-members"/>
    /// </summary>
    public class RtcRtpStreamStats : RtcStats
    {
        public virtual string Src { get; }

        /// <summary>
        /// The remoteId can be used to look up the corresponding RTCStats object that represents stats reported by the other peer.
        /// </summary>
        public virtual string RemoteId { get; }
    }


    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#dictionary-rtcinboundrtpstreamstats-members"/>
    /// </summary>
    public class RtcInboundRtpStreamStats : RtcRtpStreamStats
    {
        public virtual uint PacketsSent { get; }
        public virtual uint BytesSent { get; }
    }

    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#dictionary-rtcoutboundrtpstreamstats-members"/>
    /// </summary>
    public class RtcOutboundRtpStreamStats : RtcRtpStreamStats
    {
        public virtual int PacketsSent { get; }
        public virtual int BytesSent { get; }
    }

    /// <summary>
    /// <seealso cref="http://www.w3.org/TR/webrtc/#idl-def-RTCStatsReport"/>
    /// The set of supported property names <seealso cref="http://www.w3.org/TR/WebIDL-1/">[WEBIDL]</seealso> is defined 
    /// as the ids of all the RTCStats-derived dictionaries that have been generated for this stats report.
    /// </summary>
    public interface IRtcStatsReport : IReadOnlyDictionary<string, RtcStats>
    {
    };
}
