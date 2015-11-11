#pragma once

WEBRTCNET_START

public ref class RtcIdentityAssertion
{
public:
	property String ^ Idp { String ^ get(); void set(String ^ value); }
	property String ^ Name { String ^ get(); void set(String ^ value); }
};

WEBRTCNET_END