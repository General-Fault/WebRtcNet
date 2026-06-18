#pragma once

namespace WebRtcInterop::Media
{
	using namespace System;
	using namespace WebRtcNet::Media;

	ref class AudioCapabilityQuery sealed
	{
		initonly String^ endpoint_id_;

	public:
		AudioCapabilityQuery(String^ endpointId);
		MediaTrackCapabilities^ Query();
	};
}
