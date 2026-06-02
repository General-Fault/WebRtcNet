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
		Assert.IsNull(typeof(RtcOfferOptions).GetProperty("VoiceActivityDetection", BindingFlags.Public | BindingFlags.Instance));
	}

	[Test]
	public void RtcAnswerOptions_Does_Not_Expose_Offer_Only_IceRestart()
	{
		var answerType = typeof(RtcAnswerOptions);

		Assert.IsNull(answerType.GetProperty(nameof(RtcOfferOptions.IceRestart), BindingFlags.Public | BindingFlags.Instance));
	}

	[Test]
	public void RtcOfferOptions_Exposes_OfferToReceiveAudio_And_Video()
	{
		var type = typeof(RtcOfferOptions);

		var audioProperty = type.GetProperty("OfferToReceiveAudio", BindingFlags.Public | BindingFlags.Instance);
		var videoProperty = type.GetProperty("OfferToReceiveVideo", BindingFlags.Public | BindingFlags.Instance);

		Assert.IsNotNull(audioProperty);
		Assert.IsNotNull(videoProperty);
	}

	[Test]
	public void RtcOfferOptions_OfferToReceive_Properties_Default_To_False()
	{
		var options = new RtcOfferOptions();

		Assert.IsFalse(options.OfferToReceiveAudio);
		Assert.IsFalse(options.OfferToReceiveVideo);
	}

	[Test]
	public void RtcOfferOptions_OfferToReceive_Properties_Can_Be_Set()
	{
		var options = new RtcOfferOptions { OfferToReceiveAudio = true, OfferToReceiveVideo = true };

		Assert.IsTrue(options.OfferToReceiveAudio);
		Assert.IsTrue(options.OfferToReceiveVideo);
	}
}
