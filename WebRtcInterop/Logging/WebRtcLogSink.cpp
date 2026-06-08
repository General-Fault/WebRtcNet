#include "pch.h"

#include "WebRtcLogSink.h"

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

	void WebRtcLogSink::OnLogMessage(const webrtc::LogLineRef& msg)
	{
		try
		{
			// Extract tag, message, and convert severity
			auto tag = marshal_as<String^>(std::string(msg.tag()));
			auto message = marshal_as<String^>(std::string(msg.message()));
			auto severity = ConvertSeverity(msg.severity());

			// Resolve category and EventId base
			String^ category = String::Empty;
			int eventIdBase = 0;
			ResolveCategoryAndEventId(tag, category, eventIdBase);

			// Get current thread ID
			int threadId = Threading::Thread::CurrentThread->ManagedThreadId;

			// Create log event
			WebRtcLogWriterBridge::WriteInteropLog(
				severity,
				eventIdBase,
				category,
				threadId,
				message);
		}
		catch (...)
		{
			// Suppress exceptions; do not disrupt native logging
		}
	}

	int WebRtcLogSink::ConvertSeverity(webrtc::LoggingSeverity severity)
	{
		switch (severity)
		{
		case webrtc::LS_VERBOSE:
			return 1;
		case webrtc::LS_INFO:
			return 2;
		case webrtc::LS_WARNING:
			return 3;
		case webrtc::LS_ERROR:
			return 4;
		case webrtc::LS_NONE:
			return 6;
		default:
			return 2;
		}
	}

	void WebRtcLogSink::ResolveCategoryAndEventId(
		String^ tag,
		String^% category,
		int% eventIdBase)
	{
		try
		{
			WebRtcLogWriterBridge::ResolveWebRtcCategory(tag, category, eventIdBase);
		}
		catch (...)
		{
			// Fallback: use WebRTC.Other
			category = "WebRTC.Other";
			eventIdBase = 1900;
		}
	}
}
