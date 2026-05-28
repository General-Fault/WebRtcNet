using System.Linq;
using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RtcPeerConnectionContractTests
{
	[Test]
	public void IRtcPeerConnection_Does_Not_Expose_Legacy_AddTransceiver_Overloads()
	{
		var methods = typeof(IRtcPeerConnection).GetMethods()
			.Where(method => method.Name == "AddTransceiver")
			.ToArray();

		Assert.IsEmpty(methods);
	}
}
