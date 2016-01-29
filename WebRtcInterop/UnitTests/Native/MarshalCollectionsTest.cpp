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
			std::vector<std::string> nativeVector;
			nativeVector.push_back("TestString");

			auto result = marshal_vector_as<String^>(nativeVector);
			auto resultArray = System::Linq::Enumerable::ToArray(result);

			Assert::AreEqual(1, resultArray->Length);
			Assert::AreEqual("TestString", resultArray[0]);
		}

		[TestMethod]
		void marshal_vector_as_empty_source_test()
		{
			std::vector<std::string> nativeVector;

			auto result = marshal_vector_as<String^>(nativeVector);
			auto resultArray = System::Linq::Enumerable::ToArray(result);

			Assert::AreEqual(0, resultArray->Length);
		}
	};
}

