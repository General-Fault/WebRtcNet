using System;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcDataChannelContractTests
{
	[Test]
	public void RtcDataChannel_Id_Is_Nullable_UShort()
	{
		var property = typeof(RtcDataChannel).GetProperty(nameof(RtcDataChannel.Id));

		Assert.That(property, Is.Not.Null);
		Assert.That(property!.PropertyType, Is.EqualTo(typeof(ushort?)));
	}

	[Test]
	public void RtcDataChannel_BufferedAmountLowThreshold_Is_NonNullable_Ulong()
	{
		var property = typeof(RtcDataChannel).GetProperty(nameof(RtcDataChannel.BufferedAmountLowThreshold));

		Assert.That(property, Is.Not.Null);
		Assert.That(property!.PropertyType, Is.EqualTo(typeof(ulong)));
	}

	[Test]
	public void RtcDataChannel_Exposes_OnClosing_Event()
	{
		var eventInfo = typeof(RtcDataChannel).GetEvent(nameof(RtcDataChannel.OnClosing));

		Assert.That(eventInfo, Is.Not.Null);
		Assert.That(eventInfo!.EventHandlerType, Is.EqualTo(typeof(EventHandler)));
	}
}
