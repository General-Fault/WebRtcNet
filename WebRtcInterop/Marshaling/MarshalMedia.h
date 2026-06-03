#pragma once

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

namespace msclr { namespace interop
{
	// Marshal ValueRange<T> from native to managed
	// ValueRange<uint>: handles uint?, double?
	template<>
	inline WebRtcNet::ValueRange<uint>^ marshal_as(const std::pair<uint32_t, uint32_t>& from)
	{
		auto range = gcnew WebRtcNet::ValueRange<uint>();
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

	// Marshal MediaDeviceKind enum
	template<>
	inline WebRtcNet::Media::MediaDeviceKind marshal_as(webrtc::MediaDeviceInfo::Kind from)
	{
		switch (from)
		{
			case webrtc::MediaDeviceInfo::Kind::kAudioInput:
				return WebRtcNet::Media::MediaDeviceKind::AudioInput;
			case webrtc::MediaDeviceInfo::Kind::kAudioOutput:
				return WebRtcNet::Media::MediaDeviceKind::AudioOutput;
			case webrtc::MediaDeviceInfo::Kind::kVideoInput:
				return WebRtcNet::Media::MediaDeviceKind::VideoInput;
			default:
				throw gcnew System::ArgumentException("Unknown device kind");
		}
	}

	template<>
	inline webrtc::MediaDeviceInfo::Kind marshal_as(WebRtcNet::Media::MediaDeviceKind from)
	{
		switch (from)
		{
			case WebRtcNet::Media::MediaDeviceKind::AudioInput:
				return webrtc::MediaDeviceInfo::Kind::kAudioInput;
			case WebRtcNet::Media::MediaDeviceKind::AudioOutput:
				return webrtc::MediaDeviceInfo::Kind::kAudioOutput;
			case WebRtcNet::Media::MediaDeviceKind::VideoInput:
				return webrtc::MediaDeviceInfo::Kind::kVideoInput;
			default:
				throw gcnew System::ArgumentException("Unknown device kind");
		}
	}

	// Marshal MediaDeviceInfo: native to managed (one-way)
	template<>
	inline WebRtcNet::Media::MediaDeviceInfo^ marshal_as(const webrtc::MediaDeviceInfo& from)
	{
		auto deviceId = marshal_as<System::String^>(from.device_id());
		auto kind = marshal_as<WebRtcNet::Media::MediaDeviceKind>(from.kind());
		auto label = marshal_as<System::String^>(from.label());
		auto groupId = marshal_as<System::String^>(from.group_id());

		// Use reflection to call internal constructor
		auto ctor = WebRtcNet::Media::MediaDeviceInfo::typeid->GetConstructor(
			System::Reflection::BindingFlags::NonPublic | System::Reflection::BindingFlags::Instance,
			nullptr,
			gcnew array<System::Type^> { 
				System::String::typeid,
				WebRtcNet::Media::MediaDeviceKind::typeid,
				System::String::typeid,
				System::String::typeid
			},
			nullptr);

		if (ctor == nullptr)
			throw gcnew System::InvalidOperationException("Cannot find internal MediaDeviceInfo constructor");

		return safe_cast<WebRtcNet::Media::MediaDeviceInfo^>(
			ctor->Invoke(gcnew array<System::Object^> { deviceId, kind, label, groupId }));
	}

	// Marshal MediaStreamTrackState enum
	template<>
	inline WebRtcNet::Media::MediaStreamTrackState marshal_as(webrtc::MediaStreamTrackInterface::TrackState from)
	{
		switch (from)
		{
			case webrtc::MediaStreamTrackInterface::TrackState::kLive:
				return WebRtcNet::Media::MediaStreamTrackState::Live;
			case webrtc::MediaStreamTrackInterface::TrackState::kEnded:
				return WebRtcNet::Media::MediaStreamTrackState::Ended;
			default:
				throw gcnew System::ArgumentException("Unknown track state");
		}
	}

	template<>
	inline webrtc::MediaStreamTrackInterface::TrackState marshal_as(WebRtcNet::Media::MediaStreamTrackState from)
	{
		switch (from)
		{
			case WebRtcNet::Media::MediaStreamTrackState::Live:
				return webrtc::MediaStreamTrackInterface::TrackState::kLive;
			case WebRtcNet::Media::MediaStreamTrackState::Ended:
				return webrtc::MediaStreamTrackInterface::TrackState::kEnded;
			default:
				throw gcnew System::ArgumentException("Unknown track state");
		}
	}
}}
