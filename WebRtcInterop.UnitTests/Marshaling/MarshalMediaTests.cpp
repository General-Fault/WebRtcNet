#include "pch.h"

#include "TestUtils.h"

#include "gtest/gtest.h"

#include "Media/Marshaling/MarshalMedia.h"

using namespace msclr::interop;
using namespace System;
using namespace WebRtcNet::Media;
using namespace webrtc;
using namespace testing;

class marshal_media_track_state_tests : public TestWithParam<std::pair<const MediaStreamTrackInterface::TrackState, MediaStreamTrackState>>
{
public:
	static std::string param_test_name(
		const TestParamInfo<std::pair<const MediaStreamTrackInterface::TrackState, MediaStreamTrackState>>& info)
	{
		auto value = info.param.second;
		auto name = Enum::GetName(MediaStreamTrackState::typeid, value);
		if (name == nullptr) return std::to_string(safe_cast<int>(value));
		return marshal_as<std::string>(name);
	}
};

const std::map<MediaStreamTrackInterface::TrackState, MediaStreamTrackState> media_track_state_map{
	{MediaStreamTrackInterface::TrackState::kLive, MediaStreamTrackState::Live},
	{MediaStreamTrackInterface::TrackState::kEnded, MediaStreamTrackState::Ended},
};

TEST_P(marshal_media_track_state_tests, marshal_native_track_state_to_managed_track_state)
{
	const auto& [from, expected] = GetParam();
	const auto result = marshal_as<MediaStreamTrackState>(from);
	const auto expected_lval = expected;
	ASSERT_EQ(result, expected_lval);
}

TEST_P(marshal_media_track_state_tests, marshal_managed_track_state_to_native_track_state)
{
	const auto& [expected, from] = GetParam();
	const auto unwrapped_from = from;
	const auto result = marshal_as<MediaStreamTrackInterface::TrackState>(unwrapped_from);
	ASSERT_EQ(result, expected);
}

INSTANTIATE_TEST_SUITE_P(
	MediaTrackState,
	marshal_media_track_state_tests,
	testing::ValuesIn(media_track_state_map),
	marshal_media_track_state_tests::param_test_name);

TEST(marshal_media_track_state_tests, marshal_native_track_state_invalid_throws)
{
	try
	{
		const auto _ = marshal_as<MediaStreamTrackState>(
			static_cast<MediaStreamTrackInterface::TrackState>(MediaStreamTrackInterface::TrackState::kEnded + 1));
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}

TEST(marshal_media_track_kind_tests, marshal_native_audio_kind_to_managed_audio)
{
	const auto result = marshal_as<MediaStreamTrackKind>(std::string(MediaStreamTrackInterface::kAudioKind));
	ASSERT_EQ(result, MediaStreamTrackKind::Audio);
}

TEST(marshal_media_track_kind_tests, marshal_native_video_kind_to_managed_video)
{
	const auto result = marshal_as<MediaStreamTrackKind>(std::string(MediaStreamTrackInterface::kVideoKind));
	ASSERT_EQ(result, MediaStreamTrackKind::Video);
}

TEST(marshal_media_track_kind_tests, marshal_managed_audio_kind_to_native_audio)
{
	const auto result = marshal_as<std::string>(MediaStreamTrackKind::Audio);
	ASSERT_EQ(result, MediaStreamTrackInterface::kAudioKind);
}

TEST(marshal_media_track_kind_tests, marshal_managed_video_kind_to_native_video)
{
	const auto result = marshal_as<std::string>(MediaStreamTrackKind::Video);
	ASSERT_EQ(result, MediaStreamTrackInterface::kVideoKind);
}

TEST(marshal_media_track_kind_tests, marshal_native_track_kind_invalid_throws)
{
	try
	{
		const auto _ = marshal_as<MediaStreamTrackKind>(std::string("data"));
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}
