using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtpContractTests
{
	[Test]
	public void RtcRtpEncodingParameters_Defaults_Codec_And_MaxFramerate_To_Null()
	{
		var parameters = new RtcRtpEncodingParameters();

		Assert.IsNull(parameters.Codec);
		Assert.IsNull(parameters.MaxFramerate);
	}

	[Test]
	public void RtcRtpCodecParameters_Inherits_RtcRtpCodec()
	{
		Assert.IsTrue(typeof(RtcRtpCodec).IsAssignableFrom(typeof(RtcRtpCodecParameters)));
		Assert.IsTrue(typeof(RtcRtpCodec).IsAssignableFrom(typeof(RtcRtpCodecCapability)));
	}

	[Test]
	public void IRtcRtpSender_Exposes_Updated_SetParameters_Overload()
	{
		var method = typeof(IRtcRtpSender).GetMethod(
			nameof(IRtcRtpSender.SetParameters),
			new[] { typeof(RtcRtpSendParameters), typeof(RtcSetParameterOptions) });

		Assert.IsNotNull(method);
	}

	[Test]
	public void IRtcRtpTransceiver_Uses_Shared_RtpCodec_Type_For_Codec_Preferences()
	{
		var method = typeof(IRtcRtpTransceiver).GetMethod(
			nameof(IRtcRtpTransceiver.SetCodecPreferences),
			new[] { typeof(IEnumerable<RtcRtpCodec>) });

		Assert.IsNotNull(method);
	}

	[Test]
	public void RtcRtpCodec_Defaults_Optional_Values()
	{
		var codec = new RtcRtpCodec();

		Assert.AreEqual(string.Empty, codec.MimeType);
		Assert.IsNull(codec.Channels);
		Assert.AreEqual(string.Empty, codec.SdpFmtpLine);
	}
}
