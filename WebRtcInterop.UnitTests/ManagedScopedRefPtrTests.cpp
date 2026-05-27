#include "pch.h"

#include <rtc_base/ref_count.h>

#include "ManagedScopedRefPtr.h"
#include "TestUtils.h"

#include "gtest/gtest.h"

using namespace System;

using namespace WebRtcInterop;


class MockRefObject : public webrtc::RefCountInterface
{
public:
	MockRefObject() = default;
	~MockRefObject() override = default;

	void AddRef() const override
	{
		++add_ref_count_;
	}

	webrtc::RefCountReleaseStatus Release() const override
	{
		++release_count_;
		return webrtc::RefCountReleaseStatus::kOtherRefsRemained;
	}

	int add_ref_count() const { return add_ref_count_; }
	int release_count() const { return release_count_; }

private:
	mutable int add_ref_count_ = 0;
	mutable int release_count_ = 0;
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
	MockRefObject obj;

	ManagedScopedRefPtr<MockRefObject> ptr{&obj};
	EXPECT_EQ(obj.add_ref_count(), 1);
	EXPECT_EQ(obj.release_count(), 0);
}

TEST(managed_scoped_refptr_tests, ref_object_release_on_destruct)
{
	MockRefObject obj;

	{
		ManagedScopedRefPtr<MockRefObject> ptr{ &obj };
	}

	EXPECT_EQ(obj.add_ref_count(), 1);
	EXPECT_EQ(obj.release_count(), 1);
}