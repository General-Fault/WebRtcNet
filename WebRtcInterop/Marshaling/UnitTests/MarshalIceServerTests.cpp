#include "Stdafx.h"

#include <string>
#include "..\Marshaling\MarshalIceServer.h"

#using <System.Core.dll>

using namespace msclr::interop;

using namespace System;
using namespace System::Linq;
using namespace System::Runtime::InteropServices;
using namespace NUnit::Framework;


namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalIceServerTests
	{
	public:

		[Test]
		void marshal_as_managed_RtcIceServer_to_native_IceServer_test()
		{
			auto managedServer = gcnew WebRtcNet::RtcIceServer("A Url", "Username", "Password");

			auto nativeServer = marshal_as<webrtc::PeerConnectionInterface::IceServer>(managedServer);

			Assert::That(nativeServer.urls.size() == 1);
			Assert::That(nativeServer.urls.at(0).compare(std::string("A Url")) == 0);
			Assert::That(nativeServer.username.compare("Username") == 0);
			Assert::That(nativeServer.password.compare("Password") == 0);
		}

		[Test]
		void marshal_as_native_IceServer_to_managed_RtcIceServer_test()
		{
			webrtc::PeerConnectionInterface::IceServer nativeServer;
			nativeServer.urls.push_back(std::string("A Url"));
			nativeServer.username = "Username";
			nativeServer.password = "Password";
				
			auto managedServer = marshal_as<WebRtcNet::RtcIceServer ^>(nativeServer);
				
			Assert::NotNull(managedServer);
			Assert::AreEqual(1, Enumerable::Count<String ^>(managedServer->Urls));
			Assert::AreEqual("A Url", Enumerable::First<String ^>(managedServer->Urls));
			Assert::AreEqual("Username", managedServer->UserName);
			Assert::AreEqual("Password", managedServer->Credential);
		}

	};
}}}