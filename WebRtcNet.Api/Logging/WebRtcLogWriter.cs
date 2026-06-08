using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebRtcNet.Logging;

/// <summary>
/// Implements the managed log writer with thread-safe .NET Channel.
/// Logs are enqueued from arbitrary native threads, dequeued on a background task,
/// and forwarded to ILogger.
/// </summary>
internal class WebRtcLogWriter : IWebRtcLogWriter, IDisposable
{
	private readonly Channel<WebRtcLogEvent> channel_ = Channel.CreateUnbounded<WebRtcLogEvent>();
	private readonly Task dequeue_task_;
	private bool disposed_;

	public WebRtcLogWriter()
	{
		// Start background dequeue task
		dequeue_task_ = DequeueAndLogAsync();
	}

	/// <summary>
	/// Writes a log event to the channel (thread-safe from native threads).
	/// </summary>
	public void WriteLog(WebRtcLogEvent logEvent)
	{
		if (logEvent == null)
			throw new ArgumentNullException(nameof(logEvent));

		if (disposed_)
			return;

		// Non-blocking write; if channel is closed, drop the message.
		channel_.Writer.TryWrite(logEvent);
	}

	/// <summary>
	/// Dequeues log events from channel and writes to ILogger by category.
	/// Runs on a background task.
	/// </summary>
	private async Task DequeueAndLogAsync()
	{
		try
		{
			await foreach (var logEvent in channel_.Reader.ReadAllAsync())
			{
				try
				{
					var logger = LoggerFactoryHolder.GetLogger(logEvent.Category);
					logger.Log(
						logEvent.Severity,
						logEvent.EventId,
						logEvent.Message,
						null,
						(msg, _) => msg);
				}
				catch (InvalidOperationException)
				{
					// Logger factory may be shutting down.
				}
			}
		}
		catch (InvalidOperationException)
		{
			// Channel closed while reading.
		}
	}

	public void Dispose()
	{
		if (disposed_)
			return;

		disposed_ = true;

		// Signal channel closure
		channel_.Writer.TryComplete();

		// Give background task time to drain (best effort)
		if (!dequeue_task_.Wait(TimeSpan.FromSeconds(1)))
			return;
	}
}
