#include "pch.h"

#include "InteropHResult.h"

#include <windows.h>

using namespace System::ComponentModel;
using namespace System::Runtime::InteropServices;
using namespace System::Threading;
using namespace System;
using namespace WebRtcNet::Logging;

namespace WebRtcInterop
{
	namespace
	{
		String^ TrimSystemMessage(String^ message)
		{
			if (String::IsNullOrWhiteSpace(message))
				return "Unknown system error";

			return message->Trim();
		}

		String^ FormatSystemMessage(const HRESULT hr)
		{
			LPWSTR rawMessage = nullptr;
			const auto flags =
				FORMAT_MESSAGE_ALLOCATE_BUFFER |
				FORMAT_MESSAGE_FROM_SYSTEM |
				FORMAT_MESSAGE_IGNORE_INSERTS;
			auto messageId = static_cast<DWORD>(hr);
			auto length = FormatMessageW(
				flags,
				nullptr,
				messageId,
				0,
				reinterpret_cast<LPWSTR>(&rawMessage),
				0,
				nullptr);

			if (length == 0 && HRESULT_FACILITY(hr) == FACILITY_WIN32)
			{
				messageId = HRESULT_CODE(hr);
				length = FormatMessageW(
					flags,
					nullptr,
					messageId,
					0,
					reinterpret_cast<LPWSTR>(&rawMessage),
					0,
					nullptr);
			}

			if (length == 0 || rawMessage == nullptr)
				return "Unknown system error";

			try
			{
				return TrimSystemMessage(gcnew String(rawMessage));
			}
			finally
			{
				LocalFree(rawMessage);
			}
		}
	}

	void InteropHResult::ThrowIfFailed(HRESULT hr, System::String^ message)
	{
		if (SUCCEEDED(hr))
			return;

		if (HRESULT_FACILITY(hr) == FACILITY_WIN32)
			throw gcnew Win32Exception(HRESULT_CODE(hr), message);

		throw gcnew COMException(message, hr);
	}

	bool InteropHResult::LogIfFailed(HRESULT hr, String^ operation, String^ category)
	{
		if (SUCCEEDED(hr))
			return false;

		if (String::IsNullOrWhiteSpace(operation))
			operation = "Interop operation";
		if (String::IsNullOrWhiteSpace(category))
			category = "Interop.HResult";

		try
		{
			const auto formatted = String::Format(
				"{0} failed. HRESULT=0x{1:X8} ({2}), Facility={3}, Code={4}, SystemMessage=\"{5}\"",
				operation,
				static_cast<System::UInt32>(hr),
				hr,
				HRESULT_FACILITY(hr),
				HRESULT_CODE(hr),
				FormatSystemMessage(hr));

			WebRtcNet::Logging::WebRtcLogWriterBridge::WriteInteropLog(
				4,
				static_cast<int>(WebRtcLogEventId::InteropHResultFailure),
				category,
				Thread::CurrentThread->ManagedThreadId,
				formatted);
		}
		catch (...)
		{
		}

		return true;
	}
}
