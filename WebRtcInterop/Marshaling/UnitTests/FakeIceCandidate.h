#pragma once

#include "webrtc\base\scoped_ptr.h"
#include "talk\app\webrtc\jsep.h"
#include "webrtc\p2p\base\candidate.h"

namespace webrtc
{
	class IceCandidateInterface;
}

namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	class FakeIceCandidate : public webrtc::IceCandidateInterface
	{
	public:
		FakeIceCandidate(std::string& sdp_mid, int sdp_mline_index)
			: _sdp_mid(sdp_mid), _sdp_mline_index(sdp_mline_index),
			_candidate(1, "Fake", rtc::EmptySocketAddressWithFamily(1), 1, "username",
				"password", "FakeType", 0, "FakeFoundation")
		{
		}

		virtual std::string sdp_mid() const { return _sdp_mid; }
		virtual int sdp_mline_index() const { return _sdp_mline_index; }
		virtual const cricket::Candidate& candidate() const { return _candidate; }
		virtual bool ToString(std::string* out) const 
		{ 
			if (out == nullptr) return false; 
			*out = "Fake_IceCandidate"; 
			return true; 
		}

	private:
		std::string _sdp_mid;
		int _sdp_mline_index;
		cricket::Candidate _candidate;
	};
}}}