#pragma once

#include <msclr/marshal.h>

#include "../Marshaling/MarshalEnums.h"

namespace msclr::interop
{
	/// <summary>
	/// Marshals EchoCancellationValue from a boolean.
	/// </summary>
	inline WebRtcNet::Media::EchoCancellationValue MarshalEchoCancellationValue(bool value)
	{
		return WebRtcNet::Media::EchoCancellationValue(value);
	}

	/// <summary>
	/// Marshals EchoCancellationValue from a mode string.
	/// </summary>
	inline WebRtcNet::Media::EchoCancellationValue MarshalEchoCancellationValue(const std::string& modeStr)
	{
		return WebRtcNet::Media::EchoCancellationValue(marshal_as<System::String^>(modeStr));
	}

	/// <summary>
	/// Marshals a nullable value to a ValueRange<T> by setting both Min and Max.
	/// If the value is null, returns an empty range.
	/// </summary>
	template <typename T>
	inline WebRtcNet::ValueRange<T>^ MarshalToValueRange(System::Nullable<T> value)
	{
		auto range = gcnew WebRtcNet::ValueRange<T>();
		if (value.HasValue)
		{
			range->Min = value;
			range->Max = value;
		}
		return range;
	}

	/// <summary>
	/// Marshals a range (min, max) to a ValueRange<T>.
	/// If both are invalid/unset, returns an empty range.
	/// </summary>
	template <typename T>
	inline WebRtcNet::ValueRange<T>^ MarshalToValueRange(System::Nullable<T> min, System::Nullable<T> max)
	{
		auto range = gcnew WebRtcNet::ValueRange<T>();
		range->Min = min;
		range->Max = max;
		return range;
	}

	/// <summary>
	/// Marshals a native double range to a managed ValueRange<double>.
	/// Used for video frame rate, audio latency, aspect ratio.
	/// </summary>
	template<>
	inline WebRtcNet::ValueRange<double>^ marshal_as(const webrtc::DoubleRange& from)
	{
		return MarshalToValueRange<double>(
			from.min() > 0.0 ? from.min() : System::Nullable<double>(),
			from.max() > 0.0 ? from.max() : System::Nullable<double>()
		);
	}

	/// <summary>
	/// Marshals a native int range to a managed ValueRange<uint>.
	/// Used for video width/height, audio sample rate/size, channel count.
	/// </summary>
	template<>
	inline WebRtcNet::ValueRange<uint>^ marshal_as(const webrtc::IntRange& from)
	{
		return MarshalToValueRange<uint>(
			from.min() > 0 ? static_cast<uint>(from.min()) : System::Nullable<uint>(),
			from.max() > 0 ? static_cast<uint>(from.max()) : System::Nullable<uint>()
		);
	}

	// VideoFacingMode enum mappings
	static const std::map<const std::string, const WebRtcNet::Media::VideoFacingModes> video_facing_mode_map{
		{"user", WebRtcNet::Media::VideoFacingModes::User},
		{"environment", WebRtcNet::Media::VideoFacingModes::Environment},
		{"left", WebRtcNet::Media::VideoFacingModes::Left},
		{"right", WebRtcNet::Media::VideoFacingModes::Right},
	};

	template<>
	inline WebRtcNet::Media::VideoFacingModes marshal_as(const std::string& from)
	{
		auto entry = video_facing_mode_map.find(from);
		if (entry != video_facing_mode_map.end())
			return entry->second;

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert video facing mode '{0}' to {1}.",
				marshal_as<System::String^>(from),
				WebRtcNet::Media::VideoFacingModes::typeid->FullName));
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::VideoFacingModes>(
		const WebRtcNet::Media::VideoFacingModes& from)
	{
		for (auto [key, value] : video_facing_mode_map)
		{
			if (value == from) return key;
		}

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert {0} value '{1}' to native video facing mode.",
				WebRtcNet::Media::VideoFacingModes::typeid->FullName,
				System::Enum::GetName(WebRtcNet::Media::VideoFacingModes::typeid, from)));
	}

	// VideoResizeMode enum mappings
	static const std::map<const std::string, const WebRtcNet::Media::VideoResizeModes> video_resize_mode_map{
		{"none", WebRtcNet::Media::VideoResizeModes::None},
		{"crop-and-scale", WebRtcNet::Media::VideoResizeModes::CropAndScale},
	};

	template<>
	inline WebRtcNet::Media::VideoResizeModes marshal_as(const std::string& from)
	{
		auto entry = video_resize_mode_map.find(from);
		if (entry != video_resize_mode_map.end())
			return entry->second;

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert video resize mode '{0}' to {1}.",
				marshal_as<System::String^>(from),
				WebRtcNet::Media::VideoResizeModes::typeid->FullName));
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::VideoResizeModes>(
		const WebRtcNet::Media::VideoResizeModes& from)
	{
		for (auto [key, value] : video_resize_mode_map)
		{
			if (value == from) return key;
		}

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert {0} value '{1}' to native video resize mode.",
				WebRtcNet::Media::VideoResizeModes::typeid->FullName,
				System::Enum::GetName(WebRtcNet::Media::VideoResizeModes::typeid, from)));
	}

	// EchoCancellationMode enum mappings
	static const std::map<const std::string, const WebRtcNet::Media::EchoCancellationMode> echo_cancellation_mode_map{
		{"all", WebRtcNet::Media::EchoCancellationMode::All},
		{"remote-only", WebRtcNet::Media::EchoCancellationMode::RemoteOnly},
	};

	template<>
	inline WebRtcNet::Media::EchoCancellationMode marshal_as(const std::string& from)
	{
		auto entry = echo_cancellation_mode_map.find(from);
		if (entry != echo_cancellation_mode_map.end())
			return entry->second;

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert echo cancellation mode '{0}' to {1}.",
				marshal_as<System::String^>(from),
				WebRtcNet::Media::EchoCancellationMode::typeid->FullName));
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::EchoCancellationMode>(
		const WebRtcNet::Media::EchoCancellationMode& from)
	{
		for (auto [key, value] : echo_cancellation_mode_map)
		{
			if (value == from) return key;
		}

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert {0} value '{1}' to native echo cancellation mode.",
				WebRtcNet::Media::EchoCancellationMode::typeid->FullName,
				System::Enum::GetName(WebRtcNet::Media::EchoCancellationMode::typeid, from)));
	}
}
