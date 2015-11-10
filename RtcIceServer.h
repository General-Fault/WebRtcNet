#pragma once

WEBRTCNET_START

public ref class RtcIceServer
{
public:
	RtcIceServer(String ^ definition);

	/// STUN or TURN URI(s) as defined in [RFC7064] and [RFC7065] or other URI types.
	property IEnumerable<String ^> ^ Urls { IEnumerable<String ^> ^ get(); void set(IEnumerable<String ^> ^ value); }
	
	/// If this RTCIceServer object represents a TURN server, then this attribute specifies the username to use with that TURN server.
	property String ^ UserName { String ^ get(); void set(String ^ value); }

	/// If this RTCIceServer object represents a TURN server, then this attribute specifies the credential to use with that TURN server.
	property String ^ Credential { String ^ get(); void set(String ^ value); }
};

WEBRTCNET_END