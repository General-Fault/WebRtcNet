#include "pch.h"

#include "TestUtils.h"

#include "gtest/gtest.h"

#include "Marshaling/MarshalIceTransport.h"


using namespace msclr::interop;

using namespace System;

using namespace WebRtcNet;
using namespace webrtc;
using namespace testing;


class ice_transport_tests
{
public:
};


const std::map<IceRole, RtcIceRole> role_map{
	{IceRole::ICEROLE_CONTROLLED, RtcIceRole::Controlled},
	{IceRole::ICEROLE_CONTROLLING, RtcIceRole::Controlling},
	{IceRole::ICEROLE_UNKNOWN, RtcIceRole::Unknown},
};

class marshal_ice_transport_role_tests : public ice_transport_tests,
                                         public TestWithParam<std::pair<
	                                         const IceRole, RtcIceRole>>
{
public:
	static std::string param_test_name(
		const TestParamInfo<std::pair<const IceRole, RtcIceRole>>& info)
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
	const auto result = marshal_as<IceRole>(from_lval);

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
	catch (InvalidCastException^)
	{
	}
}
