#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

#include "RtcIceServer.h"

WEBRTCNET_START

RtcIceServer::RtcIceServer(String ^ definition)
{
}


IEnumerable<String ^> ^ RtcIceServer::Urls::get()
{
	throw gcnew NotImplementedException();
}

void RtcIceServer::Urls::set(IEnumerable<String ^> ^ value)
{
	throw gcnew NotImplementedException();
}

String ^ RtcIceServer::UserName::get()
{
	throw gcnew NotImplementedException();
}

void RtcIceServer::UserName::set(String ^ value)
{
	throw gcnew NotImplementedException();
}

String ^ RtcIceServer::Credential::get()
{
	throw gcnew NotImplementedException();
}

void RtcIceServer::Credential::set(String ^ value)
{
	throw gcnew NotImplementedException();
}


WEBRTCNET_END