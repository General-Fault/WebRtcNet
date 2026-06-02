using System.Linq;
using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcPeerConnectionContractTests
{
	[Test]
	public void RtcPeerConnection_Exposes_AddTransceiver_Overloads_For_Track_And_Kind()
	{
		var methods = typeof(RtcPeerConnection).GetMethods()
			.Where(method => method.Name == "AddTransceiver")
			.ToArray();

		Assert.That(methods, Has.Length.EqualTo(2));

		var trackOverload = methods.Single(method =>
		{
			var parameters = method.GetParameters();
			return parameters.Length == 2 && parameters[0].ParameterType == typeof(MediaStreamTrack);
		});

		Assert.That(trackOverload.ReturnType, Is.EqualTo(typeof(RtcRtpTransceiver)));
		Assert.That(trackOverload.GetParameters()[1].ParameterType, Is.EqualTo(typeof(RtcRtpTransceiverInit)));
		Assert.That(trackOverload.GetParameters()[1].IsOptional, Is.True);
		Assert.That(trackOverload.GetParameters()[1].DefaultValue, Is.Null);

		var kindOverload = methods.Single(method =>
		{
			var parameters = method.GetParameters();
			return parameters.Length == 2 && parameters[0].ParameterType == typeof(MediaStreamTrackKind);
		});

		Assert.That(kindOverload.ReturnType, Is.EqualTo(typeof(RtcRtpTransceiver)));
		Assert.That(kindOverload.GetParameters()[1].ParameterType, Is.EqualTo(typeof(RtcRtpTransceiverInit)));
		Assert.That(kindOverload.GetParameters()[1].IsOptional, Is.True);
		Assert.That(kindOverload.GetParameters()[1].DefaultValue, Is.Null);
	}

	[Test]
	public void RtcPeerConnection_RemoveTrack_Uses_RtpSender_Contract()
	{
		var methods = typeof(RtcPeerConnection).GetMethods()
			.Where(method => method.Name == "RemoveTrack")
			.ToArray();

		Assert.That(methods, Has.Length.EqualTo(1));
		Assert.That(methods[0].ReturnType, Is.EqualTo(typeof(void)));
		Assert.That(methods[0].GetParameters(), Has.Length.EqualTo(1));
		Assert.That(methods[0].GetParameters()[0].ParameterType, Is.EqualTo(typeof(RtcRtpSender)));
	}

	[Test]
	public void RtcRtpTransceiverInit_Defaults_To_SendRecv_And_Empty_Collections()
	{
		var init = new RtcRtpTransceiverInit();

		Assert.That(init.Direction, Is.EqualTo(RtcRtpTransceiverDirection.SendRecv));
		Assert.That(init.Streams, Is.Empty);
		Assert.That(init.SendEncodings, Is.Empty);
	}

	[Test]
	public void RtcIceCandidateErrorEventArgs_Constructor_Maps_Structured_Ice_Error_Payload()
	{
		var eventArgs = new RtcIceCandidateErrorEventArgs("192.0.2.1", 3478, "stun:stun.example.org", 701, "Server unreachable");

		Assert.That(eventArgs.Address, Is.EqualTo("192.0.2.1"));
		Assert.That(eventArgs.Port, Is.EqualTo((ushort)3478));
		Assert.That(eventArgs.Url, Is.EqualTo("stun:stun.example.org"));
		Assert.That(eventArgs.ErrorCode, Is.EqualTo((ushort)701));
		Assert.That(eventArgs.ErrorText, Is.EqualTo("Server unreachable"));
	}

	[Test]
	public void RtcPeerConnection_Exposes_Nullable_Optional_SetLocalDescription_And_AddIceCandidate()
	{
		var setLocalDescription = typeof(RtcPeerConnection).GetMethod(
			nameof(RtcPeerConnection.SetLocalDescription),
			new[] { typeof(RtcSessionDescription?) });
		var addIceCandidate = typeof(RtcPeerConnection).GetMethod(
			nameof(RtcPeerConnection.AddIceCandidate),
			new[] { typeof(RtcIceCandidate) });

		Assert.That(setLocalDescription, Is.Not.Null);
		Assert.That(setLocalDescription!.GetParameters()[0].IsOptional, Is.True);
		Assert.That(setLocalDescription.GetParameters()[0].DefaultValue, Is.Null);

		Assert.That(addIceCandidate, Is.Not.Null);
		Assert.That(addIceCandidate!.GetParameters()[0].IsOptional, Is.True);
		Assert.That(addIceCandidate.GetParameters()[0].DefaultValue, Is.Null);
	}

	[Test]
	public void RtcPeerConnection_Exposes_Sctp_Property()
	{
		var property = typeof(RtcPeerConnection).GetProperty(nameof(RtcPeerConnection.Sctp));

		Assert.That(property, Is.Not.Null);
		Assert.That(property!.PropertyType, Is.EqualTo(typeof(RtcSctpTransport)));
	}

	[Test]
	public void RtcIceCandidateEventArgs_Exposes_Optional_Url()
	{
		var candidate = new RtcIceCandidate("candidate:0 1 UDP 2122252543 192.0.2.1 54400 typ host");
		var eventArgs = new RtcIceCandidateEventArgs(candidate, "stun:stun.example.org");

		Assert.That(eventArgs.Candidate, Is.SameAs(candidate));
		Assert.That(eventArgs.Url, Is.EqualTo("stun:stun.example.org"));
	}

	[Test]
	public void RtcSdpType_Includes_Rollback()
	{
		Assert.That(System.Enum.GetNames(typeof(RtcSdpType)), Does.Contain(nameof(RtcSdpType.Rollback)));
	}

	[Test]
	public void RtcPeerConnection_Exposes_Static_GenerateCertificate_Method()
	{
		var method = typeof(RtcPeerConnection).GetMethod(nameof(RtcPeerConnection.GenerateCertificate), new[] { typeof(object) });

		Assert.That(method, Is.Not.Null);
		Assert.That(method!.ReturnType, Is.EqualTo(typeof(System.Threading.Tasks.Task<RtcCertificate>)));
	}
}
