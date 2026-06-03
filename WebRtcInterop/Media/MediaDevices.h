#pragma once

namespace WebRtcInterop::Media
{
	using namespace System;
	using namespace System::Collections::Generic;
	using namespace System::Threading::Tasks;

	public ref class MediaDevices : WebRtcNet::Media::MediaDevices
	{
	public:
		virtual event EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ OnDeviceChange
		{
			void add(EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ value) override { on_device_change_ += value; }
			void remove(EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ value) override { on_device_change_ -= value; }
		}

		virtual Task<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>^ EnumerateDevices() override;
		virtual WebRtcNet::Media::MediaTrackSupportedConstraints^ GetSupportedConstraints() override;
		virtual Task<WebRtcNet::Media::MediaStream^>^ GetUserMedia(WebRtcNet::Media::MediaStreamConstraints^ constraints) override;

	private:
		EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ on_device_change_;
	};
}
