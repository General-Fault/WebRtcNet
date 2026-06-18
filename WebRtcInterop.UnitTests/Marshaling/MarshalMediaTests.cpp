#include "pch.h"

#include "TestUtils.h"

#include "gtest/gtest.h"

#include "Media/Marshaling/MarshalMedia.h"
#include "Marshaling/MarshalMediaConstraints.h"

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

TEST(marshal_media_mode_value_tests, marshal_unknown_native_facing_mode_preserves_raw_value)
{
	auto result = marshal_as<VideoFacingModeValue>(std::string("vendor-facing-mode"));
	ASSERT_EQ(result.IsKnown, false);
	ASSERT_EQ(marshal_as<std::string>(result.RawValue), "vendor-facing-mode");
}

TEST(marshal_media_mode_value_tests, marshal_unknown_native_resize_mode_preserves_raw_value)
{
	auto result = marshal_as<VideoResizeModeValue>(std::string("vendor-resize-mode"));
	ASSERT_EQ(result.IsKnown, false);
	ASSERT_EQ(marshal_as<std::string>(result.RawValue), "vendor-resize-mode");
}

TEST(marshal_media_mode_value_tests, marshal_unknown_managed_facing_mode_to_native_throws)
{
	try
	{
		VideoFacingModeValue value(gcnew String("vendor-facing-mode"));
		const auto _ = marshal_video_facing_mode_value_to_native(value);
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}

TEST(marshal_media_mode_value_tests, marshal_unknown_managed_resize_mode_to_native_throws)
{
	try
	{
		VideoResizeModeValue value(gcnew String("vendor-resize-mode"));
		const auto _ = marshal_video_resize_mode_value_to_native(value);
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}

TEST(marshal_media_constraints_tests, marshal_media_stream_constraints_includes_advanced_and_echo_cancellation)
{
	auto videoTrackConstraints = gcnew MediaTrackConstraints();
	videoTrackConstraints->Width = gcnew MediaTrackConstraints::PositiveUIntRangeConstraint();
	videoTrackConstraints->Width->Min = 640;
	videoTrackConstraints->Width->Max = 1280;
	videoTrackConstraints->Width->Ideal = 800;

	videoTrackConstraints->EchoCancellation = gcnew EchoCancellationConstraint();
	videoTrackConstraints->EchoCancellation->Ideal = EchoCancellationValue(EchoCancellationMode::Software);

	videoTrackConstraints->Advanced = gcnew System::Collections::Generic::List<MediaTrackConstraintSet^>();
	auto advancedSet = gcnew MediaTrackConstraintSet();
	advancedSet->BackgroundBlur = gcnew MediaTrackConstraints::Constraint<bool>(true);
	videoTrackConstraints->Advanced->Add(advancedSet);

	auto streamConstraints = gcnew MediaStreamConstraints(true, videoTrackConstraints);
	auto marshaled = marshal_as(streamConstraints);

	ASSERT_EQ(marshaled.audio_requested, true);
	ASSERT_EQ(marshaled.video_requested, true);
	ASSERT_TRUE(marshaled.video_constraints.has_value());
	ASSERT_EQ(marshaled.video_constraints->advanced.size(), 1u);
	ASSERT_TRUE(marshaled.video_constraints->basic.width.has_value());
	ASSERT_TRUE(marshaled.video_constraints->basic.echo_cancellation.has_value());
	ASSERT_TRUE(marshaled.video_constraints->basic.echo_cancellation->ideal.has_value());
	ASSERT_TRUE(marshaled.video_constraints->basic.echo_cancellation->ideal->mode_value.has_value());
	ASSERT_EQ(marshaled.video_constraints->basic.echo_cancellation->ideal->mode_value.value(), "software");
}

TEST(media_stream_track_constraint_plumbing_tests, apply_constraints_with_unknown_facing_mode_throws)
{
	auto constraints = gcnew MediaTrackConstraints();
	constraints->FacingMode = gcnew MediaTrackConstraints::Constraint<VideoFacingModeValue>(VideoFacingModes::User);
	constraints->FacingMode->Exact = VideoFacingModeValue(gcnew String("vendor-facing-mode"));

	try
	{
		const auto _ = marshal_as(constraints);
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}

TEST(media_stream_track_constraint_plumbing_tests, apply_constraints_with_unknown_resize_mode_throws)
{
	auto constraints = gcnew MediaTrackConstraints();
	constraints->ResizeMode = gcnew MediaTrackConstraints::Constraint<VideoResizeModeValue>(VideoResizeModes::None);
	constraints->ResizeMode->Exact = VideoResizeModeValue(gcnew String("vendor-resize-mode"));

	try
	{
		const auto _ = marshal_as(constraints);
		FAIL();
	}
	catch (InvalidCastException^)
	{
	}
}
