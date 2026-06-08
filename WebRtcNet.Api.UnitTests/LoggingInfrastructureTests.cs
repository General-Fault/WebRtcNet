using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WebRtcNet.Logging;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
[NonParallelizable]
public class LoggingInfrastructureTests
{
	private ILoggerFactory original_logger_factory_ = null!;

	[SetUp]
	public void SetUp()
	{
		original_logger_factory_ = LoggerFactoryHolder.Current;
	}

	[TearDown]
	public void TearDown()
	{
		LoggerFactoryHolder.SetLoggerFactory(original_logger_factory_);
	}

	[Test]
	public void LogCategoryMapping_ResolveTagToCategory_MapsKnownAndFallbackTags()
	{
		var mapping = LogCategoryMapping.LoadFromResource();

		var (peerCategory, peerEventId) = mapping.ResolveTagToCategory("(peerconnection)");
		var (fallbackCategory, fallbackEventId) = mapping.ResolveTagToCategory("(unmapped.tag)");

		Assert.That(peerCategory, Is.EqualTo("WebRTC.PeerConnection"));
		Assert.That(peerEventId, Is.EqualTo(1000));
		Assert.That(fallbackCategory, Is.EqualTo("WebRTC.Other"));
		Assert.That(fallbackEventId, Is.EqualTo(1900));
	}

	[Test]
	public void WebRtcLogEventId_UsesConfiguredCategoryRanges()
	{
		Assert.That((int)WebRtcLogEventId.PeerConnectionStateChanged, Is.InRange(1000, 1999));
		Assert.That((int)WebRtcLogEventId.WebRtcOther, Is.InRange(1000, 1999));
		Assert.That((int)WebRtcLogEventId.AudioEnumerationStarted, Is.InRange(2000, 2999));
		Assert.That((int)WebRtcLogEventId.ConstraintError, Is.InRange(2000, 2999));
		Assert.That((int)WebRtcLogEventId.InteropMediaDevicesFailed, Is.InRange(3000, 3999));
		Assert.That((int)WebRtcLogEventId.InteropHResultFailure, Is.InRange(3000, 3999));
	}

	[Test]
	public void WebRtcLogWriter_WriteLog_ForwardsEventToLoggerFactoryCategory()
	{
		var factory = new CapturingLoggerFactory();
		LoggerFactoryHolder.SetLoggerFactory(factory);
		using var writer = new WebRtcLogWriter();
		var logEvent = new WebRtcLogEvent(
			DateTime.Now,
			LogLevel.Warning,
			new EventId((int)WebRtcLogEventId.PeerConnectionError, nameof(WebRtcLogEventId.PeerConnectionError)),
			"WebRTC.PeerConnection",
			123,
			"peer connection warning");

		writer.WriteLog(logEvent);

		var timeout = Stopwatch.StartNew();
		while (timeout.Elapsed < TimeSpan.FromSeconds(2))
		{
			if (factory.TryDequeue(out var captured))
			{
				Assert.That(captured, Is.Not.Null);
				var entry = captured!;
				Assert.That(entry.Category, Is.EqualTo("WebRTC.PeerConnection"));
				Assert.That(entry.Level, Is.EqualTo(LogLevel.Warning));
				Assert.That(entry.EventId.Id, Is.EqualTo((int)WebRtcLogEventId.PeerConnectionError));
				Assert.That(entry.Message, Is.EqualTo("peer connection warning"));
				return;
			}
		}

		Assert.Fail("Expected one captured log entry from WebRtcLogWriter.");
	}

	private sealed class CapturingLoggerFactory : ILoggerFactory
	{
		private readonly ConcurrentQueue<CapturedLogEntry> entries_ = new();

		public ILogger CreateLogger(string categoryName) => new CapturingLogger(categoryName, entries_);

		public void AddProvider(ILoggerProvider provider)
		{
		}

		public void Dispose()
		{
		}

		public bool TryDequeue(out CapturedLogEntry? entry) => entries_.TryDequeue(out entry);
	}

	private sealed class CapturingLogger(string category, ConcurrentQueue<CapturedLogEntry> entries) : ILogger
	{
		public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception? exception,
			Func<TState, Exception?, string> formatter)
		{
			entries.Enqueue(new CapturedLogEntry(category, logLevel, eventId, formatter(state, exception)));
		}
	}

	private sealed class NullScope : IDisposable
	{
		public static readonly NullScope Instance = new();

		public void Dispose()
		{
		}
	}

	private sealed record CapturedLogEntry(string Category, LogLevel Level, EventId EventId, string Message);
}
