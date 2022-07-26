#include "Stdafx.h"

#include "../MarshalRtcConfiguration.h"

using namespace msclr::interop;

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace NUnit::Framework;
using namespace webrtc;
using namespace WebRtcNet;

namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalRtcConfigurationTests
	{
	public:

#pragma region(WebRtcNet::RtcIceTransportPolicy to webrtc::PeerConnectionInterface::IceTransportsType)
	public:
		
		[Test,
			TestCase(RtcIceTransportPolicy::All, (Object ^)((int)PeerConnectionInterface::IceTransportsType::kAll)),
			TestCase(RtcIceTransportPolicy::None, (Object ^)((int)PeerConnectionInterface::IceTransportsType::kNone)),
			TestCase(RtcIceTransportPolicy::Relay, (Object ^)((int)PeerConnectionInterface::IceTransportsType::kRelay))]
		void marshal_as_manaegd_RtcIceTransportPolicy_to_native_IceTransportsType_test(RtcIceTransportPolicy from, PeerConnectionInterface::IceTransportsType expected)
		{
			auto to = marshal_as<PeerConnectionInterface::IceTransportsType>(from);

			Assert::AreEqual(static_cast<int>(expected), static_cast<int>(to));
		}

		[Test]
		void marshal_as_native_BundlePolicy_to_managed_RtcIceTransportPolicy_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalRtcConfigurationTests::MarshalInvalidIceTransportPolicy);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidIceTransportPolicy()
		{
			auto to = marshal_as<PeerConnectionInterface::IceTransportsType>(RtcIceTransportPolicy::All+(RtcIceTransportPolicy)1);
			Assert::Fail("Should not have made it here");
		}
#pragma endregion

#pragma region(WebRtcNet::RtcBundlePolicy to webrtc::PeerConnectionInterface::BundlePolicy)
	public:

		[Test,
			TestCase(RtcBundlePolicy::Balanced, (Object ^)((int)PeerConnectionInterface::BundlePolicy::kBundlePolicyBalanced)),
			TestCase(RtcBundlePolicy::MaxBundle, (Object ^)((int)PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxBundle)),
			TestCase(RtcBundlePolicy::MaxCompat, (Object ^)((int)PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat))]
		void marshal_as_manaegd_RtcBundlePolicy_to_native_BundlePolicy_test(RtcBundlePolicy from, PeerConnectionInterface::BundlePolicy expected)
		{
			auto to = marshal_as<PeerConnectionInterface::BundlePolicy>(from);

			Assert::AreEqual(static_cast<int>(expected), static_cast<int>(to));
		}

		[Test]
		void marshal_as_native_BundlePolicy_to_managed_RtcBundlePolicy_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalRtcConfigurationTests::MarshalInvalidRtcBundlePolicy);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidRtcBundlePolicy()
		{
			auto to = marshal_as<PeerConnectionInterface::BundlePolicy>(RtcBundlePolicy::MaxBundle + (RtcBundlePolicy)1);
			Assert::Fail("Should not have made it here");
		}
#pragma endregion
	};
}}}