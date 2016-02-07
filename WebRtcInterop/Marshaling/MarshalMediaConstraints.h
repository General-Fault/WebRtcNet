#pragma once

#include "talk\app\webrtc\mediaconstraintsinterface.h"
#include "talk\app\webrtc\test\fakeconstraints.h"

#include "MarshalCollections.h"

#include <msclr\marshal.h>

namespace msclr { namespace interop
{
	template<>
	inline webrtc::FakeConstraints marshal_as(WebRtcNet::MediaConstraints ^ const & from)
	{
		if (from == nullptr) throw gcnew System::ArgumentNullException("from");
		webrtc::FakeConstraints to;

		for each (auto constraint in from->Mandatory)
		{
			to.AddMandatory(marshal_as<std::string>(constraint->Key), marshal_as<std::string>(constraint->ValueString));
		}

		for each (auto constraint in from->Optional)
		{
			to.AddOptional(marshal_as<std::string>(constraint->Key), marshal_as<std::string>(constraint->ValueString));
		}

		return to;
	};
}}