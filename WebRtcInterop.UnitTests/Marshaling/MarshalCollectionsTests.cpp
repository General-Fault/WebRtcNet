#include "pch.h"

#include <vector>
#include <map>
#include <array>

#include "Marshaling/MarshalCollections.h"
#include <gtest/gtest.h>

#include "TestUtils.h"

using namespace msclr::interop;

using namespace System;
using namespace Linq;
using namespace Text;
using namespace Collections::Generic;

class marshal_as_collections_test
{
};

TEST(marshal_as_collections_test, marshal_vector_of_strings_as_list)
{
	const std::vector<std::string> nativeVector { "TestString" };

	auto result = marshal_as<String^>(nativeVector);

	ASSERT_NE(result, nullptr) << "The result == nullptr";
	ASSERT_MANAGED_TYPE_EQ(*result, (List<String^>::typeid));
	ASSERT_EQ(result->Count, 1) << "The managed List should contain a single item, but has " << result->Count << " items";
	EXPECT_TRUE(result[0]->Equals("TestString")) << "The item in the should be \"TestString\", but is \"" << marshal_as<std::string>(result[0]) << "\"";
}

TEST(marshal_as_collections_test, marshal_vector_of_ints_as_list)
{
	const std::vector<std::int32_t> nativeVector{ 123 };

	auto result = marshal_as<Int32>(nativeVector);

	ASSERT_NE(result, nullptr) << "The result == nullptr";
	ASSERT_MANAGED_TYPE_EQ(*result, (List<Int32>::typeid));
	ASSERT_EQ(result->Count, 1) << "The managed List should contain a single item, but has " << result->Count << " items";
	EXPECT_EQ(result[0], 123) << "The item in the list should be [123] but is [" << result[0] << "]";
}


TEST(marshal_as_collections_test, marshal_empty_vector_as_list)
{
	std::vector<std::string> nativeVector{};

	auto result = marshal_as<String^>(nativeVector);

	ASSERT_NE(result, nullptr) << "The result == nullptr";
	ASSERT_MANAGED_TYPE_EQ(*result, (List<String^>::typeid));
	EXPECT_EQ(result->Count, 0) << "Expected a list with 0 items, but found " << result->Count;
}

TEST(marshal_as_collections_test, marshal_map_of_strings_as_dictionary)
{
	const std::map<std::string, std::string> native_map{{"1", "a"}, {"2", "b"}, {"3", "c"}};

	auto result = marshal_as<String^, String^>(native_map);

	ASSERT_NE(result, nullptr) << "The result == nullptr";
	ASSERT_MANAGED_TYPE_EQ(*result, (Dictionary<String^, String^>::typeid));
	EXPECT_EQ(result->Count, 3) << "Expected a list with 3 items, but found " << result->Count;
	for (auto kv : native_map)
	{
		EXPECT_TRUE(result->Keys->Contains(marshal_as<String^>(kv.first))) << "The result is missing key " << kv.first;
		EXPECT_EQ(marshal_as<std::string>(result[marshal_as<String^>(kv.first)]), kv.second) << "The expected value for \"" << kv.first << "\" is \"" << kv.second << "\" but found \"" << marshal_as<std::string>(result[marshal_as<String^>(kv.first)]) << "\"";
	}
}

TEST(marshal_as_collections_test, marshal_map_of_ints_as_dictionary)
{
	const std::map<std::int32_t, std::int32_t> native_map{ {1, 1000}, {2, 2000}, {3, 3000} };

	auto result = marshal_as<Int32, Int32>(native_map);

	ASSERT_NE(result, nullptr) << "The result == nullptr";
	ASSERT_MANAGED_TYPE_EQ(*result, (Dictionary<Int32, Int32>::typeid));
	EXPECT_EQ(result->Count, 3) << "Expected a dictionary with 0 items, but found " << result->Count;
	for (auto kv : native_map)
	{
		EXPECT_TRUE(result->Keys->Contains(kv.first)) << "The result is missing key " << kv.first;
		EXPECT_EQ(result[kv.first], kv.second) << "The expected value for [" << kv.first << "] is [" << kv.second << "] but found [" << result[kv.first] << "]";
	}
}

TEST(marshal_as_collections_test, marshal_empty_map_as_dictionary)
{
	const std::map<std::string, std::string> native_map{};

	auto result = marshal_as<String^, String^>(native_map);

	ASSERT_NE(result, nullptr) << "The result == nullptr";
	ASSERT_MANAGED_TYPE_EQ(*result, (Dictionary<String^, String^>::typeid));
	EXPECT_EQ(result->Count, 0) << "Expected a dictionary with 0 items, but found " << result->Count;
}

TEST(marshal_as_collections_test, marshal_enumerable_of_strings_as_vector)
{
	using size_type = std::vector<std::string>::size_type;
	auto managed_enumerable = gcnew array<String^>{"a", "b", "c"};

	auto result = marshal_as<std::vector, std::string>(safe_cast<IEnumerable<String^>^>(managed_enumerable));

	ASSERT_TYPE_EQ(result, typeid(std::vector<std::string>));
	ASSERT_EQ(result.size(), 3) << "Expected a vector with 3 items, but found " << result.size();
	for (size_type i{0}; i < result.size(); i++)
	{
		auto expected = managed_enumerable[static_cast<int>(i)];
		EXPECT_EQ(result[i], marshal_as<std::string>(expected)) << "Expected element " << i << " to contain \"" << marshal_as<std::string>(expected) << "\" but found \"" << result[i] << "\"";
	}
}

TEST(marshal_as_collections_test, marshal_enumerable_of_ints_as_vector)
{
	using size_type = std::vector<std::string>::size_type;
	auto managed_enumerable = gcnew array<Int32>{1, 2, 3};

	auto result = marshal_as<std::vector, std::int32_t>(safe_cast<IEnumerable<Int32>^>(managed_enumerable));

	ASSERT_TYPE_EQ(result, typeid(std::vector<std::int32_t>));
	ASSERT_EQ(result.size(), 3) << "Expected a vector with 3 items, but found " << result.size();
	for (size_type i{ 0 }; i < result.size(); i++)
	{
		auto managed_i = static_cast<int>(i);
		EXPECT_EQ(result[i], managed_enumerable[managed_i]) << "Expected element " << i << " to contain [" << managed_enumerable[managed_i] << "] but found [" << result[i] << "]";
	}
}

TEST(marshal_as_collections_test, marshal_managed_array_of_same_size)
{
	auto managedArray = gcnew array<Int32>{1, 2, 3};
	auto result = marshal_as<std::array, std::int32_t, 3>(managedArray);

	ASSERT_TYPE_EQ(result, typeid(std::array<std::int32_t, 3>));
	EXPECT_EQ(result[0], 1);
	EXPECT_EQ(result[1], 2);
	EXPECT_EQ(result[2], 3);
}

TEST(marshal_as_collections_test, marshal_larger_managed_array)
{
	auto managedArray = gcnew array<Int32>{1, 2, 3, 4, 5};
	auto result = marshal_as<std::array, std::int32_t, 3>(managedArray);

	ASSERT_TYPE_EQ(result, typeid(std::array<std::int32_t, 3>));
	EXPECT_EQ(result[0], 1);
	EXPECT_EQ(result[1], 2);
	EXPECT_EQ(result[2], 3);
}

TEST(marshal_as_collections_test, marshal_smaller_managed_array)
{
	auto managedArray = gcnew array<Int32>{1, 2, 3};
	auto result = marshal_as<std::array, std::int32_t, 5>(managedArray);

	ASSERT_TYPE_EQ(result, typeid(std::array<std::int32_t, 5>));
	EXPECT_EQ(result[0], 1);
	EXPECT_EQ(result[1], 2);
	EXPECT_EQ(result[2], 3);
	EXPECT_EQ(result[3], 0);
	EXPECT_EQ(result[4], 0);
}
