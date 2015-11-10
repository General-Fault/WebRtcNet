#include "stdafx.h"
#include "RtcConfiguration.h"

WEBRTCNET_START

RtcConfiguration::RtcConfiguration(String ^ definition)
{
	throw gcnew System::NotImplementedException();
}

RtcConfiguration::RtcConfiguration(IList<RtcIceServer ^> ^ servers, String ^ username, String ^ credentials)
{

}

WEBRTCNET_END