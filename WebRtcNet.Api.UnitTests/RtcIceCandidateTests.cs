using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcIceCandidateTests
{
    [Test]
    public void RtcIceCandidate_PublicCtor_Sets_Candidate()
    {
        var candidate = new RtcIceCandidate("a=candidate:1 1 UDP 2130706431 10.0.0.1 8998 typ host");

        Assert.AreEqual("a=candidate:1 1 UDP 2130706431 10.0.0.1 8998 typ host", candidate.Candidate);
    }

    [Test]
    public void RtcIceCandidate_PublicCtor_Defaults_NullableFields_To_Null()
    {
        var candidate = new RtcIceCandidate("a=candidate:1 1 UDP 2130706431 10.0.0.1 8998 typ host");

        Assert.IsNull(candidate.SdpMid);
        Assert.IsNull(candidate.SdpMLineIndex);
        Assert.IsNull(candidate.UsernameFragment);
        Assert.IsNull(candidate.Foundation);
        Assert.IsNull(candidate.Component);
        Assert.IsNull(candidate.Priority);
        Assert.IsNull(candidate.Address);
        Assert.IsNull(candidate.Protocol);
        Assert.IsNull(candidate.Port);
        Assert.IsNull(candidate.Type);
        Assert.IsNull(candidate.TcpType);
        Assert.IsNull(candidate.RelatedAddress);
        Assert.IsNull(candidate.RelatedPort);
    }

    [Test]
    public void RtcIceCandidate_PublicCtor_Sets_OptionalSignalingFields()
    {
        var candidate = new RtcIceCandidate("a=candidate:1 1 UDP 2130706431 10.0.0.1 8998 typ host",
            sdpMid: "audio", sdpMLineIndex: 0, usernameFragment: "ufrag");

        Assert.AreEqual("audio", candidate.SdpMid);
        Assert.AreEqual((ushort)0, candidate.SdpMLineIndex);
        Assert.AreEqual("ufrag", candidate.UsernameFragment);
    }

    [Test]
    public void RtcIceCandidate_Records_Are_Equal_When_All_Fields_Match()
    {
        var a = new RtcIceCandidate("cand", "mid", 1, "ufrag");
        var b = new RtcIceCandidate("cand", "mid", 1, "ufrag");

        Assert.AreEqual(a, b);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Test]
    public void RtcIceCandidate_Records_Are_Not_Equal_When_Fields_Differ()
    {
        var a = new RtcIceCandidate("cand1", "mid", 1, "ufrag");
        var b = new RtcIceCandidate("cand2", "mid", 1, "ufrag");

        Assert.AreNotEqual(a, b);
    }

    [Test]
    public void RtcIceCandidate_With_Creates_Modified_Copy()
    {
        var original = new RtcIceCandidate("cand", "audio", 0, "ufrag");
        var modified = original with { SdpMid = "video" };

        Assert.AreEqual("video", modified.SdpMid);
        Assert.AreEqual(original.Candidate, modified.Candidate);
        Assert.AreEqual(original.SdpMLineIndex, modified.SdpMLineIndex);
    }
}
