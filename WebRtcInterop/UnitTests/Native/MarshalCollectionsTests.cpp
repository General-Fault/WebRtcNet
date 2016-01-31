#include "Stdafx.h"

#include "..\..\Marshaling\MarshalCollections.h"
#using <System.Core.dll>

using namespace msclr::interop;

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace Microsoft::VisualStudio::TestTools::UnitTesting;

namespace
{
	[TestClass]
	public ref class MarshalCollectionsTests
	{
	public:

		[TestMethod]
		void marshal_vector_as_test()
		{
			std::vector<std::string> nativeVector = { "TestString" };

			auto result = marshal_vector_as<String ^>(nativeVector);
			auto resultArray = System::Linq::Enumerable::ToArray(result);
			auto tst = marshal_as<System::String^>(std::string("test"));

			Assert::AreEqual(1, resultArray->Length);
			Assert::AreEqual("TestString", resultArray[0]);
		}

		[TestMethod]
		void marshal_vector_as_empty_source_test()
		{
			std::vector<std::string> nativeVector;

			auto result = marshal_vector_as<String ^>(nativeVector);
			auto resultArray = System::Linq::Enumerable::ToArray(result);

			Assert::AreEqual(0, resultArray->Length);
		}

		[TestMethod]
		void marshal_map_as_test()
		{
			std::map<std::string, std::string> nativeMap = { {"1","a"}, {"2","b"}, {"3", "c"} };
			
			auto result = marshal_map_as<System::String ^, System::String ^>(nativeMap);

			Assert::AreEqual(3, result->Count);
			Assert::AreEqual("a", result["1"]);
			Assert::AreEqual("b", result["2"]);
			Assert::AreEqual("c", result["3"]);
		}

		[TestMethod]
		void marshal_map_as_empty_test()
		{
			std::map<std::string, std::string> nativeMap;

			auto result = marshal_map_as<System::String ^, System::String ^>(nativeMap);

			Assert::AreEqual(0, result->Count);
		}

		[TestMethod]
		void marshal_enumerable_as_test()
		{
			System::Collections::Generic::IEnumerable<System::String ^> ^ managedEnumerable = gcnew List<System::String ^>(gcnew array<System::String ^>{ "a", "b", "c" });

			auto result = marshal_enumerable_as<std::string>(managedEnumerable);

			Assert::AreEqual(3u, result.size());
			Assert::AreEqual(0, result[0].compare("a"));
			Assert::AreEqual(0, result[1].compare("b"));
			Assert::AreEqual(0, result[2].compare("c"));
		}
	};
}

