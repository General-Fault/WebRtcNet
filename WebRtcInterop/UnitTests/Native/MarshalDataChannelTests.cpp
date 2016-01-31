#include "Stdafx.h"

#include "..\..\Marshaling\MarshalDataChannel.h"

using namespace msclr::interop;

using namespace System;
using namespace Microsoft::VisualStudio::TestTools::UnitTesting;

namespace
{
	[TestClass]
	public ref class MarshalDataChannelTests
	{
	public:
		[TestMethod]
		void marshal_vector_as_test()
		{
			webrtc::DataChannelInterface::DataState nativeDataChannelState = webrtc::DataChannelInterface::DataState::kConnecting;

			auto result = marshal_as<WebRtcNet::RtcDataChannelState>(nativeDataChannelState);

			Assert::AreEqual(WebRtcNet::RtcDataChannelState::Connecting, result);
		}
	};
}