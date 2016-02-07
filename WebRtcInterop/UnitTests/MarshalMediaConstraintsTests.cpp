#include "Stdafx.h"

#include <string>
#include "..\Marshaling\MarshalMediaConstraints.h"
#include "webrtc\base\scoped_ptr.h"

#using <System.Core.dll>

using namespace msclr::interop;

using namespace System;
using namespace Linq;
using namespace Runtime::InteropServices;
using namespace NUnit::Framework;

using namespace WebRtcNet;

namespace WebRtcInterop { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalMediaConstraintsTests
	{
	public:
		[Test]
		void marshal_as_managed_MediaConstraints_to_native_FakeMediaConstraints()
		{
			auto managedConstraints = gcnew MediaConstraints(
				gcnew array<MediaConstraints::Constraint ^>  {
				gcnew MediaConstraints::Constraint<String ^>("M_Key0", "M_Value0"),
					gcnew MediaConstraints::Constraint<String ^>("M_Key1", "M_Value1")
			},
				gcnew array<MediaConstraints::Constraint ^>  {
					gcnew MediaConstraints::Constraint<String ^>("O_Key0", "O_Value0"),
						gcnew MediaConstraints::Constraint<String ^>("O_Key1", "O_Value1")
				});

			webrtc::FakeConstraints nativeConstraints = marshal_as<webrtc::FakeConstraints>(managedConstraints);

			Assert::That(nativeConstraints.GetMandatory().size() == 2);
			Assert::That(nativeConstraints.GetMandatory()[0].key == "M_Key0");
			Assert::That(nativeConstraints.GetMandatory()[0].value == "M_Value0");
			Assert::That(nativeConstraints.GetMandatory()[1].key == "M_Key1");
			Assert::That(nativeConstraints.GetMandatory()[1].value == "M_Value1");
			Assert::That(nativeConstraints.GetOptional()[0].key == "O_Key0");
			Assert::That(nativeConstraints.GetOptional()[0].value == "O_Value0");
			Assert::That(nativeConstraints.GetOptional()[1].key == "O_Key1");
			Assert::That(nativeConstraints.GetOptional()[1].value == "O_Value1");
		}

		[Test]
		void marshal_as_null_managed_MediaConstraints_to_native_FakeMediaConstraints()
		{
			auto action = gcnew TestDelegate(this, &MarshalMediaConstraintsTests::MarshalNullManagedIceServer);

			Assert::Throws<ArgumentNullException ^>(action);
		}

	private:
		void MarshalNullManagedIceServer()
		{
			WebRtcNet::MediaConstraints ^ managedConstraints = nullptr;
			auto nativeCandidate = marshal_as<webrtc::FakeConstraints>(managedConstraints);
			Assert::Fail("Should not have made it here");
		}
	};
}}
