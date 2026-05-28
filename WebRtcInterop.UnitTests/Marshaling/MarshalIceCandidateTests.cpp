#include "pch.h"

#include "TestUtils.h"

#include "gtest/gtest.h"

#include "Marshaling/MarshalIceCandidate.h"
#include <msclr/gcroot.h>

using namespace msclr::interop;

using namespace System;

using namespace WebRtcNet;
using namespace webrtc;
using namespace testing;

// Tests for marshaling webrtc::IceCandidateInterface -> RtcIceCandidate.
//
// Using TEST_F so the marshal_as call happens once in SetUp(), not once per TEST body.
// MSVC C++/CLI shares state across all files in a batch cl.exe invocation; putting the conversion
// in a single virtual method avoids the per-invocation limit on native pointer upcasts.
class marshal_ice_candidate_tests : public Test
{
protected:
	std::unique_ptr<webrtc::IceCandidate> candidate_;
	msclr::gcroot<WebRtcNet::RtcIceCandidate^> result_;

	void SetUp() override
	{
		webrtc::Candidate cand(
			1,                                // component (RTP per RFC 5245)
			"udp",                            // protocol
			webrtc::SocketAddress(),          // address (empty)
			1,                                // priority
			"username",                       // username fragment
			"password",                       // password
			webrtc::IceCandidateType::kHost,  // type
			0,                                // generation
			"FakeFoundation"                  // foundation
		);
		candidate_ = std::make_unique<webrtc::IceCandidate>("SDP", 3, cand);
		const webrtc::IceCandidateInterface* nativeCandidate = candidate_.get();
		result_ = marshal_as<RtcIceCandidate^>(nativeCandidate);
	}
};

TEST_F(marshal_ice_candidate_tests, marshal_null_throws_ArgumentNullException)
{
	const webrtc::IceCandidateInterface* nativeCandidate = nullptr;

	try
	{
		const auto _ = marshal_as<RtcIceCandidate^>(nativeCandidate);
		FAIL();
	}
	catch (ArgumentNullException^)
	{
	}
}

TEST_F(marshal_ice_candidate_tests, populates_Candidate_from_ToString)
{
	// IceCandidate::ToString() serializes as SDP candidate-attribute; verify it round-trips through foundation.
	ASSERT_TRUE(result_->Candidate->Contains("FakeFoundation"));
}

TEST_F(marshal_ice_candidate_tests, populates_SdpMid)
{
	ASSERT_MANAGED_STREQ(result_->SdpMid, "SDP");
}

TEST_F(marshal_ice_candidate_tests, populates_SdpMLineIndex)
{
	ASSERT_TRUE(result_->SdpMLineIndex.HasValue);
	ASSERT_EQ(result_->SdpMLineIndex.Value, 3);
}

TEST_F(marshal_ice_candidate_tests, populates_Foundation)
{
	ASSERT_MANAGED_STREQ(result_->Foundation, "FakeFoundation");
}

TEST_F(marshal_ice_candidate_tests, populates_Component_Rtp_for_component_1)
{
	ASSERT_TRUE(result_->Component.HasValue);
	ASSERT_EQ(safe_cast<int>(result_->Component.Value), safe_cast<int>(RtcIceComponent::Rtp));
}

TEST_F(marshal_ice_candidate_tests, populates_Priority)
{
	ASSERT_TRUE(result_->Priority.HasValue);
	ASSERT_EQ(result_->Priority.Value, 1u);
}

TEST_F(marshal_ice_candidate_tests, populates_UsernameFragment)
{
	ASSERT_MANAGED_STREQ(result_->UsernameFragment, "username");
}

TEST_F(marshal_ice_candidate_tests, Protocol_is_Udp)
{
	ASSERT_TRUE(result_->Protocol.HasValue);
	ASSERT_EQ(result_->Protocol.Value, RtcIceProtocol::Udp);
}

TEST_F(marshal_ice_candidate_tests, Type_is_Host_for_kHost)
{
	ASSERT_TRUE(result_->Type.HasValue);
	ASSERT_EQ(result_->Type.Value, RtcIceCandidateType::Host);
}

TEST_F(marshal_ice_candidate_tests, marshal_invalid_protocol_throws_InvalidCastException)
{
	webrtc::Candidate cand(
		1,
		"invalid-proto",
		webrtc::SocketAddress(),
		1,
		"username",
		"password",
		webrtc::IceCandidateType::kHost,
		0,
		"FakeFoundation");
	webrtc::IceCandidate candidate("SDP", 3, cand);
	const webrtc::IceCandidateInterface* nativeCandidate = &candidate;

	try
	{
		const auto _ = marshal_as<RtcIceCandidate^>(nativeCandidate);
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}
