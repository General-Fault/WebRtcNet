#pragma once

#include <gtest/gtest.h>
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

inline std::stringstream& operator<< (std::stringstream& output, System::String^ str)
{
    output << marshal_as<std::string>(str);
    return output;
}

#define ASSERT_MANAGED_STREQ(val1,val2) ASSERT_EQ(val1->CompareTo(val2), 0)
#define EXPECT_MANAGED_STREQ(val1,val2) EXPECT_EQ(val1->CompareTo(val2), 0)

#define ASSERT_TYPE_EQ(item, type) ASSERT_EQ(typeid(item), type) << "The expected result type is " << type.name() << ", but found " << typeid(item).name()
#define EXPECT_TYPE_EQ(item, type) EXPECT_EQ(typeid(item), type) << "The expected result type is " << type.name() << ", but found " << typeid(item).name()

#define ASSERT_MANAGED_TYPE_EQ(object, type) ASSERT_EQ((object).GetType(), type) << "The expected result type is " << ReflectionUtils::PrettyTypeName(type) << ", but found " << ReflectionUtils::PrettyTypeName((object).GetType())
#define EXPECT_MANAGED_TYPE_EQ(object, type) EXPECT_EQ((object).GetType(), type) << "The expected result type is " << ReflectionUtils::PrettyTypeName(type) << ", but found " << ReflectionUtils::PrettyTypeName((object).GetType())

ref class ReflectionUtils
{
public:
    static System::String^ PrettyTypeName(System::Type^ t)
    {
        if (t->IsArray)
        {
            return PrettyTypeName(t->GetElementType()) + "[]";
        }

        if (t->IsGenericType)
        {
            return System::String::Format(
                "{0}<{1}>",
                t->Name->Substring(0, t->Name->LastIndexOf("`", System::StringComparison::InvariantCulture)),
                System::String::Join(", ",
                    System::Linq::Enumerable::Select(
                        safe_cast<System::Collections::Generic::IEnumerable<System::Type^>^>(t->GetGenericArguments()),
                        gcnew System::Func<System::Type^, System::String^>(&ReflectionUtils::PrettyTypeName))));
        }

        return t->Name;
    }
};
