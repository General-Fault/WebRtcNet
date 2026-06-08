using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace WebRtcNet.Logging;

/// <summary>
/// Internal singleton holder for the injected ILoggerFactory.
/// Provides thread-safe access to loggers throughout the library.
/// </summary>
internal static class LoggerFactoryHolder
{
	private static ILoggerFactory? logger_factory_;
	private static readonly object lock_ = new();

	/// <summary>
	/// Gets the current ILoggerFactory, or creates a default one if not set.
	/// </summary>
	public static ILoggerFactory Current
	{
		get
		{
			if (logger_factory_ != null)
				return logger_factory_;

			lock (lock_)
			{
				if (logger_factory_ != null)
					return logger_factory_;

				// Create default factory based on build configuration
				logger_factory_ = CreateDefaultFactory();
				return logger_factory_;
			}
		}
	}

	/// <summary>
	/// Sets the ILoggerFactory for the library.
	/// Must be called before creating MediaDevices or PeerConnection.
	/// </summary>
	public static void SetLoggerFactory(ILoggerFactory factory)
	{
		if (factory == null)
			throw new ArgumentNullException(nameof(factory));

		lock (lock_)
		{
			logger_factory_ = factory;
		}
	}

	/// <summary>
	/// Gets or creates a logger for the given category.
	/// </summary>
	public static ILogger GetLogger(string category)
	{
		if (string.IsNullOrEmpty(category))
			throw new ArgumentException("Category must not be null or empty.", nameof(category));
		return Current.CreateLogger(category);
	}

	private static ILoggerFactory CreateDefaultFactory()
	{
#if DEBUG
		// Debug builds: console logger with verbose output
		var factory = LoggerFactory.Create(builder =>
		{
			builder
				.SetMinimumLevel(LogLevel.Debug)
				.AddSimpleConsole(options =>
				{
					options.UseUtcTimestamp = false;
					options.IncludeScopes = true;
				});
		});
		return factory;
#else
		// Release builds: NullLogger (silent) unless app calls SetLoggerFactory
		return new NullLoggerFactory();
#endif
	}

	/// <summary>
	/// A no-op logger factory for Release builds.
	/// </summary>
	private class NullLoggerFactory : ILoggerFactory
	{
		public ILogger CreateLogger(string categoryName) => NullLogger.Instance;

		public void AddProvider(ILoggerProvider provider) { }

		public void Dispose() { }
	}
}
