#pragma once

#include <rtc_base/logging.h>
#include <memory>

namespace WebRtcInterop::Logging
{
	/// <summary>
	/// Custom WebRTC log sink that forwards rtc::LogMessage events to managed IWebRtcLogWriter.
	/// Registers with WebRTC's logging system to capture all diagnostic output.
	/// </summary>
	class WebRtcLogSink : public webrtc::LogSink
	{
	public:
		WebRtcLogSink();
		~WebRtcLogSink() override;

		/// <summary>
		/// Called by WebRTC for each log message.
		/// Extracts severity, tag, and message, then forwards to managed writer.
		/// </summary>
		void OnLogMessage(const webrtc::LogLineRef& msg) override;

		/// <summary>
		/// Called by WebRTC for each log message (alternate interface).
		/// </summary>
		void OnLogMessage(const std::string& message) override { }

	private:
		/// <summary>
		/// Converts WebRTC severity level to Microsoft.Extensions.Logging.LogLevel numeric values.
		/// </summary>
		static int ConvertSeverity(webrtc::LoggingSeverity severity);

		/// <summary>
		/// Extracts category and EventId base from WebRTC tag via managed mapping.
		/// </summary>
		static void ResolveCategoryAndEventId(
			System::String^ tag,
			System::String^% category,
			System::Int32% eventIdBase);
	};
}
