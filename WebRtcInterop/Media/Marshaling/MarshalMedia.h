#pragma once

#include <api/media_stream_interface.h>

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

#include <map>

#include "Marshaling/MarshalEnums.h"

namespace msclr::interop
{
	static const std::map<const webrtc::MediaStreamTrackInterface::TrackState, const WebRtcNet::Media::MediaStreamTrackState>
		media_stream_track_state_map{
			{webrtc::MediaStreamTrackInterface::TrackState::kLive, WebRtcNet::Media::MediaStreamTrackState::Live},
			{webrtc::MediaStreamTrackInterface::TrackState::kEnded, WebRtcNet::Media::MediaStreamTrackState::Ended},
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
}
