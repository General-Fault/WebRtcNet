#include "Stdafx.h"

#include "..\Marshaling\MarshalCollections.h"
#using <System.Core.dll>

using namespace msclr::interop;

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace NUnit::Framework;

namespace WebRtcInterop { namespace Marshaling { namespace UnitTests
{
	[TestFixture]
	public ref class MarshalCollectionsTests
	{
	public:

		[Test]
		void marshal_vector_as_test()
		{
			std::vector<std::string> nativeVector = { "TestString" };

			auto result = marshal_vector_as<String ^>(nativeVector);
			auto resultArray = Linq::Enumerable::ToArray(result);
			auto tst = marshal_as<String^>(std::string("test"));

			Assert::AreEqual(1, resultArray->Length);
			Assert::AreEqual("TestString", resultArray[0]);
		}

		[Test]
		void marshal_vector_as_empty_source_test()
		{
			std::vector<std::string> nativeVector;

			auto result = marshal_vector_as<String ^>(nativeVector);
			auto resultArray = Linq::Enumerable::ToArray(result);

			Assert::AreEqual((Object ^)0, resultArray->Length);
		}

		[Test]
		void marshal_map_as_test()
		{
			std::map<std::string, std::string> nativeMap = { {"1","a"}, {"2","b"}, {"3", "c"} };
			
			auto result = marshal_map_as<String ^, String ^>(nativeMap);

			Assert::AreEqual(3, result->Count);
			Assert::AreEqual("a", result["1"]);
			Assert::AreEqual("b", result["2"]);
			Assert::AreEqual("c", result["3"]);
		}

		[Test]
		void marshal_map_as_empty_test()
		{
			std::map<std::string, std::string> nativeMap;

			auto result = marshal_map_as<String ^, String ^>(nativeMap);

			Assert::AreEqual((Object ^)0, result->Count);
		}

		[Test]
		void marshal_enumerable_as_test()
		{
			auto managedEnumerable = gcnew array<String ^>{ "a", "b", "c" };

			auto result = marshal_enumerable_as<std::string, String ^>(managedEnumerable);

			Assert::AreEqual((Object ^)3, result.size());
			Assert::AreEqual((Object ^)0, result[0].compare("a"));
			Assert::AreEqual((Object ^)0, result[1].compare("b"));
			Assert::AreEqual((Object ^)0, result[2].compare("c"));
		}
	};
}}}
