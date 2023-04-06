#include "pch.h"

#include "TestUtils.h"

#include "gmock/gmock.h"
#include "gtest/gtest.h"

#include "api/ice_transport_interface.h"
#include "p2p/base/ice_transport_internal.h" // see bugs.webrtc.org/9308

#include "Marshaling/MarshalIceTransport.h"


using namespace msclr::interop;

using namespace System;

using namespace rtc;
using namespace WebRtcNet;
using namespace webrtc;
using namespace cricket;
using namespace testing;


class MockIceTransportInternal : public IceTransportInternal
{
public:
	MockIceTransportInternal()
	{
		SignalReadyToSend(this);
		SignalWritableState(this);
	}

	MOCK_METHOD(int,
	            SendPacket,
	            (const char* data,
		            size_t len,
		            const rtc::PacketOptions& options,
		            int flags),
	            (override));
	MOCK_METHOD(int, SetOption, (rtc::Socket::Option opt, int value), (override));
	MOCK_METHOD(int, GetError, (), (override));
	MOCK_METHOD(cricket::IceRole, GetIceRole, (), (const, override));
	MOCK_METHOD(bool,
	            GetStats,
	            (cricket::IceTransportStats* ice_transport_stats),
	            (override));

	cricket::IceTransportState GetState() const override
	{
		return cricket::IceTransportState::STATE_INIT;
	}

	webrtc::IceTransportState GetIceTransportState() const override
	{
		return webrtc::IceTransportState::kNew;
	}

	const std::string& transport_name() const override { return transport_name_; }
	int component() const override { return 0; }

	void SetIceRole(IceRole role) override
	{
	}

	void SetIceTiebreaker(uint64_t tiebreaker) override
	{
	}

	// The ufrag and pwd in `ice_params` must be set
	// before candidate gathering can start.
	void SetIceParameters(const IceParameters& ice_params) override
	{
	}

	void SetRemoteIceParameters(const IceParameters& ice_params) override
	{
	}

	void SetRemoteIceMode(IceMode mode) override
	{
	}

	void SetIceConfig(const IceConfig& config) override
	{
	}

	absl::optional<int> GetRttEstimate() override { return absl::nullopt; }
	const Connection* selected_connection() const override { return nullptr; }

	absl::optional<const CandidatePair> GetSelectedCandidatePair()
	const override
	{
		return absl::nullopt;
	}

	void MaybeStartGathering() override
	{
	}

	void AddRemoteCandidate(const Candidate& candidate) override
	{
	}

	void RemoveRemoteCandidate(const Candidate& candidate) override
	{
	}

	void RemoveAllRemoteCandidates() override
	{
	}

	IceGatheringState gathering_state() const override
	{
		return kIceGatheringComplete;
	}

	bool receiving() const override { return true; }
	bool writable() const override { return true; }

private:
	std::string transport_name_;
};

class MockIceTransport : IceTransportInterface
{
public:
	MockIceTransport()
		: internal_(new MockIceTransportInternal())
	{
	}

	IceTransportInternal* internal() override
	{
		return internal_.get();
	}

	std::unique_ptr<MockIceTransportInternal> internal_;
};


class ice_transport_tests
{
public:
};


const std::map<cricket::IceRole, RtcIceRole> role_map{
	{cricket::IceRole::ICEROLE_CONTROLLED, RtcIceRole::Controlled},
	{cricket::IceRole::ICEROLE_CONTROLLING, RtcIceRole::Controlling},
	{cricket::IceRole::ICEROLE_UNKNOWN, RtcIceRole::Unknown},
};

class marshal_ice_transport_role_tests : public ice_transport_tests,
                                         public TestWithParam<std::pair<
	                                         const cricket::IceRole, RtcIceRole>>
{
public:
	static std::string param_test_name(
		const TestParamInfo<std::pair<const cricket::IceRole, RtcIceRole>>& info)
	{
		auto value = info.param.second;
		auto name = Enum::GetName(RtcIceRole::typeid, value);
		if (name == nullptr) return std::to_string(safe_cast<int>(value));
		return marshal_as<std::string>(name);
	}
};


TEST_P(marshal_ice_transport_role_tests, marshal_as_native_IceRole_to_managed_RtcIceRole_test)
{
	const auto& [from, expected] = GetParam(); //get the a data state pair

	const auto result = marshal_as<RtcIceRole>(from);

	const auto expected_lval = expected;
	ASSERT_EQ(result, expected_lval);
}

TEST_P(marshal_ice_transport_role_tests, marshal_as_managed_RtcIceRole_to_native_IceRole_test)
{
	const auto& [expected, from] = GetParam(); //get the a data state pair

	const auto from_lval = from; //The Microsoft compiler apparently has some trouble with structured binding of managed value types (enums). This forces a copy
	const auto result = marshal_as<cricket::IceRole>(from_lval);

	ASSERT_EQ(result, expected);
}

INSTANTIATE_TEST_SUITE_P(IceTransportRoles, marshal_ice_transport_role_tests, testing::ValuesIn(role_map),
                         marshal_ice_transport_role_tests::param_test_name);

TEST(marshal_ice_transport_state_tests, marshal_as_native_IceRole_to_managed_RtcIceRole_invalid_test)
{
	try
	{
		auto _ = marshal_as<RtcIceRole>(
			static_cast<IceRole>(ICEROLE_UNKNOWN + 1));
		FAIL();
	}
	catch (Exception^ ex)
	{
		// check exception
		EXPECT_MANAGED_TYPE_EQ(*ex, InvalidCastException::typeid);
		EXPECT_MANAGED_STREQ(ex->Message, "Invalid cricket::IceRole value.");
	}
}
