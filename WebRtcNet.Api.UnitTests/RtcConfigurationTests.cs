using System.Linq;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcConfigurationTests
{
	[Test]
	public void RtcConfiguration_Constructor_Defaults_Test()
	{
		var configuration = new RtcConfiguration();

		Assert.AreEqual(0, configuration.IceServers.Count);
		Assert.AreEqual(RtcIceTransportPolicy.All, configuration.IceTransportPolicy);
		Assert.AreEqual(RtcBundlePolicy.Balanced, configuration.BundlePolicy);
		Assert.AreEqual(RtcRtcpMuxPolicy.Require, configuration.RtcpMuxPolicy);
		Assert.AreEqual(0, configuration.Certificates.Count);
		Assert.AreEqual((byte)0, configuration.IceCandidatePoolSize);
	}

	[Test]
	public void RtcConfiguration_Constructor_WithServers_Test()
	{
		var configuration = new RtcConfiguration([new RtcIceServer("stun:stun1.example.net"), new RtcIceServer(
			["turns:turn.example.org", "turns:turn.example.net"], "user", "myPassword")
		]);

		Assert.IsNotNull(configuration);
		Assert.AreEqual(2, configuration.IceServers.Count);
		Assert.AreEqual("stun:stun1.example.net", configuration.IceServers[0].Urls.ToArray()[0]);
		Assert.AreEqual("turns:turn.example.org", configuration.IceServers[1].Urls.ToArray()[0]);
		Assert.AreEqual("turns:turn.example.net", configuration.IceServers[1].Urls.ToArray()[1]);
		Assert.AreEqual("user", configuration.IceServers[1].UserName);
		Assert.AreEqual("myPassword", configuration.IceServers[1].Credential);
	}

	[Test]
	public void RtcConfiguration_Removes_Legacy_PeerIdentity_And_IceTransportPolicy_None()
	{
		Assert.IsNull(typeof(RtcConfiguration).GetProperty("PeerIdentity"));
		Assert.IsFalse(System.Enum.GetNames(typeof(RtcIceTransportPolicy)).Contains("None"));
	}
}