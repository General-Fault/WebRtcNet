#pragma once

#include <api/scoped_refptr.h>

namespace webrtc
{
	class MediaStreamInterface;
}

namespace WebRtcInterop::Media
{
	using namespace System;
	using namespace System::Collections::Generic;
	using namespace System::Threading::Tasks;

	public ref class MediaStream : WebRtcNet::Media::MediaStream
	{
	public:
		/// Composes a new stream from the tracks of an existing stream.
		MediaStream(WebRtcNet::Media::MediaStream^ stream);
		~MediaStream();

		virtual property String^ Id { String^ get() override; }
		IEnumerable<WebRtcNet::Media::MediaStreamTrack^>^ GetAudioTracks() override;
		IEnumerable<WebRtcNet::Media::MediaStreamTrack^>^ GetVideoTracks() override;
		IEnumerable<WebRtcNet::Media::MediaStreamTrack^>^ GetTracks() override;
		WebRtcNet::Media::MediaStreamTrack^ GetTrackById(String^ trackId) override;
		void AddTrack(WebRtcNet::Media::MediaStreamTrack^ track) override;
		void RemoveTrack(WebRtcNet::Media::MediaStreamTrack^ track) override;
		WebRtcNet::Media::MediaStream^ Clone() override;
		virtual property Boolean Active { Boolean get() override; }

		virtual event EventHandler^ OnActive
		{
			void add(EventHandler^ value) override { on_active_ += value; }
			void remove(EventHandler^ value) override { on_active_ -= value; }
		}
		virtual event EventHandler^ OnInactive
		{
			void add(EventHandler^ value) override { on_inactive_ += value; }
			void remove(EventHandler^ value) override { on_inactive_ -= value; }
		}
		virtual event EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ OnAddTrack
		{
			void add(EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ value) override { on_add_track_ += value; }
			void remove(EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ value) override { on_add_track_ -= value; }
		}
		virtual event EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ OnRemoveTrack
		{
			void add(EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ value) override { on_remove_track_ += value; }
			void remove(EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ value) override { on_remove_track_ -= value; }
		}

	internal:
		MediaStream(webrtc::scoped_refptr<webrtc::MediaStreamInterface> stream);
		!MediaStream();

	public:
		IntPtr GetNativeMediaStreamInterface(bool throwOnDisposed) override;

	private:
		webrtc::scoped_refptr<webrtc::MediaStreamInterface>* _rpMediaStreamInterface;
		EventHandler^ on_active_;
		EventHandler^ on_inactive_;
		EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ on_add_track_;
		EventHandler<WebRtcNet::Media::MediaStreamTrack^>^ on_remove_track_;
	};

}
