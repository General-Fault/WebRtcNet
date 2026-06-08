#include "pch.h"

#include "WebRtcLogSink.h"
#include "..\Marshaling\MarshalLogging.h"
#include <msclr/marshal_cppclassm.h>

using namespace System;
using namespace WebRtcNet::Logging;

namespace WebRtcInterop::Logging
{
	WebRtcLogSink::WebRtcLogSink()
	{
	}

	WebRtcLogSink::~WebRtcLogSink()
	{
	}

	void WebRtcLogSink::OnLogMessage(const rtc::LogMessage& msg)
	{
		try
		{
			// Extract tag, message, and convert severity
			auto tag = gcnew String(msg.tag);
			auto message = gcnew String(msg.str().c_str());
			auto severity = ConvertSeverity(msg.severity);

			// Resolve category and EventId base
			String^ category = String::Empty;
			int eventIdBase = 0;
			ResolveCategoryAndEventId(tag, category, eventIdBase);

			// Get current thread ID
			int threadId = Threading::Thread::CurrentThread->ManagedThreadId;

			// Create log event
			auto logEvent = gcnew WebRtcLogEvent(
				DateTime::Now,
				severity,
				gcnew EventId(eventIdBase, category),
				category,
				threadId,
				message);

			// Write to managed writer
			WebRtcLogWriterBridge::Instance->WriteLog(logEvent);
		}
		catch (...)
		{
			// Suppress exceptions; do not disrupt native logging
		}
	}

	System::Diagnostics::LogLevel WebRtcLogSink::ConvertSeverity(rtc::LoggingSeverity severity)
	{
		return marshal_as<System::Diagnostics::LogLevel>(severity);
	}

	void WebRtcLogSink::ResolveCategoryAndEventId(
		const String^ tag,
		String^% category,
		int% eventIdBase)
	{
		try
		{
			auto mapping = LogCategoryMapping::LoadFromResource();
			auto [cat, base] = mapping->ResolveTagToCategory(tag);
			category = cat;
			eventIdBase = base;
		}
		catch (...)
		{
			// Fallback: use WebRTC.Other
			category = "WebRTC.Other";
			eventIdBase = 1900;
		}
	}
}
