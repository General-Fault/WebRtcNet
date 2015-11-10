#pragma once

WEBRTCNET_START

public ref class RtpIdentityAssertion
{
public:
	RtpIdentityAssertion() 	{}

	/// A domain name representing the identity provider.
	property String ^ IdP { String^ get(); }

	/// RFC5322-conformant [RFC5322] representation of the verified peer identity. 
	/// This identity will have been verified via the procedures described in [RTCWEB-SECURITY-ARCH].
	property String ^ Name { String ^ get(); }

};

WEBRTCNET_END