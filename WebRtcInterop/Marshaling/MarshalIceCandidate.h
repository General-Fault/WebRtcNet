#pragma once

#include "api/jsep.h"

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

namespace msclr { namespace interop
{
	template<>
	inline WebRtcNet::IRtcIceCandidate^ marshal_as(const webrtc::IceCandidateInterface* const & from)
	{
		if (from == nullptr) throw gcnew System::ArgumentNullException("from");

		WebRtcNet::IRtcIceCandidate^ to = gcnew WebRtcNet::IRtcIceCandidate
		to->Candidate = marshal_as<System::String ^>(from->candidate().ToString());
		to->SdpMLineIndex = from->sdp_mline_index();
		to->SdpMid = marshal_as<System::String ^>(from->sdp_mid());

		return to;
	}
}}