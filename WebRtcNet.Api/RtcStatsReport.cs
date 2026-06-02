using System;
using System.Collections.Generic;

namespace WebRtcNet;

/// <summary>
/// Identifies the concrete stats dictionary type represented by an <see cref="RtcStats" /> instance.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcstats-type" />
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
/// Base class for all WebRTC stats snapshot dictionaries. Stats are out-only immutable snapshots
/// produced by the platform; they are not created by application code.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc-stats/#rtcstats-dictionary" />
public abstract record RtcStats
{
	/// <summary>
	/// The timestamp, of type DOMHiResTimeStamp [HIGHRES-TIME], associated with this object.
	/// The time is relative to the UNIX epoch (Jan 1, 1970, UTC).
	/// </summary>
	public TimeSpan Timestamp { get; init; }

	/// <summary>
	/// The type of this object.
	/// </summary>
	public RtcStatsType Type { get; init; }

	/// <summary>
	/// A unique id that is associated with the object that was inspected to produce this RTCStats object. 
	/// </summary>
	public string Id { get; init; } = string.Empty;
}

/// <summary>
/// Base dictionary for RTP stream statistics objects.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcrtpstreamstats" />
public abstract record RtcRtpStreamStats : RtcStats
{
	/// <summary>
	/// Gets the synchronization source identifier (SSRC) for the RTP stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcrtpstreamstats-ssrc" />
	public string Ssrc { get; init; } = string.Empty;

	/// <summary>
	/// The kind of the RTP stream, one of "audio" or "video".
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcrtpstreamstats-kind" />
	public string Kind { get; init; } = string.Empty;

	/// <summary>
	/// The TransportId identifies the transport used to send or receive this RTP stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcrtpstreamstats-transportid" />
	public string TransportId { get; init; } = string.Empty;

	/// <summary>
	/// The CodecId identifies the RTP codec used by the RTP stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcrtpstreamstats-codecid" />
	public string CodecId { get; init; } = string.Empty;

	/// <summary>
	/// The remoteId can be used to look up the corresponding RTCStats object that represents stats reported by the other peer.
	/// </summary>
	public string RemoteId { get; init; } = string.Empty;
}

/// <summary>
/// Statistics for an inbound RTP stream.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcinboundrtpstreamstats" />
public sealed record RtcInboundRtpStreamStats : RtcRtpStreamStats
{
	/// <summary>
	/// Gets the number of packets received for this stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcinboundrtpstreamstats-packetsreceived" />
	public ulong PacketsReceived { get; init; }

	/// <summary>
	/// Gets the number of bytes received for this stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcinboundrtpstreamstats-bytesreceived" />
	public ulong BytesReceived { get; init; }
}

/// <summary>
/// Statistics for an outbound RTP stream.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcoutboundrtpstreamstats" />
public sealed record RtcOutboundRtpStreamStats : RtcRtpStreamStats
{
	/// <summary>
	/// Gets the number of packets sent for this stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcoutboundrtpstreamstats-packetssent" />
	public ulong PacketsSent { get; init; }

	/// <summary>
	/// Gets the number of bytes sent for this stream.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc-stats/#dom-rtcoutboundrtpstreamstats-bytessent" />
	public ulong BytesSent { get; init; }
}

/// <summary>
/// The set of supported property names <see href="http://www.w3.org/TR/WebIDL-1/">[WEBIDL]</see> is defined 
/// as the ids of all the RTCStats-derived dictionaries that have been generated for this stats report.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc-stats/#rtcstatsreport-object" />
public interface RtcStatsReport : IReadOnlyDictionary<string, RtcStats>
{
};