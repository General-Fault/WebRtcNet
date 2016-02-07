#include "Stdafx.h"

#include <string>

#include "..\Marshaling\MarshalIceCandidate.h"
#include "FakeIceCandidate.h"

#include "webrtc\base\bind.h"

using namespace msclr::interop;

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace NUnit::Framework;


namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalIceCandidateTests
	{
	public:
		[Test]
		void marshal_as_native_IceCandidateInterface_to_managed_RtcIceCandidate_test()
		{
			FakeIceCandidate nativeCandidate(std::string("SDP"), 3);
			
			auto managedCandidate = marshal_as<WebRtcNet::RtcIceCandidate>(dynamic_cast<const webrtc::IceCandidateInterface *>(&nativeCandidate));

			Assert::AreEqual("SDP", managedCandidate.SdpMid);
			Assert::AreEqual(3, managedCandidate.SdpMLineIndex);
			Assert::AreEqual("Cand[FakeFoundation:1:Fake:1::0:FakeType::0:username:password]", managedCandidate.Candidate);
		}

		[Test]
		void marshal_as_null_native_IceCandidateInterface_to_managed_RtcIceCandidate_test()
		{
			auto action = gcnew TestDelegate(this, &MarshalIceCandidateTests::MarshalNullNativeIceCandidate);

			Assert::Throws<ArgumentNullException ^>(action);
		}

	private:
		void MarshalNullNativeIceCandidate()
		{
			const webrtc::IceCandidateInterface* nativeCandidate = nullptr;
			auto managedCandidate = marshal_as<WebRtcNet::RtcIceCandidate>(nativeCandidate);
			Assert::Fail("Should not have made it here");
		}
	};
}}}