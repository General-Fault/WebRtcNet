#include "pch.h"

#include <winerror.h>
#include "gtest/gtest.h"

using namespace WebRtcInterop;

class interop_hresult_tests
{};

TEST(interop_hresult_tests, log_if_failed_returns_false_for_success)
{
	EXPECT_FALSE(InteropHResult::LogIfFailed(S_OK, "success", "Interop.Tests"));
}

TEST(interop_hresult_tests, log_if_failed_returns_true_for_failure)
{
	EXPECT_TRUE(InteropHResult::LogIfFailed(E_FAIL, "failure", "Interop.Tests"));
}
