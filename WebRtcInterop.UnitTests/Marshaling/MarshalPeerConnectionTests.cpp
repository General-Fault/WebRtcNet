#include "Stdafx.h"

#include "../MarshalPeerConnection.h"
#include "api/jsep_session_description.h"

using namespace msclr::interop;

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace NUnit::Framework;
using namespace webrtc;
using namespace WebRtcNet;

namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalPeerConnectionTests
	{
	public:

#pragma region(webrtc::PeerConnectionInterface::RTCOfferAnswerOptions to WebRtcNet::RtcOfferOptions)
		
		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::IsNotNull(managedOptions);
		}

		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_receive_audio_undefined_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.offer_to_receive_audio = PeerConnectionInterface::RTCOfferAnswerOptions::kUndefined;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(RtcOfferOptions::Undefined, managedOptions->OfferToReceiveAudio);
		}

		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_recieve_audio_true_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.offer_to_receive_audio = PeerConnectionInterface::RTCOfferAnswerOptions::kOfferToReceiveMediaTrue;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(RtcOfferOptions::OfferToReceiveTrue, managedOptions->OfferToReceiveAudio);
		}

		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_recieve_audio_max_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.offer_to_receive_audio = PeerConnectionInterface::RTCOfferAnswerOptions::kMaxOfferToReceiveMedia;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(RtcOfferOptions::MaxOfferToReceiveMedia, managedOptions->OfferToReceiveAudio);
		}

		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_receive_video_undefined_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.offer_to_receive_video = PeerConnectionInterface::RTCOfferAnswerOptions::kUndefined;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(RtcOfferOptions::Undefined, managedOptions->OfferToReceiveVideo);
		}

		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_recieve_video_true_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.offer_to_receive_video = PeerConnectionInterface::RTCOfferAnswerOptions::kOfferToReceiveMediaTrue;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(RtcOfferOptions::OfferToReceiveTrue, managedOptions->OfferToReceiveVideo);
		}

		[Test]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferOptions_recieve_video_max_test()
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.offer_to_receive_video = PeerConnectionInterface::RTCOfferAnswerOptions::kMaxOfferToReceiveMedia;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(RtcOfferOptions::MaxOfferToReceiveMedia, managedOptions->OfferToReceiveVideo);
		}

		[Test, TestCase(true), TestCase(false)]
		void marshal_as_native_RTCOfferAnswerOptions_to_managed_RtcOfferAnswerOptions_voice_detection_test(bool voiceDetection)
		{
			PeerConnectionInterface::RTCOfferAnswerOptions nativeOptions;
			nativeOptions.voice_activity_detection = voiceDetection;

			auto managedOptions = marshal_as<RtcOfferOptions ^>(nativeOptions);

			Assert::AreEqual(voiceDetection, managedOptions->VoiceActivityDetection);
		}

#pragma endregion

#pragma region(Native enums to Managed Enums)

#pragma region(SignalingState)
	public:
		[Test, 
			TestCase((Object ^)((int)PeerConnectionInterface::SignalingState::kStable), RtcSignalingState::Stable),
			TestCase((Object ^)((int)PeerConnectionInterface::SignalingState::kHaveLocalOffer), RtcSignalingState::HaveLocalOffer),
			TestCase((Object ^)((int)PeerConnectionInterface::SignalingState::kHaveLocalPrAnswer), RtcSignalingState::HaveLocalPrAnswer),
			TestCase((Object ^)((int)PeerConnectionInterface::SignalingState::kHaveRemoteOffer), RtcSignalingState::HaveRemoteOffer),
			TestCase((Object ^)((int)PeerConnectionInterface::SignalingState::kHaveRemotePrAnswer), RtcSignalingState::HaveRemotePrAnswer),
			TestCase((Object ^)((int)PeerConnectionInterface::SignalingState::kClosed), RtcSignalingState::Closed)]
		void marshal_as_native_SignalingState_to_managed_RtcSignalingState_test(PeerConnectionInterface::SignalingState from, RtcSignalingState expected)
		{
			auto to = marshal_as<RtcSignalingState>(from);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_SignalingState_to_managed_RtcSignalingState_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalInvalidSignalingState);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidSignalingState()
		{
			auto to = marshal_as<RtcSignalingState>((PeerConnectionInterface::SignalingState)(PeerConnectionInterface::SignalingState::kClosed + 1));
			Assert::Fail("Should not have made it here");
		}

#pragma endregion

#pragma region(IceConnectionState)
	public:
		[Test,
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionNew), RtcIceConnectionState::New),
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionChecking), RtcIceConnectionState::Checking),
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionConnected), RtcIceConnectionState::Connected),
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionCompleted), RtcIceConnectionState::Completed),
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionFailed), RtcIceConnectionState::Failed),
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionDisconnected), RtcIceConnectionState::Disconnected),
			TestCase((Object ^)((int)PeerConnectionInterface::IceConnectionState::kIceConnectionClosed), RtcIceConnectionState::Closed)]
		void marshal_as_native_IceConnectionState_to_managed_RtcIceConnectionState_test(PeerConnectionInterface::IceConnectionState from, RtcIceConnectionState expected)
		{
			auto to = marshal_as<RtcIceConnectionState>(from);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_IceConnectionState_to_managed_IceConnectionState_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalInvalidIceConnectionState);

			Assert::Throws<InvalidCastException ^>(action);
		}

		[Test]
		void marshal_as_native_IceConnectionState_kIceConnectionMax_to_managed_IceConnectionState_unsupported_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalMaxIceConnectionState);

			Assert::Throws<NotSupportedException ^>(action);
		}

	private:
		static void MarshalInvalidIceConnectionState()
		{
			auto to = marshal_as<RtcIceConnectionState>((PeerConnectionInterface::IceConnectionState)(PeerConnectionInterface::IceConnectionState::kIceConnectionMax + 1));
			Assert::Fail("Should not have made it here");
		}

		static void MarshalMaxIceConnectionState()
		{
			auto to = marshal_as<RtcIceConnectionState>(PeerConnectionInterface::IceConnectionState::kIceConnectionMax);
			Assert::Fail("Should not have made it here");
		}
#pragma endregion

#pragma region(IceGatheringState)
	public:
		[Test,
			TestCase((Object ^)((int)PeerConnectionInterface::IceGatheringState::kIceGatheringNew), RtcGatheringState::New),
			TestCase((Object ^)((int)PeerConnectionInterface::IceGatheringState::kIceGatheringGathering), RtcGatheringState::Gathering),
			TestCase((Object ^)((int)PeerConnectionInterface::IceGatheringState::kIceGatheringComplete), RtcGatheringState::Complete)]
		void marshal_as_native_IceGatheringState_to_managed_RtcIceGatheringState_test(PeerConnectionInterface::IceGatheringState from, RtcGatheringState expected)
		{
			auto to = marshal_as<RtcGatheringState>(from);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_IceGatheringState_to_managed_IceGatheringState_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalInvalidIceGatheringState);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidIceGatheringState()
		{
			auto to = marshal_as<RtcGatheringState>((PeerConnectionInterface::IceGatheringState)(PeerConnectionInterface::IceGatheringState::kIceGatheringComplete + 1));
			Assert::Fail("Should not have made it here");
		}
#pragma endregion

#pragma region(IceGatheringState)
	public:
		[Test,
			TestCase((Object ^)((int)PeerConnectionInterface::IceTransportsType::kNone), RtcIceTransportPolicy::None),
			TestCase((Object ^)((int)PeerConnectionInterface::IceTransportsType::kRelay), RtcIceTransportPolicy::Relay),
			TestCase((Object ^)((int)PeerConnectionInterface::IceTransportsType::kAll), RtcIceTransportPolicy::All)]
		void marshal_as_native_IceTransportsType_to_managed_RtcIceTransportsPolicy_test(PeerConnectionInterface::IceTransportsType from, RtcIceTransportPolicy expected)
		{
			auto to = marshal_as<RtcIceTransportPolicy>(from);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_IceTransportsType_to_managed_IceTransportsPolicy_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalInvalidIceTransportsType);

			Assert::Throws<InvalidCastException ^>(action);
		}

		[Test]
		void marshal_as_native_IceTransportsType_kNoHost_to_managed_RtcIceTransportPolicy_unsupported_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalNoHostIceTransportsType);

			Assert::Throws<NotSupportedException ^>(action);
		}

	private:
		static void MarshalInvalidIceTransportsType()
		{
			auto to = marshal_as<RtcIceTransportPolicy>((PeerConnectionInterface::IceTransportsType)(PeerConnectionInterface::IceTransportsType::kAll + 1));
			Assert::Fail("Should not have made it here");
		}

		static void MarshalNoHostIceTransportsType()
		{
			auto to = marshal_as<RtcIceTransportPolicy>(PeerConnectionInterface::IceTransportsType::kNoHost);
			Assert::Fail("Should not have made it here");
		}
#pragma endregion

#pragma region(BundlePolicy)
	public:
		[Test,
			TestCase((Object ^)((int)PeerConnectionInterface::BundlePolicy::kBundlePolicyBalanced), RtcBundlePolicy::Balanced),
			TestCase((Object ^)((int)PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxBundle), RtcBundlePolicy::MaxBundle),
			TestCase((Object ^)((int)PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat), RtcBundlePolicy::MaxCompat)]
		void marshal_as_native_BundlePolicy_to_managed_RtcBundlePolicy_test(PeerConnectionInterface::BundlePolicy from, RtcBundlePolicy expected)
		{
			auto to = marshal_as<RtcBundlePolicy>(from);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_BundlePolicy_to_managed_RtcBundlePolicy_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalInvalidBundlePolicy);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidBundlePolicy()
		{
			auto to = marshal_as<RtcBundlePolicy>((PeerConnectionInterface::BundlePolicy)(PeerConnectionInterface::BundlePolicy::kBundlePolicyMaxCompat + 1));
			Assert::Fail("Should not have made it here");
		}

#pragma endregion

#pragma region(SdpType)
	public:
		[Test,
			//really hard to use the defined values in SessionDescriptionInterface. So cheating by using string literals.
			TestCase("answer", RtcSdpType::Answer), 
			TestCase("pranswer", RtcSdpType::PrAnswer),
			TestCase("offer", RtcSdpType::Offer)]
		void marshal_as_native_SdpType_to_managed_RtcSdpType_test(String ^ from, RtcSdpType expected)
		{
			auto nativeFrom = marshal_as<std::string>(from);

			auto to = marshal_as<RtcSdpType>(nativeFrom);

			Assert::AreEqual(expected, to);
		}

		[Test]
		void marshal_as_native_SdpType_to_managed_RtcSdpType_invalid_test()
		{
			auto action = gcnew TestDelegate(&MarshalPeerConnectionTests::MarshalInvalidSdpType);

			Assert::Throws<InvalidCastException ^>(action);
		}

	private:
		static void MarshalInvalidSdpType()
		{
			auto to = marshal_as<RtcSdpType>(std::string("Foo"));
			Assert::Fail("Should not have made it here");
		}

#pragma endregion

#pragma endregion

#pragma region(webrtc::SessionDescriptionInterface to WebRtcNet::SessionDescription)
		public:
		[Test]
		void marshal_as_native_SessionDescriptionInterface_to_managed_SessionDescription()
		{
			JsepSessionDescription nativeDescription(JsepSessionDescription::kOffer);
			auto sdp =
				"v=0\r\n" +
				"o=- 2890844526 2890844526 IN IP4 127.0.0.1\r\n" +
				"s=-\r\n" +
				"t=0 0\r\n" +
				"a=msid-semantic: WMS\r\n" +
				"m=audio 9 RTP 0\r\n" +
				"c=IN IP4 0.0.0.0\r\n" +
				"a=mid:audio\r\n";


			webrtc::SdpParseError error;
			auto validSdp = nativeDescription.Initialize(marshal_as<std::string>(sdp), &error);

			Console::WriteLine(String::Format("Valid={0}, ErroLine={1}, ErrorDescription={2}", validSdp, marshal_as<String^>(error.line), marshal_as<String^>(error.description)));

			auto managedDescription = marshal_as<RtcSessionDescription>(dynamic_cast<SessionDescriptionInterface*>(&nativeDescription));

			Assert::That(managedDescription.Type == RtcSdpType::Offer);
			Assert::AreEqual(sdp, managedDescription.Sdp);
		}
		
#pragma endregion
	};
}}}
