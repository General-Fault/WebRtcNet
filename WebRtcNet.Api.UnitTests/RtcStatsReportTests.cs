using System;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcRtpStreamStatsTests
{
	[Test]
	public void RtcRtpStreamStats_Ssrc_Exists()
	{
		var property = typeof(RtcRtpStreamStats).GetProperty("Ssrc");
		Assert.IsNotNull(property, "Ssrc property should exist on RtcRtpStreamStats");
	}

	[Test]
	public void RtcRtpStreamStats_Ssrc_Renamed_From_Src()
	{
		// Ensure old 'Src' property no longer exists
		var srcProperty = typeof(RtcRtpStreamStats).GetProperty("Src");
		Assert.IsNull(srcProperty, "Src property should be renamed to Ssrc");
	}

	[Test]
	public void RtcRtpStreamStats_Kind_Property_Exists()
	{
		var property = typeof(RtcRtpStreamStats).GetProperty("Kind");
		Assert.IsNotNull(property, "Kind property should exist on RtcRtpStreamStats");
		Assert.AreEqual(typeof(string), property!.PropertyType, "Kind should be string");
	}

	[Test]
	public void RtcRtpStreamStats_TransportId_Property_Exists()
	{
		var property = typeof(RtcRtpStreamStats).GetProperty("TransportId");
		Assert.IsNotNull(property, "TransportId property should exist on RtcRtpStreamStats");
		Assert.AreEqual(typeof(string), property!.PropertyType, "TransportId should be string");
	}

	[Test]
	public void RtcRtpStreamStats_CodecId_Property_Exists()
	{
		var property = typeof(RtcRtpStreamStats).GetProperty("CodecId");
		Assert.IsNotNull(property, "CodecId property should exist on RtcRtpStreamStats");
		Assert.AreEqual(typeof(string), property!.PropertyType, "CodecId should be string");
	}

	[Test]
	public void RtcRtpStreamStats_String_Properties_Default_To_Empty()
	{
		var stats = new RtcInboundRtpStreamStats();

		Assert.AreEqual(string.Empty, stats.Kind);
		Assert.AreEqual(string.Empty, stats.TransportId);
		Assert.AreEqual(string.Empty, stats.CodecId);
	}
}

[TestFixture]
public class RtcInboundRtpStreamStatsTests
{
	[Test]
	public void RtcInboundRtpStreamStats_PacketsReceived_Exists()
	{
		var property = typeof(RtcInboundRtpStreamStats).GetProperty("PacketsReceived");
		Assert.IsNotNull(property, "PacketsReceived property should exist on RtcInboundRtpStreamStats");
	}

	[Test]
	public void RtcInboundRtpStreamStats_BytesReceived_Exists()
	{
		var property = typeof(RtcInboundRtpStreamStats).GetProperty("BytesReceived");
		Assert.IsNotNull(property, "BytesReceived property should exist on RtcInboundRtpStreamStats");
	}

	[Test]
	public void RtcInboundRtpStreamStats_No_PacketsSent()
	{
		var property = typeof(RtcInboundRtpStreamStats).GetProperty("PacketsSent");
		Assert.IsNull(property, "PacketsSent should not exist on RtcInboundRtpStreamStats (inbound should not send)");
	}

	[Test]
	public void RtcInboundRtpStreamStats_No_BytesSent()
	{
		var property = typeof(RtcInboundRtpStreamStats).GetProperty("BytesSent");
		Assert.IsNull(property, "BytesSent should not exist on RtcInboundRtpStreamStats (inbound should not send)");
	}

	[Test]
	public void RtcInboundRtpStreamStats_PacketsReceived_Type()
	{
		var property = typeof(RtcInboundRtpStreamStats).GetProperty("PacketsReceived");
		Assert.AreEqual(typeof(ulong), property!.PropertyType, "PacketsReceived should be of type ulong");
	}

	[Test]
	public void RtcInboundRtpStreamStats_BytesReceived_Type()
	{
		var property = typeof(RtcInboundRtpStreamStats).GetProperty("BytesReceived");
		Assert.AreEqual(typeof(ulong), property!.PropertyType, "BytesReceived should be of type ulong");
	}

	[Test]
	public void RtcInboundRtpStreamStats_Create_With_Values()
	{
		var stats = new RtcInboundRtpStreamStats
		{
			Timestamp = TimeSpan.FromMilliseconds(100),
			Type = RtcStatsType.InboundRtp,
			Id = "inbound-123",
			Ssrc = "3876931414",
			Kind = "audio",
			TransportId = "transport-1",
			CodecId = "codec-audio-1",
			PacketsReceived = 1000UL,
			BytesReceived = 50000UL,
			RemoteId = "outbound-456"
		};

		Assert.AreEqual(1000UL, stats.PacketsReceived);
		Assert.AreEqual(50000UL, stats.BytesReceived);
		Assert.AreEqual("3876931414", stats.Ssrc);
		Assert.AreEqual("audio", stats.Kind);
	}
}

[TestFixture]
public class RtcOutboundRtpStreamStatsTests
{
	[Test]
	public void RtcOutboundRtpStreamStats_PacketsSent_Type()
	{
		var property = typeof(RtcOutboundRtpStreamStats).GetProperty("PacketsSent");
		Assert.IsNotNull(property, "PacketsSent property should exist on RtcOutboundRtpStreamStats");
		Assert.AreEqual(typeof(ulong), property!.PropertyType, "PacketsSent should be of type ulong");
	}

	[Test]
	public void RtcOutboundRtpStreamStats_BytesSent_Type()
	{
		var property = typeof(RtcOutboundRtpStreamStats).GetProperty("BytesSent");
		Assert.IsNotNull(property, "BytesSent property should exist on RtcOutboundRtpStreamStats");
		Assert.AreEqual(typeof(ulong), property!.PropertyType, "BytesSent should be of type ulong");
	}

	[Test]
	public void RtcOutboundRtpStreamStats_PacketsSent_Is_Not_Int()
	{
		var property = typeof(RtcOutboundRtpStreamStats).GetProperty("PacketsSent");
		Assert.AreNotEqual(typeof(int), property!.PropertyType, "PacketsSent should not be int");
	}

	[Test]
	public void RtcOutboundRtpStreamStats_BytesSent_Is_Not_Int()
	{
		var property = typeof(RtcOutboundRtpStreamStats).GetProperty("BytesSent");
		Assert.AreNotEqual(typeof(int), property!.PropertyType, "BytesSent should not be int");
	}

	[Test]
	public void RtcOutboundRtpStreamStats_Create_With_Large_Values()
	{
		// Test that ulong can handle large values
		var stats = new RtcOutboundRtpStreamStats
		{
			Timestamp = TimeSpan.FromMilliseconds(100),
			Type = RtcStatsType.OutboundRtp,
			Id = "outbound-123",
			Ssrc = "3876931414",
			Kind = "video",
			TransportId = "transport-1",
			CodecId = "codec-video-1",
			PacketsSent = 18446744073709551615UL,  // Max ulong value
			BytesSent = 18446744073709551615UL,    // Max ulong value
			RemoteId = "inbound-456"
		};

		Assert.AreEqual(18446744073709551615UL, stats.PacketsSent);
		Assert.AreEqual(18446744073709551615UL, stats.BytesSent);
		Assert.AreEqual("3876931414", stats.Ssrc);
		Assert.AreEqual("video", stats.Kind);
	}

	[Test]
	public void RtcOutboundRtpStreamStats_Create_With_Standard_Values()
	{
		var stats = new RtcOutboundRtpStreamStats
		{
			Timestamp = TimeSpan.FromMilliseconds(100),
			Type = RtcStatsType.OutboundRtp,
			Id = "outbound-456",
			Ssrc = "1234567890",
			Kind = "video",
			TransportId = "transport-2",
			CodecId = "codec-video-2",
			PacketsSent = 5000UL,
			BytesSent = 2500000UL,
			RemoteId = "inbound-789"
		};

		Assert.AreEqual(5000UL, stats.PacketsSent);
		Assert.AreEqual(2500000UL, stats.BytesSent);
	}
}
