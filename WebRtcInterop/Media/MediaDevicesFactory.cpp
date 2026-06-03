#include "pch.h"

#include "MediaDevicesFactory.h"
#include "MediaDevices.h"

namespace WebRtcInterop::Media
{
	WebRtcNet::Media::MediaDevices^ MediaDevicesFactory::CreateMediaDevices()
	{
		return gcnew MediaDevices();
	}
}
