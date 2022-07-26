#include "pch.h"

#include "Marshaling/MarshalDataChannel.h"
#include "gtest/gtest.h"

using namespace msclr::interop;

using namespace System;

using namespace WebRtcNet;
using namespace webrtc;

class marshal_data_channel_tests
{
public:
};


const std::map<DataChannelInterface::DataState, RtcDataChannelState> state_map{
	{ DataChannelInterface::DataState::kConnecting, RtcDataChannelState::Connecting },
	{ DataChannelInterface::DataState::kOpen, RtcDataChannelState::Open },
	{ DataChannelInterface::DataState::kClosing, RtcDataChannelState::Closing },
	{ DataChannelInterface::DataState::kClosed, RtcDataChannelState::Closed }
};

class marshal_data_channel_state_tests : public marshal_data_channel_tests,
	                                     public testing::TestWithParam<std::pair<const DataChannelInterface::DataState, RtcDataChannelState>>
{

public:
	static std::string param_test_name(const testing::TestParamInfo<std::pair<const DataChannelInterface::DataState, RtcDataChannelState>>& info)
	{
		auto value = info.param.second;
		auto name = System::Enum::GetName(RtcDataChannelState::typeid, (Object^)value);
		if (name == nullptr) return std::to_string(safe_cast<int>(value));
		return marshal_as<std::string>(name);
	}
};



TEST_P(marshal_data_channel_state_tests, marshal_as_native_DataState_to_managed_RtcDataChannelState_test)
{
	auto testpair = GetParam(); //get the a data state pair
	auto from = testpair.first;

	auto result = marshal_as<RtcDataChannelState>(from);

	ASSERT_EQ(result, testpair.second);
}

TEST_P(marshal_data_channel_state_tests, marshal_as_managed_RtcDataChannelState_to_native_DataState_test)
{
	auto testpair = GetParam(); //get the a data state pair
	auto from = testpair.second;

	auto result = marshal_as<DataChannelInterface::DataState>(from);

	ASSERT_EQ(result, testpair.first);
}

INSTANTIATE_TEST_CASE_P(DataChannelStates, marshal_data_channel_state_tests, testing::ValuesIn(state_map), marshal_data_channel_state_tests::param_test_name);

//
//	[Test]
//	void marshal_as_native_DataState_to_managed_RtcDataChannelState_invalid_test()
//	{
//		auto action = gcnew TestDelegate(&MarshalDataChannelTests::MarshalInvalidRtcDataChannelState);
//
//		Assert::Throws<InvalidCastException ^>(action);
//	}
//
//private:
//	static void MarshalInvalidRtcDataChannelState()
//	{
//		auto to = marshal_as<RtcDataChannelState>((DataChannelInterface::DataState)(DataChannelInterface::DataState::kClosed + 1));
//		Assert::Fail("Should not have made it here");
//	}
