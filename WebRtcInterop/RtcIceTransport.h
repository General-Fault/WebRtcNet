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

		virtual property RtcIceRole Role{ RtcIceRole get() override; }
		virtual property RtcIceComponent Component{ RtcIceComponent get() override; }
		virtual property RtcIceTransportState State{ RtcIceTransportState get() override; }
		virtual property RtcIceGatheringState GatheringState{ RtcIceGatheringState get() override; }
		virtual IEnumerable<IRtcIceCandidate^>^ GetLocalCandidates() override;
		virtual IEnumerable<IRtcIceCandidate^>^ GetRemoteCandidates() override;
		virtual RtcIceCandidatePair^ GetSelectedCandidatePair() override;
		virtual RtcIceParameters^ GetLocalParameters() override;
		virtual RtcIceParameters^ GetRemoteParameters() override;

		virtual event EventHandler^ OnStateChange
		{
			void add(EventHandler^ value) override { on_state_change_ += value; }
			void remove(EventHandler^ value) override { on_state_change_ -= value; }
		}
		virtual event EventHandler^ OnGatheringStateChange
		{
			void add(EventHandler^ value) override { on_gathering_state_change_ += value; }
			void remove(EventHandler^ value) override { on_gathering_state_change_ -= value; }
		}
		virtual event EventHandler^ OnSelectedCandidatePairChange
		{
			void add(EventHandler^ value) override { on_selected_candidate_pair_change_ += value; }
			void remove(EventHandler^ value) override { on_selected_candidate_pair_change_ -= value; }
		}

	internal:
		RtcIceTransport(webrtc::IceTransportInterface* ice_transport_interface);
		!RtcIceTransport();
		webrtc::IceTransportInterface* GetNativeIceTransportInterface(bool throwOnDisposed);
		virtual System::IntPtr GetNativeIceTransportHandle(bool throwOnDisposed) override;

		void FireOnStateChange() { if (on_state_change_ != nullptr) on_state_change_(this, EventArgs::Empty); }
		void FireOnGatheringStateChange() { if (on_gathering_state_change_ != nullptr) on_gathering_state_change_(this, EventArgs::Empty); }
		void FireOnSelectedCandidatePairChange() { if (on_selected_candidate_pair_change_ != nullptr) on_selected_candidate_pair_change_(this, EventArgs::Empty); }

	private:
		ManagedScopedRefPtr<webrtc::IceTransportInterface> rp_ice_transport_interface_;
		EventHandler^ on_state_change_;
		EventHandler^ on_gathering_state_change_;
		EventHandler^ on_selected_candidate_pair_change_;
	};
}
