#include "pch.h"

#include "MediaStreamTrack.h"

#include <api/media_stream_interface.h>
#include <api/media_stream_track.h>

#include "Media/Marshaling/MarshalMedia.h"

using namespace System;
using namespace WebRtcNet;
using namespace WebRtcNet::Media;

namespace WebRtcInterop::Media
{
	MediaStreamTrack::MediaStreamTrack()
	{
		_rpMediaStreamTrackInterface = nullptr;
		applied_constraints_ = gcnew MediaTrackConstraints();
		on_mute_ = nullptr;
		on_unmute_ = nullptr;
		on_ended_ = nullptr;
	}

	MediaStreamTrack::MediaStreamTrack(webrtc::scoped_refptr<webrtc::MediaStreamTrackInterface> track)
	{
		_rpMediaStreamTrackInterface = new webrtc::scoped_refptr(track);
		applied_constraints_ = gcnew MediaTrackConstraints();
		on_mute_ = nullptr;
		on_unmute_ = nullptr;
		on_ended_ = nullptr;
	}


	MediaStreamTrack::~MediaStreamTrack()
	{
		this->!MediaStreamTrack();
	}

	MediaStreamTrack::!MediaStreamTrack()
	{
		delete _rpMediaStreamTrackInterface;
		_rpMediaStreamTrackInterface = nullptr;
	}

	IntPtr MediaStreamTrack::GetNativeMediaStreamTrackInterface(bool throwOnDisposed)
	{
		if (_rpMediaStreamTrackInterface == nullptr || _rpMediaStreamTrackInterface->get() == nullptr)
		{
			if (throwOnDisposed) throw gcnew ObjectDisposedException("MediaStreamTrack");
			return IntPtr::Zero;
		}

		return IntPtr(_rpMediaStreamTrackInterface->get());
	}

	MediaStreamTrackKind MediaStreamTrack::Kind::get()
	{
		const auto native = _rpMediaStreamTrackInterface->get();
		return marshal_as<MediaStreamTrackKind>(native->kind());
	}

	String^ MediaStreamTrack::Id::get()
	{
		const auto native = _rpMediaStreamTrackInterface->get();
		return marshal_as<String^>(native->id());
	}

	String^ MediaStreamTrack::Label::get()
	{
		return String::Empty;
	}

	bool MediaStreamTrack::Enabled::get()
	{
		const auto native = _rpMediaStreamTrackInterface->get();
		return native->enabled();
	}

	void MediaStreamTrack::Enabled::set(bool value)
	{
		const auto native = _rpMediaStreamTrackInterface->get();
		native->set_enabled(value);
	}

	bool MediaStreamTrack::Muted::get()
	{
		const auto native = _rpMediaStreamTrackInterface->get();

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
		const auto native = _rpMediaStreamTrackInterface->get();
		return marshal_as<MediaStreamTrackState>(native->state());
	}

	WebRtcNet::Media::MediaStreamTrack^ MediaStreamTrack::Clone()
	{
		return gcnew MediaStreamTrack(*_rpMediaStreamTrackInterface);
	}

	void MediaStreamTrack::Stop()
	{
		if (on_ended_ != nullptr) on_ended_(this, EventArgs::Empty);
	}

	MediaTrackCapabilities^ MediaStreamTrack::GetCapabilities()
	{
		return gcnew MediaTrackCapabilities();
	}

	MediaTrackConstraints^ MediaStreamTrack::GetConstraints()
	{
		if (applied_constraints_ == nullptr)
			applied_constraints_ = gcnew MediaTrackConstraints();

		return applied_constraints_;
	}

	MediaTrackSettings^ MediaStreamTrack::GetSettings()
	{
		return gcnew MediaTrackSettings();
	}

	void MediaStreamTrack::ApplyConstraints(MediaTrackConstraints^ constraints)
	{
		if (constraints == nullptr)
			applied_constraints_ = gcnew MediaTrackConstraints();
		else
			applied_constraints_ = constraints;
	}
}
