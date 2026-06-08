using System;
using Microsoft.Extensions.Logging;

namespace WebRtcNet.Logging;

/// <summary>
/// Bridge between C++/CLI interop and managed log writer.
/// Exposes a singleton IWebRtcLogWriter instance that C++/CLI can call.
/// </summary>
public static class WebRtcLogWriterBridge
{
	private static IWebRtcLogWriter? writer_;
	private static readonly object lock_ = new();

	/// <summary>
	/// Gets or creates the log writer singleton.
	/// Called from C++/CLI to enqueue log events.
	/// </summary>
	internal static IWebRtcLogWriter Instance
	{
		get
		{
			if (writer_ != null)
				return writer_;

			lock (lock_)
			{
				if (writer_ != null)
					return writer_;

				writer_ = new WebRtcLogWriter();
				return writer_;
			}
		}
	}

	/// <summary>
	/// Sets the shared logger factory used by all managed and interop logs.
	/// </summary>
	public static void SetLoggerFactory(ILoggerFactory factory)
	{
		if (factory == null)
			throw new ArgumentNullException(nameof(factory));
		LoggerFactoryHolder.SetLoggerFactory(factory);
	}

	/// <summary>
	/// Resolves a WebRTC tag to category and EventId base values.
	/// </summary>
	public static void ResolveWebRtcCategory(string tag, out string category, out int eventIdBase)
	{
		var mapping = LogCategoryMapping.LoadFromResource();
		var resolved = mapping.ResolveTagToCategory(tag);
		category = resolved.Category;
		eventIdBase = resolved.EventIdBase;
	}

	/// <summary>
	/// Writes an interop-originated log entry using primitive arguments.
	/// </summary>
	public static void WriteInteropLog(
		int severity,
		int eventId,
		string category,
		int threadId,
		string message)
	{
		if (string.IsNullOrEmpty(category))
			category = "Interop.Other";
		message ??= string.Empty;

		var resolvedSeverity = Enum.IsDefined(typeof(LogLevel), severity)
			? (LogLevel)severity
			: LogLevel.Information;

		var logEvent = new WebRtcLogEvent(
			DateTime.Now,
			resolvedSeverity,
			new EventId(eventId, category),
			category,
			threadId,
			message);
		Instance.WriteLog(logEvent);
	}

	/// <summary>
	/// Disposes the writer singleton (called on app shutdown).
	/// </summary>
	public static void Shutdown()
	{
		lock (lock_)
		{
			(writer_ as IDisposable)?.Dispose();
			writer_ = null;
		}
	}
}
