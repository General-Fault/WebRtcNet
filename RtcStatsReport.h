#pragma once


WEBRTCNET_START

public enum RtcStatsType
{
	/// Inbound RTP.
	InboundRtp,

	/// Outbound RTP.
	OutboundRtp
};

public ref class RtcStats
{
	/// The timestamp, of type DOMHiResTimeStamp[HIGHRES - TIME], associated with this object.
	/// The time is relative to the UNIX epoch(Jan 1, 1970, UTC).
	property TimeSpan  Timestamp { TimeSpan get() { throw gcnew NotImplementedException(); } }

	/// The type of this object.
	property RtcStatsType Type { RtcStatsType get() { throw gcnew NotImplementedException(); } }

	/// A unique id that is associated with the object that was inspected to produce this RTCStats object. 
	property String ^ Id { String ^ get() { throw gcnew NotImplementedException(); } }
};

public ref class RtcRtpStreamStats : RtcStats
{
	property String ^ Src { String ^ get() { throw gcnew NotImplementedException(); } }

	/// The remoteId can be used to look up the corresponding RTCStats object that represents stats reported by the other peer.
	property String ^ RemoteId { String ^ get() { throw gcnew NotImplementedException(); } }
};


public ref class RtcInboundRtpStreamStats : RtcRtpStreamStats
{
	property UInt32 PacketsSent { UInt32 get() { throw gcnew NotImplementedException(); } }
	property UInt32 BytesSent { UInt32 get() { throw gcnew NotImplementedException(); } }
};

public ref class RtcOutboundRtpStreamStats : RtcRtpStreamStats
{
	property UInt32 PacketsSent { UInt32 get() { throw gcnew NotImplementedException(); } }
	property UInt32 BytesSent { UInt32 get() { throw gcnew NotImplementedException(); } }
};

public ref class RtcStatsReport abstract
{
public:
	RtcStats ^ RtcStats(String^ id) { throw gcnew NotImplementedException(); }
};

WEBRTCNET_END