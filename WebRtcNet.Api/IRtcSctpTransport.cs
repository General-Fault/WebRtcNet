using System;

namespace WebRtcNet;

/// <summary>
/// The possible states of an SCTP transport.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcsctptransportstate-enum"/>
public enum RtcSctpTransportState
{
	/// <summary>
	/// The SCTP transport is establishing connectivity.
	/// </summary>
	Connecting,

	/// <summary>
	/// The SCTP transport is established.
	/// </summary>
	Connected,

	/// <summary>
	/// The SCTP transport is closed.
	/// </summary>
	Closed
}

/// <summary>
/// Represents the SCTP transport used for data channels.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcsctptransport-interface"/>
public abstract class IRtcSctpTransport
{
	/// <summary>
	/// Initializes the SCTP transport wrapper.
	/// </summary>
	protected IRtcSctpTransport()
	{
	}

	/// <summary>
	/// Returns the native SCTP transport interface used by WebRtcInterop.
	/// </summary>
	/// <param name="throwOnDisposed">True to throw when the transport has already been disposed.</param>
	internal abstract IntPtr GetNativeSctpTransportHandle(bool throwOnDisposed);

	/// <summary>
	/// Gets the underlying DTLS transport, if available.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsctptransport-transport"/>
	public abstract IRtcDtlsTransport? Transport { get; }

	/// <summary>
	/// Gets the current SCTP transport state.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsctptransport-state"/>
	public abstract RtcSctpTransportState State { get; }

	/// <summary>
	/// Gets the maximum message size in bytes, or <c>double.PositiveInfinity</c> when unbounded.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsctptransport-maxmessagesize"/>
	public abstract double MaxMessageSize { get; }

	/// <summary>
	/// Raised when <see cref="State"/> changes.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#event-rtcsctptransport-statechange"/>
	public abstract event EventHandler OnStateChange;
}
