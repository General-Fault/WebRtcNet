#pragma once

#include "talk\app\webrtc\jsep.h"

#include <msclr\marshal.h>

namespace msclr {
	namespace interop
	{
		template<>
		inline WebRtcNet::RtcIceCandidate marshal_as(const webrtc::IceCandidateInterface* const & from)
		{
			WebRtcNet::RtcIceCandidate to;
			std::string candidateStr;
			from->ToString(&candidateStr);
			to.Candidate = marshal_as<System::String ^>(candidateStr);
			to.SdpMLineIndex = from->sdp_mline_index();
			to.SdpMid = marshal_as<System::String ^>(from->sdp_mid());

			return to;
		}
	}
}