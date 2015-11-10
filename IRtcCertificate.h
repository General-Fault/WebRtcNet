#pragma once

WEBRTCNET_START

public interface class IRtcCertificate
{
	/// Expires indicates the date and time after which the certificate will be considered invalid . 
	/// After this time, attempts to construct an RTCPeerConnection using this certificate fail.
	property DateTime ^ Expires { DateTime ^ get(); }
};

WEBRTCNET_END