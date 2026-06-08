#pragma once

#include <winerror.h>

namespace WebRtcInterop
{
	public ref class InteropHResult abstract sealed
	{
	public:
		static void ThrowIfFailed(HRESULT hr, System::String^ message);
		static bool LogIfFailed(HRESULT hr, System::String^ operation, System::String^ category);
	};
}
