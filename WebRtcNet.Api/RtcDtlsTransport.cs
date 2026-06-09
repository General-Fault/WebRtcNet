using System;
using System.Collections.Generic;

namespace WebRtcNet;

/// <summary>
/// Represents the possible states of a <see cref="RtcDtlsTransport">RTCDtlsTransport</see> object.
/// Note that this enum is currently identical to the <see cref="RtcIceConnectionState"/> enum used by
/// <see cref="RtcPeerConnection.ConnectionState"/> and the <see cref="RtcIceTransportState"/> used by
/// <see cref="RtcIceTransport.State"/>.
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
	/// <see cref="RtcPeerConnection.Close">close()</see>.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-closed"/>
	Closed,

	/// <summary>
	/// DTLS negotiation failed and the transport can no longer be used.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransportstate-failed"/>
	Failed
};

/// <summary>
/// The RtcDtlsTransport interface allows an application access to information about the Datagram Transport Layer Security
/// (DTLS) transport over which RTP and RTCP packets are sent and received by <see cref="RtcRtpSender">RTCRtpSender</see>
/// and <see cref="RtcRtpReceiver">RTCRtpReceiver</see> objects, as well other data such as SCTP packets sent and
/// received by data channels. In particular, DTLS adds security to an underlying transport, and the RtcDtlsTransport
/// interface allows access to information about the underlying transport and the security added. RTCDtlsTransport objects
/// are constructed as a result of calls to <see cref="RtcPeerConnection.SetLocalDescription">SetLocalDescription()</see>
/// and <see cref="RtcPeerConnection.SetRemoteDescription">setRemoteDescription()</see>. Each RTCDtlsTransport object
/// represents the DTLS transport layer for the RTP or RTCP <see cref="RtcIceTransport.Component">component</see> of a
/// specific <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see>, or a group of
/// <see cref="RtcRtpTransceiver">RTCRtpTransceivers</see> if such a group has been negotiated via
/// <see href="https://tools.ietf.org/html/rfc8843">[RFC8843]</see>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcdtlstransport-interface"/>
/// <seealso href="https://tools.ietf.org/html/rfc8843"/>
public abstract class RtcDtlsTransport
{
	/// <summary>
	/// Initializes the DTLS transport wrapper.
	/// </summary>
	protected RtcDtlsTransport()
	{
	}

	/// <summary>
	/// The IceTransport property is the underlying <see cref="RtcIceTransport">transport</see> that is used to send and
	/// receive packets. The underlying <see cref="RtcIceTransport">transport</see> may not be shared between multiple
	/// active RTCDtlsTransport objects.
	/// </summary>
	public abstract RtcIceTransport IceTransport { get; }

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
	/// Fired when the RTCDtlsTransport <see cref="State"/> changes.
	/// </summary>
	public abstract event EventHandler OnStateChange;

	/// <summary>
	/// Fired when an error occurred on the RTCDtlsTransport (either <see cref="RtcErrorDetailType.DtlsFailure"/> or
	/// <see cref="RtcErrorDetailType.FingerprintFailure"/>).
	/// </summary>
	public abstract event EventHandler<RtcErrorEventArgs> OnError;
};