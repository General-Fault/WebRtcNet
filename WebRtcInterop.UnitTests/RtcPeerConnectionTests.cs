using System.Threading.Tasks;
using NUnit.Framework;

using WebRtcNet;

namespace WebRtcInterop.UnitTests
{
    [TestFixture]
    public class RtcPeerConnectionTests
    {
        private RtcPeerConnection _peerConnection;

        [SetUp]
        public void Setup()
        {
            var configuration = new RtcConfiguration(new RtcIceServer[] { });
            _peerConnection = new RtcPeerConnection(configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _peerConnection?.Dispose();
        }

        [Test]
        public void TestCreateRtcPeerConnection()
        {
            Assert.IsNotNull(_peerConnection);
        }

        [Test]
        public void TestCreateRtcPeerConnection_InitialConfiguration()
        {
            var configuration = new RtcConfiguration(new RtcIceServer[] { });
            var rtcPeerConnection = new RtcPeerConnection(configuration);

            Assert.AreEqual(configuration, rtcPeerConnection.GetConfiguration());
        }

        [Test]
        public void TestCreateRtcPeerConnection_InitialSignalingState()
        {
            Assert.AreEqual(RtcSignalingState.Stable, _peerConnection.SignalingState);
        }

        [Test]
        public void TestCreateRtcPeerConnection_InitialIceConnectionState()
        {
            Assert.AreEqual(RtcIceConnectionState.New, _peerConnection.IceConnectionState);
        }

        [Test]
        public void TestCreateRtcPeerConnection_InitialIceGatheringState()
        {
            Assert.AreEqual(RtcGatheringState.New, _peerConnection.IceGatheringState);
        }

        [Test]
        public void DefaultOfferOptionsTest()
        {
            var options = new RtcOfferOptions();

            Assert.AreEqual(1, options.OfferToReceiveAudio);
            Assert.AreEqual(1, options.OfferToReceiveVideo);
            Assert.IsFalse(options.IceRestart);
            Assert.IsTrue(options.VoiceActivityDetection);
        }

        [Test]
        public void UpdateIceTest()
        {
            var newConfiguration = new RtcConfiguration(new RtcIceServer[] { new RtcIceServer("stun:stun1.google.com", "username", "credential") });
            _peerConnection.UpdateIce(newConfiguration);
            var configuration = _peerConnection.GetConfiguration();

            Assert.AreSame(newConfiguration, configuration);
        }

        [Test]
        public async Task CreateOfferTest()
        {
            RtcSessionDescription sessionDescription = new RtcSessionDescription(RtcSdpType.Answer, string.Empty);
            var task = _peerConnection.CreateOffer();
            sessionDescription = await task;

            Assert.AreEqual(RtcSdpType.Offer, sessionDescription.Type);
            Assert.IsFalse(string.IsNullOrWhiteSpace(sessionDescription.Sdp));
        }
    }
}
