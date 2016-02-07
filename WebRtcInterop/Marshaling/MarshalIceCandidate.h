#pragma once

#include "talk\app\webrtc\jsep.h"
#include "webrtc\p2p\base\candidate.h"

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

namespace msclr { namespace interop
{
	template<>
	inline WebRtcNet::RtcIceCandidate marshal_as(const webrtc::IceCandidateInterface* const & from)
	{
		if (from == nullptr) throw gcnew System::ArgumentNullException("from");

		WebRtcNet::RtcIceCandidate to;
		to.Candidate = marshal_as<System::String ^>(from->candidate().ToString());
		to.SdpMLineIndex = from->sdp_mline_index();
		to.SdpMid = marshal_as<System::String ^>(from->sdp_mid());

		return to;
	}
}}