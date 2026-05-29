using System;
using System.Collections.Generic;

namespace WebRtcNet;

/// <summary>
/// Represents the possible states of a <see cref="IRtcDtlsTransport">RTCDtlsTransport</see> object.
/// Note that this enum is currently identical to the <see cref="RtcIceConnectionState"/> enum used by
/// <see cref="IRtcPeerConnection.ConnectionState"/> and the <see cref="RtcIceTransportState"/> used by
/// <see cref="IRtcIceTransport.State"/>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate"/>
public enum RtcDtlsTransportState
{
	/// <summary>
	/// DTLS has not started negotiating yet.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-new"/>
	New,

	/// <summary>
	/// DTLS is in the process of negotiating a secure connection and verifying the remote fingerprint.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-connecting"/>
	Connecting,

	/// <summary>
	/// DTLS has completed negotiation of a secure connection and verified the remote fingerprint.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-connected"/>
	Connected,

	/// <summary>
	/// The transport has been closed intentionally as the result of receipt of a close_notify alert, or calling
	/// <see cref="IRtcPeerConnection.Close">close()</see>.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-closed"/>
	Closed,

	/// <summary>
	/// 
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-failed"/>
	Failed
};

/// <summary>
/// The IRtcDtlsTransport interface allows an application access to information about the Datagram Transport Layer Security
/// (DTLS) transport over which RTP and RTCP packets are sent and received by <see cref="IRtcRtpSender">RTCRtpSender</see>
/// and <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> objects, as well other data such as SCTP packets sent and
/// received by data channels. In particular, DTLS adds security to an underlying transport, and the IRtcDtlsTransport
/// interface allows access to information about the underlying transport and the security added. RTCDtlsTransport objects
/// are constructed as a result of calls to <see cref="IRtcPeerConnection.SetLocalDescription">SetLocalDescription()</see>
/// and <see cref="IRtcPeerConnection.SetRemoteDescription">setRemoteDescription()</see>. Each RTCDtlsTransport object
/// represents the DTLS transport layer for the RTP or RTCP <see cref="IRtcIceTransport.Component">component</see> of a
/// specific <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see>, or a group of
/// <see cref="IRtcRtpTransceiver">RTCRtpTransceivers</see> if such a group has been negotiated via
/// <see href="https://tools.ietf.org/html/rfc8843">[RFC8843]</see>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcdtlstransport-interface"/>
/// <seealso href="https://tools.ietf.org/html/rfc8843"/>
public abstract class IRtcDtlsTransport
{
	/// <summary>
	/// Initializes the DTLS transport wrapper.
	/// </summary>
	protected IRtcDtlsTransport()
	{
	}

	/// <summary>
	/// Returns the native DTLS transport interface used by WebRtcInterop.
	/// </summary>
	/// <param name="throwOnDisposed">True to throw when the transport has already been disposed.</param>
	internal abstract IntPtr GetNativeDtlsTransportHandle(bool throwOnDisposed);

	/// <summary>
	/// The IceTransport property is the underlying <see cref="IRtcIceTransport">transport</see> that is used to send and
	/// receive packets. The underlying <see cref="IRtcIceTransport">transport</see> may not be shared between multiple
	/// active RTCDtlsTransport objects.
	/// </summary>
	public abstract IRtcIceTransport IceTransport { get; }

	/// <summary>
	/// The current state of this transport.
	/// </summary>
	public abstract RtcDtlsTransportState State { get; }

	/// <summary>
	/// A list of certificates used by this transport.
	/// </summary>
	/// <returns></returns>
	public abstract IEnumerator<byte[]> GetRemoteCertificates();

	/// <summary>
	/// Fired when the RTCSctpTransport <see cref="State"/> changes.
	/// </summary>
	public abstract event EventHandler OnStateChange;

	/// <summary>
	/// Fired when an error occurred on the RTCDtlsTransport (either <see cref="RtcErrorDetailType.DtlsFailure"/> or
	/// <see cref="RtcErrorDetailType.FingerprintFailure"/>).
	/// </summary>
	public abstract event EventHandler<RtcErrorEventArgs> OnError;
};