#pragma once

#include <mmdeviceapi.h>

#include <msclr/marshal.h>

#include "../Marshaling/MarshalEnums.h"

namespace msclr::interop
{
	static const std::map<const EDataFlow, const WebRtcNet::Media::MediaDeviceKind> e_data_flow_map{
		{eCapture, WebRtcNet::Media::MediaDeviceKind::AudioInput},
		{eRender,  WebRtcNet::Media::MediaDeviceKind::AudioOutput},
	};

	template<>
	inline WebRtcNet::Media::MediaDeviceKind marshal_as(const EDataFlow& from)
	{
		return marshal_mapped_native_type(e_data_flow_map, from);
	}
}
