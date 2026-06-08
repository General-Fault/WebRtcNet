using System;
using System.Diagnostics.Contracts;

namespace WebRtcNet.Logging;

/// <summary>
/// Bridge between C++/CLI interop and managed log writer.
/// Exposes a singleton IWebRtcLogWriter instance that C++/CLI can call.
/// </summary>
internal static class WebRtcLogWriterBridge
{
	private static IWebRtcLogWriter? writer_;
	private static readonly object lock_ = new();

	/// <summary>
	/// Gets or creates the log writer singleton.
	/// Called from C++/CLI to enqueue log events.
	/// </summary>
	public static IWebRtcLogWriter Instance
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
