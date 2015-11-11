#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

#include "RtcConfiguration.h"


WEBRTCNET_START

RtcConfiguration::RtcConfiguration(String ^ definition)
{
	throw gcnew NotImplementedException();
}

RtcConfiguration::RtcConfiguration(IList<RtcIceServer ^> ^ servers, String ^ username, String ^ credentials)
{

}

WEBRTCNET_END