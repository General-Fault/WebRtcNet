using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WebRtcInterop;
using WebRtcNet;

namespace WebRtcNetTests
{
    [TestClass]
    public class RtcPeerConnectionTests
    {
        [TestMethod]
        public void TestCreateRtcPeerConnection()
        {
            var configuration = new RtcConfiguration(new RtcIceServer[] { });
            var rtcPeerConnection = new RtcPeerConnection(configuration);

            Assert.IsNotNull(rtcPeerConnection);
            Assert.AreEqual(configuration, rtcPeerConnection.GetConfiguration());
            Assert.AreEqual(RtcSignalingState.Stable, rtcPeerConnection.SignalingState);
            Assert.AreEqual(RtcIceConnectionState.New, rtcPeerConnection.IceConnectionState);
            Assert.AreEqual(RtcGatheringState.New, rtcPeerConnection.IceGatheringState);
        }

        [TestMethod]
        public void TestDefaultOfferOptions()
        {
            var options = new RtcOfferOptions();

            Assert.AreEqual(1, options.OfferToReceiveAudio);
            Assert.AreEqual(1, options.OfferToReceiveVideo);
            Assert.IsFalse(options.IceRestart);
            Assert.IsTrue(options.VoiceActivityDetection);
        }

        [TestMethod]
        public async Task TestCreateOffer()
        {
            var configuration = new RtcConfiguration(new RtcIceServer[] { });
            var rtcPeerConnection = new RtcPeerConnection(configuration);

            var options = new RtcOfferOptions();

            var sessionDescription = await rtcPeerConnection.CreateOffer(options);

            Assert.IsNotNull(sessionDescription);
        }
    }
}
