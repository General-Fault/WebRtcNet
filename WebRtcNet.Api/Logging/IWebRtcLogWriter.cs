namespace WebRtcNet.Logging;

/// <summary>
/// Managed interface that C++/CLI interop calls to enqueue structured log events.
/// Logs flow from native WebRTC threads through this interface to a managed channel,
/// then to background dequeue and ILogger.
/// </summary>
internal interface IWebRtcLogWriter
{
	/// <summary>
	/// Writes a structured log event to the log queue.
	/// May be called from arbitrary native threads; implementation must be thread-safe.
	/// </summary>
	void WriteLog(WebRtcLogEvent logEvent);
}
