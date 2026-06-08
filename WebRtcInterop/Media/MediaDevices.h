#pragma once

namespace WebRtcInterop::Media
{
	using namespace System;
	using namespace System::Collections::Generic;
	using namespace System::Threading::Tasks;

	public ref class MediaDevices : WebRtcNet::Media::MediaDevices
	{
	public:
		MediaDevices();
		~MediaDevices();
		!MediaDevices();

		virtual event EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ OnDeviceChange
		{
			void add(EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ value) override { on_device_change_ += value; }
			void remove(EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ value) override { on_device_change_ -= value; }
		}

		Task<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>^ EnumerateDevices() override;
		WebRtcNet::Media::MediaTrackSupportedConstraints^ GetSupportedConstraints() override;
		Task<WebRtcNet::Media::MediaStream^>^ GetUserMedia(WebRtcNet::Media::MediaStreamConstraints^ constraints) override;

	private:
		void RefreshKnownDevices(bool raiseEvent);
		void OnDevicePoll(Object^ sender, Timers::ElapsedEventArgs^ args);
		void StopDevicePolling();

		Dictionary<String^, WebRtcNet::Media::MediaDeviceInfo^>^ known_devices_;
		Timers::Timer^ device_poll_timer_;
		Object^ device_poll_gate_;
		EventHandler<WebRtcNet::Media::DeviceChangeEventArgs^>^ on_device_change_;
	};
}
