#pragma once

#include "talk\app\webrtc\mediaconstraintsinterface.h"
#include "talk\app\webrtc\test\fakeconstraints.h"

#include "MarshalCollections.h"

#include <msclr\marshal.h>

namespace msclr {
	namespace interop
	{
		template<>
		inline webrtc::FakeConstraints marshal_as(WebRtcNet::MediaConstraints ^ const & from)
		{
			if (from == nullptr) throw gcnew ArgumentNullException("from");
			webrtc::FakeConstraints to;

			for each (auto constraint in from->Mandatory)
			{
				auto key = constraint.Key;
				to.AddMandatory(marshal_as<std::string>(key), marshal_as<std::string>(constraint.Value));
			}

			for each (auto constraint in from->Optional)
			{
				auto key = constraint.Key;
				to.AddOptional(marshal_as<std::string>(key), marshal_as<std::string>(constraint.Value));
			}

			return to;
		};
	}
}