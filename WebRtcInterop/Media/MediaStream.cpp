#include "pch.h"

#include "MediaStream.h"
#include "MediaStreamTrack.h"

#include <api/media_stream_interface.h>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;
using namespace WebRtcNet;

namespace WebRtcInterop::Media
{
	MediaStream::MediaStream(WebRtcNet::Media::MediaStream^ stream)
		: _rpMediaStreamInterface(nullptr),
		  on_active_(nullptr),
		  on_inactive_(nullptr),
		  on_add_track_(nullptr),
		  on_remove_track_(nullptr)
	{
		auto interop_stream = dynamic_cast<MediaStream^>(stream);
		if (interop_stream == nullptr)
		{
			throw gcnew InvalidCastException("The provided stream must be a WebRtcInterop::Media::MediaStream instance.");
		}

		auto native_stream = interop_stream->GetNativeMediaStreamInterface(true);
		_rpMediaStreamInterface = new webrtc::scoped_refptr(
			reinterpret_cast<webrtc::MediaStreamInterface*>(native_stream.ToPointer()));
	}

	MediaStream::MediaStream(webrtc::scoped_refptr<webrtc::MediaStreamInterface> stream)
		: _rpMediaStreamInterface(new webrtc::scoped_refptr(stream)),
		  on_active_(nullptr),
		  on_inactive_(nullptr),
		  on_add_track_(nullptr),
		  on_remove_track_(nullptr)
	{
	}

	MediaStream::~MediaStream()
	{
		this->!MediaStream();
	}

	MediaStream::!MediaStream()
	{
		delete _rpMediaStreamInterface;
		_rpMediaStreamInterface = nullptr;
	}

	IntPtr MediaStream::GetNativeMediaStreamInterface(bool throwOnDisposed)
	{
		if (_rpMediaStreamInterface == nullptr || _rpMediaStreamInterface->get() == nullptr)
		{
			if (throwOnDisposed) throw gcnew ObjectDisposedException(NAMEOF(MediaStream));
			return IntPtr::Zero;
		}

		return IntPtr(_rpMediaStreamInterface->get());
	}

	String^ MediaStream::Id::get()
	{
		const auto native = _rpMediaStreamInterface->get();
		return marshal_as<String^>(native->id());
	}

	IEnumerable<WebRtcNet::Media::MediaStreamTrack^>^ MediaStream::GetAudioTracks()
	{
		const auto native = _rpMediaStreamInterface->get();
		auto tracks = gcnew List<WebRtcNet::Media::MediaStreamTrack^>();

		for (const auto& track : native->GetAudioTracks())
		{
			tracks->Add(gcnew MediaStreamTrack(track));
		}

		return tracks;
	}

	IEnumerable<WebRtcNet::Media::MediaStreamTrack^>^ MediaStream::GetVideoTracks()
	{
		const auto native = _rpMediaStreamInterface->get();
		auto tracks = gcnew List<WebRtcNet::Media::MediaStreamTrack^>();

		for (const auto& track : native->GetVideoTracks())
		{
			tracks->Add(gcnew MediaStreamTrack(track));
		}

		return tracks;
	}

	IEnumerable<WebRtcNet::Media::MediaStreamTrack^>^ MediaStream::GetTracks()
	{
		auto tracks = gcnew List<WebRtcNet::Media::MediaStreamTrack^>();
		for each (auto track in GetAudioTracks()) tracks->Add(track);
		for each (auto track in GetVideoTracks()) tracks->Add(track);
		return tracks;
	}

	WebRtcNet::Media::MediaStreamTrack^ MediaStream::GetTrackById(String^ trackId)
	{
		if (trackId == nullptr) throw gcnew ArgumentNullException(NAMEOF(trackId));

		const auto native = _rpMediaStreamInterface->get();
		const auto native_track_id = marshal_as<std::string>(trackId);

		const auto audio = native->FindAudioTrack(native_track_id);
		if (audio != nullptr) return gcnew MediaStreamTrack(audio);

		const auto video = native->FindVideoTrack(native_track_id);
		if (video != nullptr) return gcnew MediaStreamTrack(video);

		return nullptr;
	}

	void MediaStream::AddTrack(WebRtcNet::Media::MediaStreamTrack^ track)
	{
		if (track == nullptr) throw gcnew ArgumentNullException(NAMEOF(track));

		const auto native_stream = _rpMediaStreamInterface->get();
		const auto native_track = reinterpret_cast<webrtc::MediaStreamTrackInterface*>(
			track->GetNativeMediaStreamTrackInterface(true).ToPointer());

		if (native_track->kind() == webrtc::MediaStreamTrackInterface::kAudioKind)
		{
			native_stream->AddTrack(
				webrtc::scoped_refptr(static_cast<webrtc::AudioTrackInterface*>(native_track)));
		}
		else if (native_track->kind() == webrtc::MediaStreamTrackInterface::kVideoKind)
		{
			native_stream->AddTrack(
				webrtc::scoped_refptr(static_cast<webrtc::VideoTrackInterface*>(native_track)));
		}
		else
		{
			throw gcnew InvalidCastException(
				String::Format("Unsupported MediaStreamTrack kind '{0}'.", marshal_as<String^>(native_track->kind())));
		}

		if (on_add_track_ != nullptr) on_add_track_(this, track);
	}

	void MediaStream::RemoveTrack(WebRtcNet::Media::MediaStreamTrack^ track)
	{
		if (track == nullptr) throw gcnew ArgumentNullException(NAMEOF(track));

		const auto native_stream = _rpMediaStreamInterface->get();
		const auto native_track = reinterpret_cast<webrtc::MediaStreamTrackInterface*>(
			track->GetNativeMediaStreamTrackInterface(true).ToPointer());

		if (native_track->kind() == webrtc::MediaStreamTrackInterface::kAudioKind)
		{
			native_stream->RemoveTrack(
				webrtc::scoped_refptr(static_cast<webrtc::AudioTrackInterface*>(native_track)));
		}
		else if (native_track->kind() == webrtc::MediaStreamTrackInterface::kVideoKind)
		{
			native_stream->RemoveTrack(
				webrtc::scoped_refptr(static_cast<webrtc::VideoTrackInterface*>(native_track)));
		}
		else
		{
			throw gcnew InvalidCastException(
				String::Format("Unsupported MediaStreamTrack kind '{0}'.", marshal_as<String^>(native_track->kind())));
		}

		if (on_remove_track_ != nullptr) on_remove_track_(this, track);
	}

	WebRtcNet::Media::MediaStream^ MediaStream::Clone()
	{
		return gcnew MediaStream(this);
	}

	Boolean MediaStream::Active::get()
	{
		const auto native = _rpMediaStreamInterface->get();

		for (const auto& track : native->GetAudioTracks())
		{
			if (track != nullptr && track->state() == webrtc::MediaStreamTrackInterface::kLive) return true;
		}

		for (const auto& track : native->GetVideoTracks())
		{
			if (track != nullptr && track->state() == webrtc::MediaStreamTrackInterface::kLive) return true;
		}

		return false;
	}
}
