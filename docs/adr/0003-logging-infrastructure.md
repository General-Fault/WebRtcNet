# Logging infrastructure using Microsoft.Extensions.Logging

WebRtcNet will adopt Microsoft.Extensions.Logging (MEL) as the standard for structured logging across managed and native interop code.

## Context

Issue #56 requires a consistent logging infrastructure to surface diagnostics and failures from both managed device enumeration and native WebRTC library operations. Without structured logging, audio/video device enumeration silently suppresses HRESULT failures (e.g., in MediaDevices.cpp:GetAudioDeviceLabel), making failures hard to diagnose.

## Decision

We chose **ILogger dependency injection via Host.SetLoggerFactory()** with the following characteristics:

### Architecture
- **Injection point** — App calls Host.SetLoggerFactory(ILoggerFactory) early in startup to configure logging; no DI container required
- **Default behavior** — Debug builds default to console logger; Release builds default to NullLogger (silent) if SetLoggerFactory not called
- **Managed layer** — WebRtcNet.Api queries injected factory for loggers by category
- **Interop layer** — C++/CLI forwards WebRTC rtc::LogSink events and HRESULT failures to managed code via .NET Channel
- **Threading** — WebRTC logs arrive on arbitrary native threads; a lock-free .NET Channel with background dequeue ensures thread-safe, non-blocking forwarding to managed ILogger

### Log Structure
All logs use:
- **Category** — Hierarchical category (e.g., "WebRTC.PeerConnection", "Interop.MediaDevices") for filtering and aggregation
- **EventId** — Numeric identifier for event type (ranges: WebRTC 1000-1999, Media 2000-2999, Interop 3000-3999) to enable telemetry grouping
- **Timestamp, Severity, ThreadId, Message** — Standard logging envelope

### EventId Strategy
- **WebRtcLogEventId enum** — Managed code (InteropHResult failures, MediaDevices enumeration) references enum constants
- **Tag-to-category mapping (JSON)** — WebRTC rtc::LogSink logs matched by regex pattern to category + eventIdBase; all events in a category initially share the same EventId

### WebRTC Log Sink
- **Eager initialization** — rtc::LogSink registered with WebRTC on RtcPeerConnectionFactory creation (not lazy)
- **Lock-free forwarding** — Logs pushed to .NET Channel from arbitrary WebRTC threads; background thread dequeues and writes to ILogger
- **Coverage** — Captures all WebRTC diagnostics (peer connection state, ICE, codec negotiation, audio/video device initialization, etc.)

## Rationale

### Why ILogger Injection vs. Alternatives?

**OnLogging Callback** — Simple but requires app to wire handlers; threading complexity from C++ → managed; harder to capture WebRTC library logs.

**Static/Singleton Registry** — Pragmatic but not idiomatic .NET; harder to unit test; violates DI principles.

**Hybrid (Callback + Optional ILogger)** — Flexible but dual maintenance paths and more complex.

We chose **ILogger Injection** because:
1. Standard .NET pattern, composable with DI containers (ASP.NET Core, generic host builder)
2. Aligns with ecosystem expectations (Serilog, NLog integrations)
3. Supports both simple and enterprise logging scenarios
4. Enables fine-grained filtering by category without app-side handler logic
5. No mandatory DI container required for this project (explicit Host.SetLoggerFactory call is minimal)

### Why .NET Channel for Threading?

**Synchronous direct marshaling** — Simple but risks blocking WebRTC threads or deadlock if ILogger throws.

**std::queue + mutex** — Not truly lock-free; overkill complexity for C++.

**moodycamel::ConcurrentQueue** — High-performance but external C++ dependency.

We chose **.NET Channel** because:
1. Async/await, structured concurrency (built-in backpressure)
2. Integrates naturally with managed async code (background thread via Task)
3. No external dependencies
4. Straightforward C++/CLI marshaling to queue on unmanaged thread, dequeue on managed thread
5. Naturally handles channel closure on shutdown

### Why Eager Initialization?

WebRTC log sink started on RtcPeerConnectionFactory creation (not lazy or opt-in) to ensure comprehensive capture from first peer connection creation. Decouples logging lifecycle from early Host API calls.

### Why Default Console Logger in Debug?

Developer ergonomics: engineers building or debugging WebRtcNet should see diagnostics without additional setup. Production apps explicitly call Host.SetLoggerFactory and choose their logging provider. Release builds default to NullLogger (silent) to avoid overhead in production.

## Consequences

### Positive
- **Unified logging surface** — App configures logging once (via SetLoggerFactory), captures diagnostics from all layers
- **Zero required setup** — Debug builds provide console logs out-of-the-box; production apps opt-in
- **Web-aligned semantics** — Category hierarchy mirrors W3C WebRTC spec components (PeerConnection, DataChannel, Media, Transport, etc.)
- **Extensible EventIds** — Enum-based ranges allow future refinement (e.g., per-event-type discrimination via message pattern-matching)
- **Production-ready** — MEL is well-tested, widely adopted; integrates with Application Insights, Seq, ELK, etc.
- **No DI container required** — Single static API call (Host.SetLoggerFactory) fits this project's architecture

### Negative
- **Manual configuration in Release** — Production apps must call Host.SetLoggerFactory(); no default
- **Background thread overhead** — Channel dequeue on background thread adds latency and thread pool work (minor at typical logging rates)
- **WebRTC thread blocking** — If ILogger is slow, Channel can back up; app responsible for fast logger implementations

## Future Refinement

1. **EventId discrimination** — Start with coarse (all events in category share eventIdBase); evolve to per-event-type EventIds if telemetry warrants
2. **Channel capacity** — Start unbounded; profile and cap if needed
3. **Host shutdown hook** — May need explicit channel drain on application shutdown
4. **Native WebRTC log sink configuration** — Options for enabling/disabling rtc::LogSink, filtering by severity
