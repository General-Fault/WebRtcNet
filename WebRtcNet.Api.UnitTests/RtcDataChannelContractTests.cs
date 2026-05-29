using System;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcDataChannelContractTests
{
	[Test]
	public void IRtcDataChannel_Id_Is_Nullable_UShort()
	{
		var property = typeof(IRtcDataChannel).GetProperty(nameof(IRtcDataChannel.Id));

		Assert.That(property, Is.Not.Null);
		Assert.That(property!.PropertyType, Is.EqualTo(typeof(ushort?)));
	}

	[Test]
	public void IRtcDataChannel_BufferedAmountLowThreshold_Is_NonNullable_Ulong()
	{
		var property = typeof(IRtcDataChannel).GetProperty(nameof(IRtcDataChannel.BufferedAmountLowThreshold));

		Assert.That(property, Is.Not.Null);
		Assert.That(property!.PropertyType, Is.EqualTo(typeof(ulong)));
	}

	[Test]
	public void IRtcDataChannel_Exposes_OnClosing_Event()
	{
		var eventInfo = typeof(IRtcDataChannel).GetEvent(nameof(IRtcDataChannel.OnClosing));

		Assert.That(eventInfo, Is.Not.Null);
		Assert.That(eventInfo!.EventHandlerType, Is.EqualTo(typeof(EventHandler)));
	}
}
