#include "pch.h"

#include "InteropHResult.h"

using namespace System::ComponentModel;
using namespace System::Runtime::InteropServices;

namespace WebRtcInterop
{
	void InteropHResult::ThrowIfFailed(HRESULT hr, System::String^ message)
	{
		if (SUCCEEDED(hr))
			return;

		if (HRESULT_FACILITY(hr) == FACILITY_WIN32)
			throw gcnew Win32Exception(HRESULT_CODE(hr), message);

		throw gcnew COMException(message, hr);
	}
}
