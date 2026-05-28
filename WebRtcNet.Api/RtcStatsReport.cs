using System;
using System.Collections.Generic;

namespace WebRtcNet;

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
/// <seealso href="http://www.w3.org/TR/webrtc/#rtcstats-dictionary"/>
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
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#dictionary-rtcrtpstreamstats-members"/>
public abstract record RtcRtpStreamStats : RtcStats
{
	public string Src { get; init; } = string.Empty;

	/// <summary>
	/// The remoteId can be used to look up the corresponding RTCStats object that represents stats reported by the other peer.
	/// </summary>
	public string RemoteId { get; init; } = string.Empty;
}

/// <summary>
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#dictionary-rtcinboundrtpstreamstats-members"/>
public sealed record RtcInboundRtpStreamStats : RtcRtpStreamStats
{
	public uint PacketsSent { get; init; }
	public uint BytesSent { get; init; }
}

/// <summary>
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#dictionary-rtcoutboundrtpstreamstats-members"/>
public sealed record RtcOutboundRtpStreamStats : RtcRtpStreamStats
{
	public int PacketsSent { get; init; }
	public int BytesSent { get; init; }
}

/// <summary>
/// The set of supported property names <see href="http://www.w3.org/TR/WebIDL-1/">[WEBIDL]</see> is defined 
/// as the ids of all the RTCStats-derived dictionaries that have been generated for this stats report.
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#idl-def-RTCStatsReport"/>
public interface IRtcStatsReport : IReadOnlyDictionary<string, RtcStats>
{
};