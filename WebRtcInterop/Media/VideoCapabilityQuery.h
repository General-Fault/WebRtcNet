#pragma once

namespace WebRtcInterop::Media
{
	using namespace System;
	using namespace WebRtcNet::Media;

	ref class VideoCapabilityQuery sealed
	{
		initonly String^ device_id_;

	public:
		VideoCapabilityQuery(String^ deviceId);
		MediaTrackCapabilities^ Query();
	};
}
