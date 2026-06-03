#pragma once

namespace WebRtcInterop::Media
{
	public ref class MediaDevicesFactory sealed
	{
	public:
		static WebRtcNet::Media::MediaDevices^ CreateMediaDevices();
	};
}
