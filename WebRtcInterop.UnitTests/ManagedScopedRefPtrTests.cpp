#include "pch.h"

#include <rtc_base/ref_count.h>

#include "ManagedScopedRefPtr.h"
#include "TestUtils.h"

#include "gmock/gmock.h"
#include "gtest/gtest.h"

using namespace System;

using namespace WebRtcInterop;
using namespace testing;


public class MockRefObject : public rtc::RefCountInterface
{
public:
	MockRefObject() { std::cerr << "Constructor" << std::endl; };
	~MockRefObject() override { std::cerr << "Destructor" << std::endl; };

	MOCK_METHOD(void, AddRef, (), (const, override));
	MOCK_METHOD(rtc::RefCountReleaseStatus, Release, (), (const, override));

private:
	MockRefObject(const MockRefObject&) = delete;
	MockRefObject& operator= (const MockRefObject&) = delete;
};

ref class Container
{
public:
	~Container() { this->!Container();  }
	Container(MockRefObject* obj) : rp_ptr_(obj){}
	!Container() { rp_ptr_ = nullptr; };

private:
	ManagedScopedRefPtr<MockRefObject> rp_ptr_;
};

class managed_scoped_refptr_tests
{};

TEST(managed_scoped_refptr_tests, ref_object_addref_on_construct)
{
	InSequence s;
	MockRefObject obj;
	EXPECT_CALL(obj, AddRef()).Times(1);

	ManagedScopedRefPtr<MockRefObject> ptr{&obj};
}

TEST(managed_scoped_refptr_tests, ref_object_release_on_destruct)
{
	InSequence s;
	MockRefObject obj;
	EXPECT_CALL(obj, Release()).Times(1);

	{
		ManagedScopedRefPtr<MockRefObject> ptr{ &obj };
	}
}