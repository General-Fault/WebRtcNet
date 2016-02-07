#include "Stdafx.h"

#include "..\Marshaling\MarshalDataChannel.h"

using namespace msclr::interop;

using namespace System;
using namespace NUnit::Framework;
using namespace WebRtcNet;
using namespace webrtc;

namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalDataChannelTests
	{
	public:
		[Test,
			TestCase((Object ^)((int)DataChannelInterface::DataState::kConnecting), RtcDataChannelState::Connecting),
			TestCase((Object ^)((int)DataChannelInterface::DataState::kOpen), RtcDataChannelState::Open),
			TestCase((Object ^)((int)DataChannelInterface::DataState::kClosing), RtcDataChannelState::Closing),
			TestCase((Object ^)((int)DataChannelInterface::DataState::kClosed), RtcDataChannelState::Closed)
		]
		void marshal_as_native_DataState_to_managed_RtcDataChannelState_test(DataChannelInterface::DataState from, RtcDataChannelState expected)
		{
			auto to = marshal_as<RtcDataChannelState>(from);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_DataState_to_managed_RtcDataChannelState_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalDataChannelTests::MarshalInvalidRtcDataChannelState);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidRtcDataChannelState()
		{
			auto to = marshal_as<RtcDataChannelState>((DataChannelInterface::DataState)(DataChannelInterface::DataState::kClosed + 1));
			Assert::Fail("Should not have made it here");
		}
	};
}}}