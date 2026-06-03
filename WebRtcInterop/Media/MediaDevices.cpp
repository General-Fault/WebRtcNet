#include "pch.h"

#include "MediaDevices.h"

using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;

namespace WebRtcInterop::Media
{
	Task<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>^ MediaDevices::EnumerateDevices()
	{
		auto devices = gcnew List<WebRtcNet::Media::MediaDeviceInfo^>();
		return Task::FromResult<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>(devices);
	}

	WebRtcNet::Media::MediaTrackSupportedConstraints^ MediaDevices::GetSupportedConstraints()
	{
		return gcnew WebRtcNet::Media::MediaTrackSupportedConstraints();
	}

	Task<WebRtcNet::Media::MediaStream^>^ MediaDevices::GetUserMedia(WebRtcNet::Media::MediaStreamConstraints^ constraints)
	{
		return Task::FromException<WebRtcNet::Media::MediaStream^>(
			gcnew WebRtcNet::Media::MediaStreamException("GetUserMedia is not currently implemented."));
	}
}
