#include "pch.h"

#include "TestUtils.h"

#include "gmock/gmock.h"
#include "gtest/gtest.h"

#include "Marshaling/MarshalDataChannel.h"


using namespace msclr::interop;

using namespace System;

using namespace rtc;
using namespace WebRtcNet;
using namespace webrtc;
using namespace testing;

class FakeDataChannel : public RefCountedObject<DataChannelInterface>
{
public:
	MOCK_METHOD(void, RegisterObserver, (webrtc::DataChannelObserver*), (override));
	MOCK_METHOD(void, UnregisterObserver, (), (override));
	MOCK_METHOD(std::string, label, (), (const, override));
	MOCK_METHOD(bool, reliable, (), (const, override));
	MOCK_METHOD(bool, ordered, (), (const, override));
	MOCK_METHOD(uint16_t, maxRetransmitTime, (), (const, override));
	MOCK_METHOD(uint16_t, maxRetransmits, (), (const, override));
	MOCK_METHOD(absl::optional<int>, maxRetransmitsOpt, (), (const, override));
	MOCK_METHOD(absl::optional<int>, maxPacketLifeTime, (), (const, override));
	MOCK_METHOD(std::string, protocol, (), (const, override));
	MOCK_METHOD(bool, negotiated, (), (const, override));
	MOCK_METHOD(int, id, (), (const, override));
	MOCK_METHOD(Priority, priority, (), (const, override));
	MOCK_METHOD(DataState, state, (), (const, override));
	MOCK_METHOD(RTCError, error, (), (const, override));
	MOCK_METHOD(uint32_t, messages_sent, (), (const, override));
	MOCK_METHOD(uint64_t, bytes_sent, (), (const, override));
	MOCK_METHOD(uint32_t, messages_received, (), (const, override));
	MOCK_METHOD(uint64_t, bytes_received, (), (const, override));
	MOCK_METHOD(uint64_t, buffered_amount, (), (const, override));
	MOCK_METHOD(void, Close, (), (override));
	MOCK_METHOD(bool, Send, (const DataBuffer&), (override));
};

class marshal_data_channel_tests
{
public:
};


const std::map<DataChannelInterface::DataState, RtcDataChannelState> state_map{
	{DataChannelInterface::DataState::kConnecting, RtcDataChannelState::Connecting},
	{DataChannelInterface::DataState::kOpen, RtcDataChannelState::Open},
	{DataChannelInterface::DataState::kClosing, RtcDataChannelState::Closing},
	{DataChannelInterface::DataState::kClosed, RtcDataChannelState::Closed}
};

class marshal_data_channel_state_tests : public marshal_data_channel_tests,
                                         public TestWithParam<std::pair<
	                                         const DataChannelInterface::DataState, RtcDataChannelState>>
{
public:
	static std::string param_test_name(
		const TestParamInfo<std::pair<const DataChannelInterface::DataState, RtcDataChannelState>>& info)
	{
		auto value = info.param.second;
		auto name = Enum::GetName(RtcDataChannelState::typeid, value);
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

INSTANTIATE_TEST_SUITE_P(DataChannelStates, marshal_data_channel_state_tests, testing::ValuesIn(state_map),
                         marshal_data_channel_state_tests::param_test_name);

TEST(marshal_data_channel_state_tests, marshal_as_native_dataState_to_managed_rtcdatachannelstate_invalid_test)
{
	try
	{
		auto _ = marshal_as<RtcDataChannelState>(
			static_cast<DataChannelInterface::DataState>(DataChannelInterface::DataState::kClosed + 1));
		FAIL();
	}
	catch (Exception^ ex)
	{
		// check exception
		EXPECT_MANAGED_TYPE_EQ(*ex, InvalidCastException::typeid);
		EXPECT_MANAGED_STREQ(ex->Message, "Invalid DataChannelInterface::DataState value.");
	}
}
