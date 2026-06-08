#pragma once

#include <api/rtc_event_log.h>
#include <rtc_base/logging.h>
#include <memory>

namespace WebRtcInterop::Logging
{
	/// <summary>
	/// Custom WebRTC log sink that forwards rtc::LogMessage events to managed IWebRtcLogWriter.
	/// Registers with WebRTC's logging system to capture all diagnostic output.
	/// </summary>
	class WebRtcLogSink : public rtc::LogSink
	{
	public:
		WebRtcLogSink();
		~WebRtcLogSink() override;

		/// <summary>
		/// Called by WebRTC for each log message.
		/// Extracts severity, tag, and message, then forwards to managed writer.
		/// </summary>
		void OnLogMessage(const rtc::LogMessage& msg) override;

		/// <summary>
		/// Called by WebRTC for each log message (alternate interface).
		/// </summary>
		void OnLogMessage(const System::String^ message) override { }

	private:
		/// <summary>
		/// Converts WebRTC severity level to .NET LogLevel using marshal_as.
		/// </summary>
		static System::Diagnostics::LogLevel ConvertSeverity(rtc::LoggingSeverity severity);

		/// <summary>
		/// Extracts category and EventId base from WebRTC tag via managed mapping.
		/// </summary>
		static void ResolveCategoryAndEventId(
			const System::String^ tag,
			System::String^% category,
			System::Int32% eventIdBase);
	};
}
