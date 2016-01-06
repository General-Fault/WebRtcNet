#include "stdafx.h"

#include "webrtc\base\scoped_ref_ptr.h"
#include "webrtc\base\scoped_ptr.h"
#include "talk\app\webrtc\mediastreaminterface.h"
#include "talk\media\devices\devicemanager.h"
#include "talk\app\webrtc\peerconnectionfactory.h"
#include "talk\app\webrtc\videosourceinterface.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;

#include "MediaStream.h"
#include "RtcPeerConnectionFactory.h"
#include "Marshaling\MarshalMediaConstraints.h"

using namespace WebRtcNet;


namespace WebRtcNet
{
	cricket::VideoCapturer* OpenVideoCaptureDevice()
	{
		rtc::scoped_ptr<cricket::DeviceManagerInterface> dev_manager(
			cricket::DeviceManagerFactory::Create());
		if (!dev_manager->Init())
		{
			throw gcnew MediaStreamException("Can't create device manager");
		}
		std::vector<cricket::Device> devs;
		if (!dev_manager->GetVideoCaptureDevices(&devs))
		{
			throw gcnew MediaStreamException("Can't enumerate video devices");
		}

		cricket::VideoCapturer* capturer = nullptr;
		for (auto dev : devs)
		{
			capturer = dev_manager->CreateVideoCapturer(dev);
			if (capturer != nullptr)
				break;
		}
		return capturer;
	}

	IMediaStream ^ Media::GetUserMedia(MediaConstraints ^ constraints)
	{
		auto peerConnectionFactory = RtcPeerConnectionFactory::Instance->GetNativePeerConnectionFactoryInterface(true);

		webrtc::FakeConstraints nativeConstraints(marshal_as<webrtc::FakeConstraints>(constraints));
		auto audioSource(peerConnectionFactory->CreateAudioSource(&nativeConstraints));
		auto audioTrack(peerConnectionFactory->CreateAudioTrack("audio_label", audioSource));

		auto videoSource(peerConnectionFactory->CreateVideoSource(OpenVideoCaptureDevice(), &nativeConstraints));
		auto videoTrack(peerConnectionFactory->CreateVideoTrack("video_label", videoSource));

		auto stream(peerConnectionFactory->CreateLocalMediaStream("stream_label"));

		stream->AddTrack(audioTrack);
		stream->AddTrack(videoTrack);

		return gcnew WebRtcInterop::MediaStream(stream.release());
	}
}

namespace WebRtcInterop {

MediaStream::MediaStream(MediaStream ^ & stream)
	: _rpMediaStreamInterface(nullptr)
{
	auto nativeStream = stream->GetNativeMediaStreamInterface(true);
	_rpMediaStreamInterface = new rtc::scoped_refptr<webrtc::MediaStreamInterface>(nativeStream);
}

MediaStream::MediaStream(webrtc::MediaStreamInterface * stream)
	: _rpMediaStreamInterface(new rtc::scoped_refptr<webrtc::MediaStreamInterface>(stream))
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

webrtc::MediaStreamInterface* MediaStream::GetNativeMediaStreamInterface(bool throwOnDisposed)
{
	if (_rpMediaStreamInterface == nullptr || _rpMediaStreamInterface->get() == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException("MediaStream");
		return nullptr;
	}

	return _rpMediaStreamInterface->get();
}


IEnumerable<IMediaStreamTrack^>^ MediaStream::GetAudioTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack^>^ MediaStream::GetVideoTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IEnumerable<IMediaStreamTrack^>^ MediaStream::GetTracks()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

IMediaStreamTrack ^ MediaStream::GetTrackById(String ^ trackId)
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

void MediaStream::AddTrack(IMediaStreamTrack ^ track)
{
	throw gcnew NotImplementedException();
}

void MediaStream::RemoveTrack(IMediaStreamTrack ^ track)
{
	throw gcnew NotImplementedException();
}

IMediaStream ^ MediaStream::Clone()
{
	throw gcnew NotImplementedException();
	// TODO: insert return statement here
}

}