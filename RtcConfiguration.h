#pragma once

#include "RtcIceServer.h"
#include "IRtcCertificate.h"

WEBRTCNET_START

public enum RtcIceTransportPolicy
{
	/// The ICE engine must not send or receive any packets at this point.
	None,

	/// The ICE engine must only use media relay candidates such as candidates 
	/// passing through a TURN server. This can be used to reduce leakage of 
	/// IP addresses in certain use cases.
	Relay,

	/// The ICE engine may use any type of candidates when this value is specified.
	All,
};

public enum RtcBundlePolicy
{
	/// Gather ICE candidates for each media type in use (audio, video, and data). 
	/// If the remote endpoint is not BUNDLE-aware, negotiate only one audio and video 
	/// track on separate transports.
	Balanced,

	/// Gather ICE candidates for each track.If the remote endpoint is 
	/// not BUNDLE - aware, negotiate all media tracks on separate transports.
	MaxCompat,

	/// Gather ICE candidates for only one track. If the remote endpoint is 
	/// not BUNDLE-aware, negotiate only one media track.
	MaxBundle
};

public ref class RtcConfiguration
{
public:
	RtcConfiguration(String ^ definition);
	RtcConfiguration(IList<RtcIceServer ^> ^ servers, String ^ username, String ^ credentials);

	/// A list containing URIs of servers available to be used by ICE, such as STUN and TURN server.
	property IList<RtcIceServer^>^ IceServers
	{ 
		IList<RtcIceServer ^> ^ get() { throw gcnew NotImplementedException(); } 
		void set(IList<RtcIceServer ^> ^ value) { throw gcnew NotImplementedException(); }
	}
	
	/// Indicates which candidates the ICE engine is allowed to use.
	property RtcIceTransportPolicy IceTransportPolicy 
	{ 
		RtcIceTransportPolicy get() { throw gcnew NotImplementedException(); } 
		void set(RtcIceTransportPolicy value) { throw gcnew NotImplementedException(); }
	}

	/// Indicates which BundlePolicy to use. Defaults to "Balanced"
	property RtcBundlePolicy BundlePolicy 
	{ 
		RtcBundlePolicy get() { throw gcnew NotImplementedException(); } 
		void set(RtcBundlePolicy value) { throw gcnew NotImplementedException(); }
	}

	/// Sets the target peer identity for the RTCPeerConnection. The RTCPeerConnection will establish 
	/// a connection to a remote peer unless it can be successfully authenticated with the provided name.
	property String ^ PeerIdentity 
	{ 
		String ^ get() { throw gcnew NotImplementedException(); } 
		void set(String ^ value) { throw gcnew NotImplementedException(); }
	}

	/// A set of certificates that the RTCPeerConnection uses to authenticate.
	property IList<IRtcCertificate ^> ^ Certificates 
	{
		IList<IRtcCertificate ^> ^ get() { throw gcnew NotImplementedException(); }
		void set(IList<IRtcCertificate ^> ^value) { throw gcnew NotImplementedException(); }
	}

	/// Size of the prefetched ICE pool as defined in [RTCWEB-JSEP] Section 3.4.4 and 4.1.1.
	property UInt16 IceCandidatePoolSize
	{
		UInt16 get() { throw gcnew NotImplementedException(); }
		void set(UInt16 value) { throw gcnew NotImplementedException(); }
	}
};

WEBRTCNET_END