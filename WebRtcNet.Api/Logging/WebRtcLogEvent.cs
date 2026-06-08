using System;
using Microsoft.Extensions.Logging;

namespace WebRtcNet.Logging;

/// <summary>
/// Represents a structured log event from WebRTC or interop layers.
/// </summary>
/// <param name="Timestamp">When the event occurred.</param>
/// <param name="Severity">Log level (Trace, Debug, Information, Warning, Error, Critical).</param>
/// <param name="EventId">Structured event identifier for telemetry and grouping.</param>
/// <param name="Category">Logger category (e.g., "WebRTC.PeerConnection", "Interop.MediaDevices").</param>
/// <param name="ThreadId">Managed thread ID where the event was logged.</param>
/// <param name="Message">Log message.</param>
public record WebRtcLogEvent(
	DateTime Timestamp,
	LogLevel Severity,
	EventId EventId,
	string Category,
	int ThreadId,
	string Message);
