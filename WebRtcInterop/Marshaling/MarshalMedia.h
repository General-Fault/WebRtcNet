#pragma once

#include <api/media_stream_interface.h>
#include <api/media_stream_track.h>
#include <api/media_types.h>
#include <api/peer_connection_interface.h>

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

#include <map>
#include <string>

#include "MarshalEnums.h"

namespace msclr { namespace interop
{
	using namespace WebRtcNet::Logging;

	inline void WriteMediaInteropWarning(System::String^ message)
	{
		WebRtcLogWriterBridge::WriteInteropLog(
			3,
			9300,
			"Interop.Media.Marshaling",
			System::Threading::Thread::CurrentThread->ManagedThreadId,
			message != nullptr ? message : System::String::Empty);
	}

	static const std::map<const webrtc::MediaStreamTrackInterface::TrackState, const WebRtcNet::Media::MediaStreamTrackState>
		media_stream_track_state_map{
			{webrtc::MediaStreamTrackInterface::TrackState::kLive, WebRtcNet::Media::MediaStreamTrackState::Live},
			{webrtc::MediaStreamTrackInterface::TrackState::kEnded, WebRtcNet::Media::MediaStreamTrackState::Ended},
		};

	static const std::map<const std::string, const WebRtcNet::Media::VideoFacingModes> video_facing_mode_map{
		{"user", WebRtcNet::Media::VideoFacingModes::User},
		{"environment", WebRtcNet::Media::VideoFacingModes::Environment},
		{"left", WebRtcNet::Media::VideoFacingModes::Left},
		{"right", WebRtcNet::Media::VideoFacingModes::Right},
	};

	static const std::map<const std::string, const WebRtcNet::Media::VideoResizeModes> video_resize_mode_map{
		{"none", WebRtcNet::Media::VideoResizeModes::None},
		{"crop-and-scale", WebRtcNet::Media::VideoResizeModes::CropAndScale},
	};

	static const std::map<const std::string, const WebRtcNet::Media::EchoCancellationMode> echo_cancellation_mode_map{
		{"software", WebRtcNet::Media::EchoCancellationMode::Software},
		{"system", WebRtcNet::Media::EchoCancellationMode::System},
	};

	template<>
	inline WebRtcNet::Media::MediaStreamTrackState marshal_as(const webrtc::MediaStreamTrackInterface::TrackState& from)
	{
		return marshal_mapped_native_type(media_stream_track_state_map, from);
	}

	template<>
	inline webrtc::MediaStreamTrackInterface::TrackState
	marshal_as<webrtc::MediaStreamTrackInterface::TrackState, WebRtcNet::Media::MediaStreamTrackState>(
		const WebRtcNet::Media::MediaStreamTrackState& from)
	{
		return marshal_mapped_managed_type(media_stream_track_state_map, from);
	}

	template<>
	inline WebRtcNet::Media::MediaStreamTrackKind marshal_as(const std::string& from)
	{
		if (from == webrtc::MediaStreamTrackInterface::kAudioKind) return WebRtcNet::Media::MediaStreamTrackKind::Audio;
		if (from == webrtc::MediaStreamTrackInterface::kVideoKind) return WebRtcNet::Media::MediaStreamTrackKind::Video;

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert track kind '{0}' to {1}.",
				marshal_as<System::String^>(from),
				WebRtcNet::Media::MediaStreamTrackKind::typeid->FullName));
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::MediaStreamTrackKind>(
		const WebRtcNet::Media::MediaStreamTrackKind& from)
	{
		switch (from)
		{
		case WebRtcNet::Media::MediaStreamTrackKind::Audio:
			return webrtc::MediaStreamTrackInterface::kAudioKind;
		case WebRtcNet::Media::MediaStreamTrackKind::Video:
			return webrtc::MediaStreamTrackInterface::kVideoKind;
		}

		throw gcnew System::InvalidCastException(
			System::String::Format("Unable to convert {0} value '{1}' to native track kind.",
				WebRtcNet::Media::MediaStreamTrackKind::typeid->FullName,
				System::Enum::GetName(WebRtcNet::Media::MediaStreamTrackKind::typeid, from)));
	}

	template<>
	inline WebRtcNet::Media::VideoFacingModes marshal_as(const std::string& from)
	{
		return marshal_mapped_native_type(video_facing_mode_map, from);
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::VideoFacingModes>(
		const WebRtcNet::Media::VideoFacingModes& from)
	{
		return marshal_mapped_managed_type(video_facing_mode_map, from);
	}

	template<>
	inline WebRtcNet::Media::VideoResizeModes marshal_as(const std::string& from)
	{
		return marshal_mapped_native_type(video_resize_mode_map, from);
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::VideoResizeModes>(
		const WebRtcNet::Media::VideoResizeModes& from)
	{
		return marshal_mapped_managed_type(video_resize_mode_map, from);
	}

	template<>
	inline WebRtcNet::Media::VideoFacingModeValue marshal_as(const std::string& from)
	{
		auto entry = video_facing_mode_map.find(from);
		if (entry != video_facing_mode_map.end())
			return WebRtcNet::Media::VideoFacingModeValue(entry->second);

		WriteMediaInteropWarning(System::String::Format(
			"Unknown native facing mode '{0}' preserved as raw value.",
			marshal_as<System::String^>(from)));

		return WebRtcNet::Media::VideoFacingModeValue(marshal_as<System::String^>(from));
	}

	inline std::string marshal_video_facing_mode_value_to_native(WebRtcNet::Media::VideoFacingModeValue from)
	{
		if (!from.IsKnown)
		{
			throw gcnew System::InvalidCastException(
				System::String::Format("Unable to convert unknown facing mode value '{0}' to native facing mode.",
					from.RawValue));
		}

		auto knownValue = from.KnownValue;
		return marshal_as<std::string>(knownValue.Value);
	}

	template<>
	inline WebRtcNet::Media::VideoResizeModeValue marshal_as(const std::string& from)
	{
		auto entry = video_resize_mode_map.find(from);
		if (entry != video_resize_mode_map.end())
			return WebRtcNet::Media::VideoResizeModeValue(entry->second);

		WriteMediaInteropWarning(System::String::Format(
			"Unknown native resize mode '{0}' preserved as raw value.",
			marshal_as<System::String^>(from)));

		return WebRtcNet::Media::VideoResizeModeValue(marshal_as<System::String^>(from));
	}

	inline std::string marshal_video_resize_mode_value_to_native(WebRtcNet::Media::VideoResizeModeValue from)
	{
		if (!from.IsKnown)
		{
			throw gcnew System::InvalidCastException(
				System::String::Format("Unable to convert unknown resize mode value '{0}' to native resize mode.",
					from.RawValue));
		}

		auto knownValue = from.KnownValue;
		return marshal_as<std::string>(knownValue.Value);
	}

	template<>
	inline WebRtcNet::Media::EchoCancellationMode marshal_as(const std::string& from)
	{
		return marshal_mapped_native_type(echo_cancellation_mode_map, from);
	}

	template<>
	inline std::string marshal_as<std::string, WebRtcNet::Media::EchoCancellationMode>(
		const WebRtcNet::Media::EchoCancellationMode& from)
	{
		return marshal_mapped_managed_type(echo_cancellation_mode_map, from);
	}

	template<>
	inline WebRtcNet::Media::EchoCancellationValue marshal_as(const bool& from)
	{
		return WebRtcNet::Media::EchoCancellationValue(from);
	}

	template<>
	inline WebRtcNet::Media::EchoCancellationValue marshal_as(const std::string& from)
	{
		return WebRtcNet::Media::EchoCancellationValue(marshal_as<System::String^>(from));
	}

	template<>
	inline WebRtcNet::ValueRange<unsigned int>^ marshal_as(const std::pair<uint32_t, uint32_t>& from)
	{
		auto range = gcnew WebRtcNet::ValueRange<unsigned int>();
		range->Min = from.first;
		range->Max = from.second;
		return range;
	}

	template<>
	inline WebRtcNet::ValueRange<double>^ marshal_as(const std::pair<double, double>& from)
	{
		auto range = gcnew WebRtcNet::ValueRange<double>();
		range->Min = from.first;
		range->Max = from.second;
		return range;
	}

}}
