#pragma once

#include <winerror.h>

namespace WebRtcInterop
{
	public ref class InteropHResult abstract sealed
	{
	public:
		static void ThrowIfFailed(HRESULT hr, System::String^ message);
	};
}
