#pragma once

#include <cstdint>
#include <optional>
#include <string>
#include <vector>

#include "MarshalCollections.h"
#include "MarshalMedia.h"
#include <msclr/marshal.h>

namespace WebRtcInterop::Marshaling
{
	struct StringConstraint
	{
		std::optional<std::string> ideal;
		std::optional<std::string> exact;
	};

	struct BooleanConstraint
	{
		std::optional<bool> ideal;
		std::optional<bool> exact;
	};

	struct UintConstraint
	{
		std::optional<uint32_t> ideal;
		std::optional<uint32_t> exact;
		std::optional<uint32_t> min;
		std::optional<uint32_t> max;
	};

	struct DoubleConstraint
	{
		std::optional<double> ideal;
		std::optional<double> exact;
		std::optional<double> min;
		std::optional<double> max;
	};

	struct EchoCancellationValue
	{
		std::optional<bool> boolean_value;
		std::optional<std::string> mode_value;
	};

	struct EchoCancellationConstraint
	{
		std::optional<EchoCancellationValue> ideal;
		std::optional<EchoCancellationValue> exact;
	};

	struct MediaTrackConstraintSet
	{
		std::optional<UintConstraint> width;
		std::optional<UintConstraint> height;
		std::optional<DoubleConstraint> aspect_ratio;
		std::optional<DoubleConstraint> frame_rate;
		std::optional<StringConstraint> facing_mode;
		std::optional<StringConstraint> resize_mode;
		std::optional<UintConstraint> sample_rate;
		std::optional<UintConstraint> sample_size;
		std::optional<BooleanConstraint> background_blur;
		std::optional<EchoCancellationConstraint> echo_cancellation;
		std::optional<BooleanConstraint> auto_gain_control;
		std::optional<BooleanConstraint> noise_suppression;
		std::optional<DoubleConstraint> latency;
		std::optional<UintConstraint> channel_count;
		std::optional<StringConstraint> device_id;
		std::optional<StringConstraint> group_id;
	};

	struct MediaTrackConstraints
	{
		MediaTrackConstraintSet basic;
		std::vector<MediaTrackConstraintSet> advanced;
	};

	struct MediaStreamConstraints
	{
		bool audio_requested = false;
		bool video_requested = false;
		std::optional<MediaTrackConstraints> audio_constraints;
		std::optional<MediaTrackConstraints> video_constraints;
	};
}

namespace msclr { namespace interop
{
	inline WebRtcInterop::Marshaling::StringConstraint marshal_as(WebRtcNet::Media::MediaTrackConstraints::StringConstraint^ from)
	{
		WebRtcInterop::Marshaling::StringConstraint to{};
		if (from == nullptr)
			return to;

		if (from->Ideal != nullptr)
		{
			auto ideal = from->Ideal;
			to.ideal = marshal_as<std::string>(ideal);
		}
		if (from->Exact != nullptr)
		{
			auto exact = from->Exact;
			to.exact = marshal_as<std::string>(exact);
		}

		return to;
	}

	inline WebRtcInterop::Marshaling::BooleanConstraint marshal_as(WebRtcNet::Media::MediaTrackConstraints::Constraint<bool>^ from)
	{
		WebRtcInterop::Marshaling::BooleanConstraint to{};
		if (from == nullptr)
			return to;

		if (from->Ideal.HasValue)
			to.ideal = from->Ideal.Value;
		if (from->Exact.HasValue)
			to.exact = from->Exact.Value;

		return to;
	}

	inline WebRtcInterop::Marshaling::UintConstraint marshal_as(WebRtcNet::Media::MediaTrackConstraints::UIntRangeConstraint^ from)
	{
		WebRtcInterop::Marshaling::UintConstraint to{};
		if (from == nullptr)
			return to;

		if (from->Ideal.HasValue)
			to.ideal = from->Ideal.Value;
		if (from->Exact.HasValue)
			to.exact = from->Exact.Value;
		if (from->Min.HasValue)
			to.min = from->Min.Value;
		if (from->Max.HasValue)
			to.max = from->Max.Value;

		return to;
	}

	inline WebRtcInterop::Marshaling::DoubleConstraint marshal_as(WebRtcNet::Media::MediaTrackConstraints::DoubleRangeConstraint^ from)
	{
		WebRtcInterop::Marshaling::DoubleConstraint to{};
		if (from == nullptr)
			return to;

		if (from->Ideal.HasValue)
			to.ideal = from->Ideal.Value;
		if (from->Exact.HasValue)
			to.exact = from->Exact.Value;
		if (from->Min.HasValue)
			to.min = from->Min.Value;
		if (from->Max.HasValue)
			to.max = from->Max.Value;

		return to;
	}

	inline WebRtcInterop::Marshaling::EchoCancellationValue marshal_as(WebRtcNet::Media::EchoCancellationValue from)
	{
		WebRtcInterop::Marshaling::EchoCancellationValue to{};
		if (from.IsBoolean)
		{
			to.boolean_value = from.BooleanValue.GetValueOrDefault();
			return to;
		}

		if (from.IsMode)
		{
			to.mode_value = marshal_as<std::string>(from.ModeValue);
			return to;
		}

		throw gcnew System::InvalidCastException("EchoCancellationValue must contain either a boolean value or a mode value.");
	}

	inline WebRtcInterop::Marshaling::EchoCancellationConstraint marshal_as(WebRtcNet::Media::EchoCancellationConstraint^ from)
	{
		WebRtcInterop::Marshaling::EchoCancellationConstraint to{};
		if (from == nullptr)
			return to;

		if (from->Ideal.HasValue)
			to.ideal = marshal_as(from->Ideal.Value);
		if (from->Exact.HasValue)
			to.exact = marshal_as(from->Exact.Value);

		return to;
	}

	inline WebRtcInterop::Marshaling::MediaTrackConstraintSet marshal_as(WebRtcNet::Media::MediaTrackConstraintSet^ from)
	{
		WebRtcInterop::Marshaling::MediaTrackConstraintSet to{};
		if (from == nullptr)
			return to;

		if (from->Width != nullptr)
			to.width = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::UIntRangeConstraint^>(from->Width));
		if (from->Height != nullptr)
			to.height = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::UIntRangeConstraint^>(from->Height));
		if (from->AspectRatio != nullptr)
			to.aspect_ratio = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::DoubleRangeConstraint^>(from->AspectRatio));
		if (from->FrameRate != nullptr)
			to.frame_rate = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::DoubleRangeConstraint^>(from->FrameRate));

		if (from->FacingMode != nullptr)
		{
			WebRtcInterop::Marshaling::StringConstraint facing{};
			if (from->FacingMode->Ideal.HasValue)
				facing.ideal = marshal_video_facing_mode_value_to_native(from->FacingMode->Ideal.Value);
			if (from->FacingMode->Exact.HasValue)
				facing.exact = marshal_video_facing_mode_value_to_native(from->FacingMode->Exact.Value);
			to.facing_mode = facing;
		}

		if (from->ResizeMode != nullptr)
		{
			WebRtcInterop::Marshaling::StringConstraint resize{};
			if (from->ResizeMode->Ideal.HasValue)
				resize.ideal = marshal_video_resize_mode_value_to_native(from->ResizeMode->Ideal.Value);
			if (from->ResizeMode->Exact.HasValue)
				resize.exact = marshal_video_resize_mode_value_to_native(from->ResizeMode->Exact.Value);
			to.resize_mode = resize;
		}

		if (from->SampleRate != nullptr)
			to.sample_rate = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::UIntRangeConstraint^>(from->SampleRate));
		if (from->SampleSize != nullptr)
			to.sample_size = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::UIntRangeConstraint^>(from->SampleSize));
		if (from->BackgroundBlur != nullptr)
			to.background_blur = marshal_as(from->BackgroundBlur);
		if (from->EchoCancellation != nullptr)
			to.echo_cancellation = marshal_as(from->EchoCancellation);
		if (from->AutoGainControl != nullptr)
			to.auto_gain_control = marshal_as(from->AutoGainControl);
		if (from->NoiseSuppression != nullptr)
			to.noise_suppression = marshal_as(from->NoiseSuppression);
		if (from->Latency != nullptr)
			to.latency = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::DoubleRangeConstraint^>(from->Latency));
		if (from->ChannelCount != nullptr)
			to.channel_count = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraints::UIntRangeConstraint^>(from->ChannelCount));
		if (from->DeviceId != nullptr)
			to.device_id = marshal_as(from->DeviceId);
		if (from->GroupId != nullptr)
			to.group_id = marshal_as(from->GroupId);

		return to;
	}

	inline WebRtcInterop::Marshaling::MediaTrackConstraints marshal_as(WebRtcNet::Media::MediaTrackConstraints^ from)
	{
		WebRtcInterop::Marshaling::MediaTrackConstraints to{};
		if (from == nullptr)
			return to;

		to.basic = marshal_as(safe_cast<WebRtcNet::Media::MediaTrackConstraintSet^>(from));

		if (from->Advanced != nullptr)
		{
			for each (auto advanced_constraint in from->Advanced)
			{
				to.advanced.push_back(marshal_as(advanced_constraint));
			}
		}

		return to;
	}

	inline WebRtcInterop::Marshaling::MediaStreamConstraints marshal_as(WebRtcNet::Media::MediaStreamConstraints^ from)
	{
		WebRtcInterop::Marshaling::MediaStreamConstraints to{};
		if (from == nullptr)
			return to;

		to.audio_requested = from->Audio;
		to.video_requested = from->Video;

		if (from->AudioConstraints != nullptr)
			to.audio_constraints = marshal_as(from->AudioConstraints);
		if (from->VideoConstraints != nullptr)
			to.video_constraints = marshal_as(from->VideoConstraints);

		return to;
	}
}}