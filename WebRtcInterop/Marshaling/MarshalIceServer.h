#pragma once

#include "talk\app\webrtc\peerconnectioninterface.h"
#include "MarshalCollections.h"

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

namespace msclr {
	namespace interop
	{

		template<>
		inline webrtc::PeerConnectionInterface::IceServer marshal_as(WebRtcNet::RtcIceServer ^ const & from)
		{
			webrtc::PeerConnectionInterface::IceServer to;
			to.urls = marshal_enumerable_as<std::string, System::String ^>(safe_cast<System::Collections::Generic::IEnumerable<System::String ^> ^>(from->Urls));
			to.username = System::String::IsNullOrEmpty(from->UserName) ? "" : marshal_as<std::string>(from->UserName);
			to.password = System::String::IsNullOrEmpty(from->Credential) ? "" : marshal_as<std::string>(from->Credential);
			return to;
		};

		template<>
		inline WebRtcNet::RtcIceServer ^ marshal_as(const webrtc::PeerConnectionInterface::IceServer & from)
		{
			auto urls = gcnew System::Collections::Generic::List<System::String ^>(marshal_vector_as<System::String ^, std::string>(from.urls));
			auto username = marshal_as<System::String ^>(from.username);
			auto password = marshal_as<System::String ^>(from.password);

			auto to = gcnew WebRtcNet::RtcIceServer(urls, username, password);
			return to;
		};
	}
}