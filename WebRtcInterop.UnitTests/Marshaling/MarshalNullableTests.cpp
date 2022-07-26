#include "pch.h"

#include "third_party/abseil-cpp/absl/types/optional.h"

#include "Marshaling/MarshalNullable.h"
#include <gtest/gtest.h>

#include "TestUtils.h"

using namespace msclr::interop;
using namespace System;

class marshal_as_nullable_test
{
};


TEST(marshal_as_nullable_test, marshal_optional_with_value_as_nullable)
{
	const absl::optional<int> native_optional{ 123 };

	auto result = marshal_as<Int32>(native_optional);

	ASSERT_TRUE(result.HasValue) << "The Nullable should have a value but does not";
	ASSERT_EQ(result.Value, 123);
}

TEST(marshal_as_nullable_test, marshal_optional_without_value_as_nullable)
{
	const absl::optional<int> native_optional;

	auto result = marshal_as<Int32>(native_optional);

	ASSERT_FALSE(result.HasValue) << "The optional should not have a value";
}

TEST(marshal_as_nullable_test, marshal_nullable_with_value_as_optional)
{
	Nullable<Int32> nullable(123);
	
	auto result = marshal_as<std::int32_t, Int32>(nullable);
	
	ASSERT_TRUE(result.has_value()) << "The optional should have a value but does not";
	ASSERT_EQ(result.value(), 123);
}

TEST(marshal_as_nullable_test, marshal_nullable_without_value_as_optional)
{
	Nullable<Int32> nullable = Nullable<Int32>();
	
	auto result = marshal_as<std::int32_t, Int32>(nullable);
	
	ASSERT_FALSE(result.has_value()) << "The optional should have a value but does not";
}