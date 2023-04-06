#pragma once

#include "ManagedScopedRefPtr.h"

namespace webrtc
{
	class IceTransportInterface;
}

namespace WebRtcInterop
{
	using namespace System;
	using namespace Collections::Generic;
	using namespace WebRtcNet;

	public ref class RtcIceTransport : public IRtcIceTransport
	{
	public:
		~RtcIceTransport();

		virtual property RtcIceRole Role{ RtcIceRole get(); }
		virtual property RtcIceComponent Component{ RtcIceComponent get(); }
		virtual property RtcIceTransportState State{ RtcIceTransportState get(); }
		virtual property RtcIceGatheringState GatheringState{ RtcIceGatheringState get(); }
		virtual IEnumerable<IRtcIceCandidate^>^ GetLocalCandidates();
		virtual IEnumerable<IRtcIceCandidate^>^ GetRemoteCandidates();
		virtual RtcIceCandidatePair^ GetSelectedCandidatePair();
		virtual RtcIceParameters^ GetLocalParameters();
		virtual RtcIceParameters^ GetRemoteParameters();

		virtual event EventHandler^ OnStateChange;
		virtual event EventHandler^ OnGatheringStateChange;
		virtual event EventHandler^ OnSelectedCandidatePairChange;

	internal:
		RtcIceTransport(webrtc::IceTransportInterface* ice_transport_interface);
		!RtcIceTransport();
		webrtc::IceTransportInterface* GetNativeIceTransportInterface(bool throwOnDisposed);

		void FireOnStateChange() { OnStateChange(this, EventArgs::Empty); }
		void FireOnGatheringStateChange() { OnGatheringStateChange(this, EventArgs::Empty); }
		void FireOnSelectedCandidatePairChange() { OnSelectedCandidatePairChange(this, EventArgs::Empty); }

	private:
		ManagedScopedRefPtr<webrtc::IceTransportInterface> rp_ice_transport_interface_;
	};
}
