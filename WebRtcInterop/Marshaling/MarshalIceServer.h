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
			to.urls = marshal_enumerable_as<std::string, String^>(safe_cast<IEnumerable<String^>^>(from->Urls));
			to.username = String::IsNullOrEmpty(from->UserName) ? "" : marshal_as<std::string>(%*(from->UserName));
			to.password = String::IsNullOrEmpty(from->Credential) ? "" : marshal_as<std::string>(%*(from->Credential));
			return to;
		};

		template<>
		inline WebRtcNet::RtcIceServer ^ marshal_as(const webrtc::PeerConnectionInterface::IceServer & from)
		{
			auto urls = gcnew List<String^>(marshal_vector_as<String^, std::string>(from.urls));
			auto username = marshal_as<String^>(from.username);
			auto password = marshal_as<String^>(from.password);

			auto to = gcnew WebRtcNet::RtcIceServer(urls, username, password);
			return to;
		};

	/*	template<>
		inline WebRtcInterop::IceServers marshal_as(webrtc::PeerConnectionInterface::IceServers const& from)
		{
			auto to = gcnew List<WebRtcInterop::PeerConnection::IceServer^>();
			for (auto server : from)
			{
				to->Add(marshal_as<WebRtcInterop::PeerConnection::IceServer^>(server));
			}
			return (static_cast<WebRtcInterop::PeerConnection::IceServers>(to));
		};

		inline webrtc::PeerConnectionInterface::IceServers marshal_as(WebRtcInterop::PeerConnection::IceServers const & from)
		{
			webrtc::PeerConnectionInterface::IceServers to;
			for each (auto server in from)
			{
				to.push_back(marshal_as<webrtc::PeerConnectionInterface::IceServer>(server));
			}
			return to;
		}*/
	}
}