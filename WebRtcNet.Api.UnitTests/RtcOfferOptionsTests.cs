using System.Reflection;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcOfferOptionsTests
{
	[Test]
	public void RtcOfferOptions_Defaults_Reflect_Current_Standard_Options()
	{
		var options = new RtcOfferOptions();

		Assert.IsFalse(options.IceRestart);
		Assert.IsTrue(options.VoiceActivityDetection);
	}

	[Test]
	public void RtcOfferOptions_Does_Not_Expose_Legacy_Offer_To_Receive_Members()
	{
		var type = typeof(RtcOfferOptions);

		Assert.IsNull(type.GetProperty("OfferToReceiveAudio", BindingFlags.Public | BindingFlags.Instance));
		Assert.IsNull(type.GetProperty("OfferToReceiveVideo", BindingFlags.Public | BindingFlags.Instance));
		Assert.IsNull(type.GetField("Undefined", BindingFlags.Public | BindingFlags.Static));
		Assert.IsNull(type.GetField("MaxOfferToReceiveMedia", BindingFlags.Public | BindingFlags.Static));
		Assert.IsNull(type.GetField("OfferToReceiveTrue", BindingFlags.Public | BindingFlags.Static));
		Assert.IsNull(type.GetField("OfferToReceiveFalse", BindingFlags.Public | BindingFlags.Static));
	}
}
