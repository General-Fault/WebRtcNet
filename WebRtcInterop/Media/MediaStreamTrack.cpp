#include "pch.h"

#include "MediaStreamTrack.h"

#include <api/media_stream_interface.h>
#include <api/audio_options.h>

#include "Marshaling/MarshalMediaConstraints.h"

using namespace System;
using namespace WebRtcNet;
using namespace WebRtcNet::Media;

namespace
{
	Nullable<VideoFacingModeValue> ResolveFacingMode(MediaTrackConstraints^ constraints)
	{
		if (constraints == nullptr || constraints->FacingMode == nullptr)
			return System::Nullable<VideoFacingModeValue>();

		if (constraints->FacingMode->Exact.HasValue)
			return constraints->FacingMode->Exact.Value;
		if (constraints->FacingMode->Ideal.HasValue)
			return constraints->FacingMode->Ideal.Value;

		return System::Nullable<VideoFacingModeValue>();
	}

	Nullable<VideoResizeModeValue> ResolveResizeMode(MediaTrackConstraints^ constraints)
	{
		if (constraints == nullptr || constraints->ResizeMode == nullptr)
			return System::Nullable<VideoResizeModeValue>();

		if (constraints->ResizeMode->Exact.HasValue)
			return constraints->ResizeMode->Exact.Value;
		if (constraints->ResizeMode->Ideal.HasValue)
			return constraints->ResizeMode->Ideal.Value;

		return System::Nullable<VideoResizeModeValue>();
	}

	ValueRange<unsigned int>^ CreateSingleUIntRange(int value)
	{
		if (value <= 0)
			return nullptr;

		auto range = gcnew ValueRange<unsigned int>();
		range->Min = static_cast<unsigned int>(value);
		range->Max = static_cast<unsigned int>(value);
		return range;
	}
}

namespace WebRtcInterop::Media
{
	MediaStreamTrack::MediaStreamTrack() 
		: _nativeMediaStreamTrackInterface(nullptr),
			applied_constraints_(gcnew MediaTrackConstraints()),
			on_mute_(nullptr),
			on_unmute_(nullptr),
			on_ended_(nullptr)
	{
	}

	MediaStreamTrack::MediaStreamTrack(webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface> track) 
		: _nativeMediaStreamTrackInterface(gcnew NativeWrapper<webrtc::MediaStreamTrackInterface>(track)),
			applied_constraints_(gcnew MediaTrackConstraints()),
			on_mute_(nullptr),
			on_unmute_(nullptr),
			on_ended_(nullptr)
	{	  
	}


	MediaStreamTrack::~MediaStreamTrack()
	{
		this->!MediaStreamTrack();
	}

	MediaStreamTrack::!MediaStreamTrack()
	{
		delete _nativeMediaStreamTrackInterface;
		_nativeMediaStreamTrackInterface = nullptr;
	}

	webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface> MediaStreamTrack::GetNativeMediaStreamTrackInterface(bool throwOnDisposed)
	{
		if (_nativeMediaStreamTrackInterface == nullptr || !_nativeMediaStreamTrackInterface->HasValue())
		{
			if (throwOnDisposed) throw gcnew ObjectDisposedException("MediaStreamTrack");
			return webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface>();
		}

		return _nativeMediaStreamTrackInterface->GetScopedRef();
	}

	MediaStreamTrackKind MediaStreamTrack::Kind::get()
	{
		const auto native = _nativeMediaStreamTrackInterface->Get();
		return marshal_as<MediaStreamTrackKind>(native->kind());
	}

	String^ MediaStreamTrack::Id::get()
	{
		const auto native = _nativeMediaStreamTrackInterface->Get();
		return marshal_as<String^>(native->id());
	}

	String^ MediaStreamTrack::Label::get()
	{
		return String::Empty;
	}

	bool MediaStreamTrack::Enabled::get()
	{
		const auto native = _nativeMediaStreamTrackInterface->Get();
		return native->enabled();
	}

	void MediaStreamTrack::Enabled::set(bool value)
	{
		const auto native = _nativeMediaStreamTrackInterface->Get();
		native->set_enabled(value);
	}

	bool MediaStreamTrack::Muted::get()
	{
		const auto native = _nativeMediaStreamTrackInterface->Get();

		if (native->kind() == webrtc::MediaStreamTrackInterface::kAudioKind)
		{
			const auto audio_track = static_cast<webrtc::AudioTrackInterface*>(native);
			const auto source = audio_track->GetSource();
			return source != nullptr && source->state() == webrtc::MediaSourceInterface::kMuted;
		}

		if (native->kind() == webrtc::MediaStreamTrackInterface::kVideoKind)
		{
			const auto video_track = static_cast<webrtc::VideoTrackInterface*>(native);
			const auto source = video_track->GetSource();
			return source != nullptr && source->state() == webrtc::MediaSourceInterface::kMuted;
		}

		return false;
	}

	MediaStreamTrackState MediaStreamTrack::ReadyState::get()
	{
		const auto native = _nativeMediaStreamTrackInterface->Get();
		return marshal_as<MediaStreamTrackState>(native->state());
	}

	WebRtcNet::Media::MediaStreamTrack^ MediaStreamTrack::Clone()
	{
		return gcnew MediaStreamTrack(_nativeMediaStreamTrackInterface->GetScopedRef());
	}

	void MediaStreamTrack::Stop()
	{
		if (on_ended_ != nullptr) on_ended_(this, EventArgs::Empty);
	}

	MediaTrackCapabilities^ MediaStreamTrack::GetCapabilities()
	{
		auto native = GetNativeMediaStreamTrackInterface(false);
		if (native == nullptr)
		{
			return MediaTrackCapabilities::Create(
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				nullptr,
				String::Empty,
				String::Empty);
		}

		auto width = static_cast<ValueRange<unsigned int>^>(nullptr);
		auto height = static_cast<ValueRange<unsigned int>^>(nullptr);
		auto autoGainControl = gcnew List<bool>();
		auto noiseSuppression = gcnew List<bool>();
		auto echoCancellation = gcnew List<EchoCancellationValue>();

		if (native->kind() == webrtc::MediaStreamTrackInterface::kVideoKind)
		{
			auto video_track = static_cast<webrtc::VideoTrackInterface*>(native.get());
			if (video_track != nullptr)
			{
				auto source = video_track->GetSource();
				if (source != nullptr)
				{
					webrtc::VideoTrackSourceInterface::Stats stats{};
					if (source->GetStats(&stats))
					{
						width = CreateSingleUIntRange(stats.input_width);
						height = CreateSingleUIntRange(stats.input_height);
					}
				}
			}
		}
		else if (native->kind() == webrtc::MediaStreamTrackInterface::kAudioKind)
		{
			auto audio_track = static_cast<webrtc::AudioTrackInterface*>(native.get());
			if (audio_track != nullptr)
			{
				auto source = audio_track->GetSource();
				if (source != nullptr)
				{
					auto options = source->options();
					if (options.echo_cancellation.has_value())
						echoCancellation->Add(EchoCancellationValue(options.echo_cancellation.value()));
					if (options.auto_gain_control.has_value())
						autoGainControl->Add(options.auto_gain_control.value());
					if (options.noise_suppression.has_value())
						noiseSuppression->Add(options.noise_suppression.value());
				}
			}
		}

		return MediaTrackCapabilities::Create(
			width,
			height,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			echoCancellation,
			nullptr,
			autoGainControl,
			noiseSuppression,
			nullptr,
			nullptr,
			Id != nullptr ? Id : String::Empty,
			String::Empty);
	}

	MediaTrackConstraints^ MediaStreamTrack::GetConstraints()
	{
		if (applied_constraints_ == nullptr)
			applied_constraints_ = gcnew MediaTrackConstraints();

		return applied_constraints_;
	}

	MediaTrackSettings^ MediaStreamTrack::GetSettings()
	{
		auto native = GetNativeMediaStreamTrackInterface(false);
		auto width = 0u;
		auto height = 0u;
		auto echoCancellation = EchoCancellationValue(false);
		Nullable<bool> autoGainControl;
		Nullable<bool> noiseSuppression;

		if (native != nullptr)
		{
			if (native->kind() == webrtc::MediaStreamTrackInterface::kVideoKind)
			{
				auto video_track = static_cast<webrtc::VideoTrackInterface*>(native.get());
				if (video_track != nullptr)
				{
					auto source = video_track->GetSource();
					if (source != nullptr)
					{
						webrtc::VideoTrackSourceInterface::Stats stats{};
						if (source->GetStats(&stats))
						{
							width = stats.input_width > 0 ? static_cast<unsigned int>(stats.input_width) : 0u;
							height = stats.input_height > 0 ? static_cast<unsigned int>(stats.input_height) : 0u;
						}
					}
				}
			}
			else if (native->kind() == webrtc::MediaStreamTrackInterface::kAudioKind)
			{
				auto audio_track = static_cast<webrtc::AudioTrackInterface*>(native.get());
				if (audio_track != nullptr)
				{
					auto source = audio_track->GetSource();
					if (source != nullptr)
					{
						auto options = source->options();
						if (options.echo_cancellation.has_value())
							echoCancellation = EchoCancellationValue(options.echo_cancellation.value());
						if (options.auto_gain_control.has_value())
							autoGainControl = options.auto_gain_control.value();
						if (options.noise_suppression.has_value())
							noiseSuppression = options.noise_suppression.value();
					}
				}
			}
		}

		return MediaTrackSettings::Create(
			width,
			height,
			0.0,
			0.0,
			ResolveFacingMode(applied_constraints_),
			ResolveResizeMode(applied_constraints_),
			0,
			0,
			echoCancellation,
			false,
			autoGainControl,
			noiseSuppression,
			0.0,
			0,
			Id != nullptr ? Id : String::Empty,
			String::Empty);
	}

	void MediaStreamTrack::ApplyConstraints(MediaTrackConstraints^ constraints)
	{
		if (constraints == nullptr)
		{
			applied_constraints_ = gcnew MediaTrackConstraints();
			return;
		}

		const auto marshaled_constraints = marshal_as(constraints);
		(void)marshaled_constraints;

		applied_constraints_ = constraints;
	}
}
