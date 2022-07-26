using System.Linq;
using NUnit.Framework;

namespace WebRtcNet.UnitTests
{
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
            Assert.IsNull(configuration.PeerIdentity);
        }

        [Test]
        public void RtcConfiguration_Constructor_WithServers_Test()
        {
            var configuration = new RtcConfiguration(new[] { new RtcIceServer("stun:stun1.example.net"), new RtcIceServer(new[] { "turns:turn.example.org", "turns:turn.example.net" }, "user", "myPassword") });

            Assert.IsNotNull(configuration);
            Assert.AreEqual(2, configuration.IceServers.Count);
            Assert.AreEqual("stun:stun1.example.net", configuration.IceServers[0].Urls.ToArray()[0]);
            Assert.AreEqual("turns:turn.example.org", configuration.IceServers[1].Urls.ToArray()[0]);
            Assert.AreEqual("turns:turn.example.net", configuration.IceServers[1].Urls.ToArray()[1]);
            Assert.AreEqual("user", configuration.IceServers[1].UserName);
            Assert.AreEqual("myPassword", configuration.IceServers[1].Credential);
        }
    }
}
