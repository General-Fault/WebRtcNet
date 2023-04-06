#include "pch.h"

#include "RtcIceTransport.h"

#include "api/ice_transport_interface.h"
#include "p2p/base/ice_transport_internal.h" // see bugs.webrtc.org/9308
#include "Marshaling/MarshalIceTransport.h"

using namespace WebRtcNet;
using namespace WebRtcInterop;

RtcIceTransport::RtcIceTransport(webrtc::IceTransportInterface* ice_transport_interface)
	: rp_ice_transport_interface_(ice_transport_interface)
{
}

RtcIceTransport::~RtcIceTransport()
{
	this->!RtcIceTransport();
}

RtcIceTransport::!RtcIceTransport()
{
	rp_ice_transport_interface_ = nullptr;
}

webrtc::IceTransportInterface* RtcIceTransport::GetNativeIceTransportInterface(bool throwOnDisposed)
{
	const auto result = rp_ice_transport_interface_.Get();
	if (result == nullptr)
	{
		if (throwOnDisposed) throw gcnew ObjectDisposedException(NAMEOF(RtcIceTransport));
		return nullptr;
	}

	return result;
}

RtcIceRole RtcIceTransport::Role::get()
{
	return marshal_as<RtcIceRole>(GetNativeIceTransportInterface(true)->internal()->GetIceRole());
}

RtcIceComponent RtcIceTransport::Component::get()
{
	return marshal_as<RtcIceComponent>(GetNativeIceTransportInterface(true)->internal()->component());
}

RtcIceTransportState RtcIceTransport::State::get()
{
	return marshal_as<RtcIceTransportState>(GetNativeIceTransportInterface(true)->internal()->GetIceTransportState());
}

RtcIceGatheringState RtcIceTransport::GatheringState::get()
{
	throw gcnew NotImplementedException();
}

IEnumerable<IRtcIceCandidate^>^ RtcIceTransport::GetLocalCandidates()
{
	throw gcnew NotImplementedException();
}

IEnumerable<IRtcIceCandidate^>^ RtcIceTransport::GetRemoteCandidates()
{
	throw gcnew NotImplementedException();
}

RtcIceCandidatePair^ RtcIceTransport::GetSelectedCandidatePair()
{
	throw gcnew NotImplementedException();
}

RtcIceParameters^ RtcIceTransport::GetLocalParameters()
{
	throw gcnew NotImplementedException();
}

RtcIceParameters^ RtcIceTransport::GetRemoteParameters()
{
	throw gcnew NotImplementedException();
}


