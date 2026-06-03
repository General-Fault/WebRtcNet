using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRtcNet.Media;

namespace WebRtcNet;

/// <summary>
/// Describes the possible states of the PeerConnection connection. Note this is currently identical to
/// <see cref="RtcIceTransportState" />.
/// </summary>
/// <seealso cref="RtcPeerConnection.ConnectionState" />
/// <seealso href="http://www.w3.org/TR/webrtc/#rtciceconnectionstate-enum" />
public enum RtcIceConnectionState
{
	/// <summary>
	/// The ICE Agent is gathering addresses and / or waiting for remote candidates to be supplied.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-new" />
	New,

	/// <summary>
	/// The ICE Agent has received remote candidates on at least one component, and is checking candidate pairs but has not
	/// yet found a connection. In addition to checking, it may also still be gathering.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-checking" />
	Checking,

	/// <summary>
	/// The ICE Agent has found a usable connection for all components but is still checking other candidate pairs to see
	/// if there is a better connection.It may also still be gathering.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-connected" />
	Connected,

	/// <summary>
	/// The ICE Agent has finished gathering and checking and found a connection for all components.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-completed" />
	Completed,

	/// <summary>
	/// The ICE Agent is finished checking all candidate pairs and failed to find a connection for at least one component.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-failed" />
	Failed,

	/// <summary>
	/// Liveness checks have failed for one or more components. This is more aggressive than failed, and may trigger
	/// intermittently(and resolve itself without action) on a flaky network.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-disconnected" />
	Disconnected,

	/// <summary>
	/// The ICE Agent has shut down and is no longer responding to STUN requests.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-closed" />
	Closed
}

/// <summary>
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate" />
public enum RtcSignalingState
{
	/// <summary>
	/// There is no offer­/answer exchange in progress. This is also the initial state in which case the local and remote
	/// <see cref="RtcSessionDescription">descriptions</see> are empty.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-stable" />
	Stable,

	/// <summary>
	/// A local description, of type <see cref="RtcSdpType.Offer">offer</see>, has been successfully applied.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-local-offer" />
	HaveLocalOffer,

	/// <summary>
	/// A remote description, of type <see cref="RtcSdpType.Offer">offer</see>, has been successfully applied.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-remote-offer" />
	HaveRemoteOffer,

	/// <summary>
	/// A remote description of type <see cref="RtcSdpType.Offer">offer</see> has been successfully applied and a local
	/// description of type <see cref="RtcSdpType.PrAnswer">pranswer</see> has been successfully applied
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-local-pranswer" />
	HaveLocalPrAnswer,

	/// <summary>
	/// A local description of type <see cref="RtcSdpType.Offer">offer</see> has been successfully applied and a remote
	/// description of type <see cref="RtcSdpType.PrAnswer">pranswer</see> has been successfully applied.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-remote-pranswer" />
	HaveRemotePrAnswer,

	/// <summary>
	/// The connection is closed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-closed" />
	Closed
}

/// <summary>
/// Represents the possible states of the RtcPeerConnection
/// </summary>
/// <see cref="RtcPeerConnection.ConnectionState" />
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcpeerconnectionstate-enum" />
public enum RtcPeerConnectionState
{
	/// <summary>
	/// The <see cref="RtcPeerConnection">RtcPeerConnection</see> is closed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-closed" />
	Closed,

	/// <summary>
	/// The previous state doesn't apply and any <see cref="RtcIceTransport">RTCIceTransports</see> are in the
	/// <see cref="RtcIceTransportState.Failed">failed</see> state or any
	/// <see cref="RtcDtlsTransport">RTCDtlsTransports</see> are in the
	/// <see cref="RtcIceTransportState.Failed">failed</see> state.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-failed" />
	Failed,

	/// <summary>
	/// None of the previous states apply and any <see cref="RtcIceTransport">RTCIceTransports</see> are in the
	/// <see cref="RtcIceTransportState.Disconnected">disconnected</see> state.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-disconnected" />
	Disconnected,

	/// <summary>
	/// None of the previous states apply and all <see cref="RtcIceTransport">RTCIceTransports</see> are in the
	/// <see cref="RtcIceTransportState.New">new</see> or <see cref="RtcIceTransportState.Closed">closed</see> state, and
	/// all <seealso cref="RtcDtlsTransport">RTCDtlsTransports</seealso> are in the
	/// <see cref="RtcDtlsTransportState.New">new</see> or <see cref="RtcDtlsTransportState.Closed">closed</see> state, or
	/// there are no transports.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-new" />
	New,

	/// <summary>
	/// None of the previous states apply and any <see cref="RtcIceTransport">RTCIceTransports</see> is in the
	/// <see cref="RtcIceTransportState.Checking">checking</see> state or any
	/// <see cref="RtcDtlsTransport">RTCDtlsIceTransport</see> is in the
	/// <see cref="RtcDtlsTransportState.Connecting">connecting</see> state.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-connecting" />
	Connecting,

	/// <summary>
	/// None of the previous states apply and all <see cref="RtcIceTransport">RTCIceTransports</see> are in the
	/// <see cref="RtcIceTransportState.Connected">connected</see>,
	/// <see cref="RtcIceTransportState.Completed">completed</see> or <see cref="RtcIceTransportState.Closed">closed</see>
	/// state, and all <see cref="RtcIceTransport">RTCDtlsTransports</see> are in the
	/// <see cref="RtcDtlsTransportState.Connected">connected</see> or
	/// <see cref="RtcDtlsTransportState.Closed">closed</see> state.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-connected" />
	Connected
}

/// <Summary>
/// A .Net implementation of the WebRTC RTCPeerConnection Interface (W3C Recommendation 26 January 2021)
/// </Summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#rtcpeerconnection-interface" />
/// <seealso href="https://www.w3.org/TR/webrtc/#interface-definition" />
public abstract class RtcPeerConnection
{
	/// <summary>
	/// Initializes the peer connection wrapper.
	/// </summary>
	protected RtcPeerConnection()
	{
	}

	/// <summary>
	/// Returns the native peer connection interface used by WebRtcInterop.
	/// </summary>
	/// <param name="throwOnDisposed">True to throw when the peer connection has already been disposed.</param>
	protected internal abstract IntPtr GetNativePeerConnectionHandle(bool throwOnDisposed);

	/// <summary>
	/// The local <see cref="RtcSessionDescription">RTCSessionDescription</see> that was successfully set using
	/// <see cref="SetLocalDescription" />, plus any local <see cref="RtcIceCandidate">candidates</see> that have been
	/// generated by the ICE Agent since then.  A null object will be returned if the local description has not yet been
	/// set.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-localdescription" />
	public abstract RtcSessionDescription? LocalDescription { get; }

	/// <summary>
	/// Represents the local description that was successfully negotiated the last time the RTCPeerConnection transitioned
	/// into the stable state plus any local <see cref="RtcIceCandidate">candidates</see> that have been generated by the
	/// ICE Agent since the offer or answer was created.
	/// </summary>
	/// <seealso cref="CreateOffer" />
	/// <seealso cref="CreateAnswer" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-currentlocaldesc" />
	public abstract RtcSessionDescription? CurrentLocalDescription { get; }

	/// <summary>
	/// Represents a local <see cref="RtcSessionDescription">description</see> that is in the process of being negotiated
	/// plus any local candidates that have been generated by the ICE Agent since the <see cref="CreateOffer">offer</see>
	/// or <see cref="CreateAnswer">answer</see> was created. If the RTCPeerConnection is in the stable state, the value is
	/// null.
	/// </summary>
	/// <seealso cref="CreateOffer" />
	/// <seealso cref="CreateAnswer" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-pendinglocaldesc" />
	public abstract RtcSessionDescription? PendingLocalDescription { get; }

	/// <summary>
	/// The <see cref="RtcSessionDescription">RemoteDescription</see> that was successfully set using
	/// <see cref="SetRemoteDescription" />, plus any remote candidates that have been supplied via
	/// <see cref="AddIceCandidate" /> since then. A null object will be returned if the remote description has not yet
	/// been set.
	/// </summary>
	/// <seealso cref="SetRemoteDescription" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-remotedescription" />
	public abstract RtcSessionDescription? RemoteDescription { get; }

	/// <summary>
	/// It represents the last remote <see cref="RtcSessionDescription">description</see> that was successfully negotiated
	/// the last time the RTCPeerConnection transitioned into the stable state plus any remote candidates that have been
	/// supplied via <see cref="AddIceCandidate" /> since the offer or answer was created.
	/// </summary>
	/// <seealso cref="CreateOffer" />
	/// <seealso cref="CreateAnswer" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-currentremotedesc" />
	public abstract RtcSessionDescription? CurrentRemoteDescription { get; }

	/// <summary>
	/// It represents a remote description that is in the process of being negotiated, complete with any remote
	/// <see cref="RtcIceCandidate">candidates</see> that have been supplied via <see cref="AddIceCandidate" /> since the
	/// offer or answer was created. If the RTCPeerConnection is in the stable state, the value is null.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-pendingremotedesc" />
	public abstract RtcSessionDescription? PendingRemoteDescription { get; }

	/// <summary>
	/// The signaling state of the RtcPeerConnection.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-signaling-state" />
	public abstract RtcSignalingState SignalingState { get; }

	/// <summary>
	/// The gathering state of the RtcPeerConnection ICE Agent.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-ice-gathering-state" />
	public abstract RtcIceGatheringState IceGatheringState { get; }

	/// <summary>
	/// The ICE connection state of the RtcPeerConnection ICE Agent.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-ice-connection-state" />
	public abstract RtcIceConnectionState IceConnectionState { get; }

	/// <summary>
	/// The current connection state of the RtcPeerConnection and its transports.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-connection-state" />
	public abstract RtcPeerConnectionState ConnectionState { get; }

	/// <summary>
	/// Dictates whether the remote peer is able to accept trickled ICE candidates
	/// <see href="https://www.w3.org/TR/webrtc/#bib-rfc8829">[RFC8838]</see>. The value is determined based on whether a
	/// remote description indicates support for trickle ICE, as defined in
	/// <see href="https://tools.ietf.org/html/rfc8829#section-4.1.17">[RFC8829] (section 4.1.17.)</see>. Prior to the
	/// completion of setRemoteDescription, this value is null.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-cantrickleicecandidates" />
	public abstract bool? CanTrickleIceCandidates { get; }

	/// <summary>
	/// Gets or sets the <see cref="RtcConfiguration" /> object representing the current configuration of this
	/// RtcPeerConnection object. Setting the configuration updates the ICE Agent process of gathering local
	/// <see cref="RtcIceCandidate">candidates</see> and pinging remote <see cref="RtcIceCandidate">candidates</see>.
	/// This call may result in a change to the state of the ICE Agent, and may result in a change to media state if it
	/// results in connectivity being established.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-getconfiguration" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-setconfiguration" />
	public abstract RtcConfiguration Configuration { get; set; }

	/// <summary>
	/// Gets the SCTP transport used for data channels, if available.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-sctp" />
	public abstract RtcSctpTransport? Sctp { get; }

	/// <summary>
	/// Creates a certificate for use with peer connections.
	/// </summary>
	/// <param name="keygenAlgorithm">The key generation algorithm descriptor.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-generatecertificate" />
	public static Task<RtcCertificate> GenerateCertificate(object keygenAlgorithm)
	{
		throw new NotSupportedException();
	}

	/// <summary>
	/// The CreateOffer method generates a <see cref="RtcSessionDescription" />
	/// <see href="http://tools.ietf.org/html/rfc3264">[SDP]</see> that contains an
	/// <see href="https://tools.ietf.org/html/rfc3264">RFC 3264</see> offer with the supported configurations for the
	/// session, including <see cref="RtcSessionDescription">descriptions</see> of the local
	/// <see cref="MediaStreamTrack">MediaStreamTracks</see> attached to this RTCPeerConnection, the codec/RTP/RTCP
	/// options supported by this implementation, and any <see cref="RtcIceCandidate">candidates</see> that have been
	/// gathered by the ICE Agent. The <see cref="RtcOfferOptions">options</see> parameter may be supplied to provide
	/// additional control over the offer generated.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-createoffer" />
	/// <exception cref="CreateSessionDescriptionFailure" />
	public abstract Task<RtcSessionDescription> CreateOffer(RtcOfferOptions? options = null);

	/// <summary>
	/// Generates a <see cref="RtcSessionDescription" /> <see href="http://tools.ietf.org/html/rfc3264">[SDP]</see> answer
	/// with the supported configuration for the session that is compatible with the parameters in the remote
	/// configuration. Like <see cref="CreateOffer" />, the returned <see cref="RtcSessionDescription" /> contains
	/// <see cref="RtcSessionDescription">descriptions</see> of the local <see cref="MediaStream">MediaStreams</see>
	/// attached to this RTCPeerConnection, the codec/RTP/RTCP options negotiated for this session, and any
	/// <see cref="RtcIceCandidate">candidates</see> that have been gathered by the ICE Agent. The
	/// <see cref="RtcAnswerOptions">options</see> parameter may be supplied to provide additional control over the
	/// generated answer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-createanswer" />
	/// <exception cref="CreateSessionDescriptionFailure" />
	public abstract Task<RtcSessionDescription> CreateAnswer(RtcAnswerOptions? options = null);

	/// <summary>
	/// The SetLocalDescription() method instructs the RtcPeerConnection to apply the supplied
	/// <see cref="RtcLocalSessionDescriptionInit">description</see> as the
	/// <see cref="LocalDescription">local description</see>.
	/// </summary>
	/// <param name="description">
	/// A local session description init whose <c>type</c> may be omitted to let the implementation
	/// infer it from the current signaling state, or <see langword="null"/> to use the implementation's
	/// default (equivalent to passing an empty <c>RTCLocalSessionDescriptionInit</c>).
	/// </param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-setlocaldescription" />
	public abstract Task SetLocalDescription(RtcLocalSessionDescriptionInit? description = null);

	/// <summary>
	/// The SetRemoteDescription() method instructs the RTCPeerConnection to apply the supplied
	/// <see cref="RtcSessionDescription">RTCSessionDescription</see> as the remote offer or answer. This API changes the
	/// local media state.
	/// </summary>
	/// <param name="description">
	/// A session description containing the SDP describing the remote session as received from a
	/// peer.
	/// </param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-setremotedescription" />
	public abstract Task SetRemoteDescription(RtcSessionDescription description);

	/// <summary>
	/// The AddIceCandidate() method provides a remote <see cref="RtcIceCandidate">candidate</see> to the ICE Agent. In
	/// addition to being added to the <see cref="RemoteDescription">remote description</see>, connectivity checks will be
	/// sent to the new <see cref="RtcIceCandidate">candidates</see>. This call will result in a change to the connection
	/// state of the ICE Agent, and may result in a change to media state if it results in different connectivity being
	/// established.
	/// </summary>
	/// <param name="candidate">An ICE candidate to add, or null to signal end-of-candidates.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-addicecandidate" />
	public abstract Task AddIceCandidate(RtcIceCandidate? candidate = null);

	/// <summary>
	/// The restartIce method tells the RTCPeerConnection that ICE should be restarted. Subsequent calls to
	/// <see cref="CreateOffer" /> will create <see cref="RtcSessionDescription">descriptions</see> that will restart ICE,
	/// as described in section
	/// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-9.1.1.1">9.1.1.1 of [RFC5245]</see>.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-restartice" />
	public abstract void RestartIce();

	/// <summary>
	/// Destroys the RtcPeerConnection ICE Agent, abruptly ending any active ICE processing and any active streaming, and
	/// releasing any relevant resources (e.g. TURN permissions). Sets the <see cref="IceConnectionState" /> and
	/// <see cref="ConnectionState " /> to Closed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-close" />
	public abstract void Close();

	/// <summary>
	/// Session negotiation needs to be done at some point in the near future.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onnegotiationneeded" />
	public abstract event EventHandler OnNegotiationNeeded;

	/// <summary>
	/// A new RtcIceCandidate is made available.
	/// </summary>
	public abstract event EventHandler<RtcIceCandidateEventArgs> OnIceCandidate;

	/// <summary>
	/// An ICE candidate gathering error occurred. Event payload aligns with
	/// <c>RTCPeerConnectionIceErrorEvent</c> via <see cref="RtcIceCandidateErrorEventArgs" />.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onicecandidateerror" />
	public abstract event EventHandler<RtcIceCandidateErrorEventArgs> OnIceCandidateError;

	/// <summary>
	/// The RtcPeerConnection <see cref="SignalingState" /> has changed.
	/// This state change is the result of either setLocalDescription() or setRemoteDescription() being invoked.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onsignalingstatechange" />
	public abstract event EventHandler OnSignalingStateChange;

	/// <summary>
	/// The RtcPeerConnection <see cref="IceConnectionState" /> state has changed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-oniceconnectionstatechange" />
	public abstract event EventHandler OnIceConnectionStateChange;

	/// <summary>
	/// The RtcPeerConnection <see cref="IceGatheringState" /> has changed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onicegatheringstatechange" />
	public abstract event EventHandler OnGatheringStateChange;

	/// <summary>
	/// The RtcPeerConnection <see cref="ConnectionState" /> has changed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onconnectionstatechange" />
	public abstract event EventHandler OnConnectionStateChange;

	#region 8 Statistics Model

	/// <summary>
	/// Gathers stats for the given selector and reports the result asynchronously.
	/// </summary>
	/// <param name="selector">An <seealso cref="MediaStreamTrack" /> for which to generate a stats report.</param>
	/// <seealso
	///     href="https://www.w3.org/TR/webrtc/#widl-RTCPeerConnection-getStats-Promise-RTCStatsReport--MediaStreamTrack-selector" />
	public abstract Task<RtcStatsReport> GetStats(MediaStreamTrack? selector = null);

	#endregion


	#region 5 RTP Media API

	/// <summary>
	/// Returns a sequence of <see cref="RtcRtpSender">RTCRtpSender</see> objects representing the RTP senders that belong
	/// to non-stopped <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see> objects currently attached to this
	/// RTCPeerConnection object.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-getsenders" />
	public abstract IEnumerable<RtcRtpSender> GetSenders();

	/// <summary>
	/// Returns a sequence of <see cref="RtcRtpReceiver">RTCRtpReceiver</see> objects representing the RTP receivers that
	/// belong to non-stopped <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see> objects currently attached to this
	/// RTCPeerConnection object.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-getreceivers" />
	public abstract IEnumerable<RtcRtpReceiver> GetReceivers();

	/// <summary>
	/// Returns a sequence of <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see> objects representing the RTP
	/// transceivers that are currently attached to this RTCPeerConnection object.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-gettranseceivers" />
	public abstract IEnumerable<RtcRtpTransceiver> GetTransceivers();

	/// <summary>
	/// Adds a new <see cref="MediaStreamTrack">track</see> to the RTCPeerConnection, and indicates that it is contained
	/// in the specified <see cref="MediaStream">MediaStreams</see>. If an <see cref="RtcRtpSender">RTCRtpSender</see>
	/// for track already exists in <see cref="GetSenders">senders</see>, throw an <see cref="InvalidOperationException" />
	/// .
	/// </summary>
	/// <returns>
	/// A newly created <see cref="RtcRtpSender" /> created from the <see cref="MediaStreamTrack">track</see> and
	/// <see cref="MediaStream">streams</see>.
	/// </returns>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-addtrack" />
	public abstract RtcRtpSender AddTrack(MediaStreamTrack track, params MediaStream[] streams);

	/// <summary>
	/// Adds a new <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see> to this
	/// <see cref="RtcPeerConnection">RTCPeerConnection</see> from an existing media <paramref name="track" />.
	/// </summary>
	/// <param name="track">The media track used to create the transceiver.</param>
	/// <param name="init">Optional initialization settings for the created transceiver.</param>
	/// <returns>The created <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see>.</returns>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-addtransceiver" />
	public abstract RtcRtpTransceiver AddTransceiver(MediaStreamTrack track, RtcRtpTransceiverInit? init = null);

	/// <summary>
	/// Adds a new <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see> to this
	/// <see cref="RtcPeerConnection">RTCPeerConnection</see> for the specified media <paramref name="kind" />.
	/// </summary>
	/// <param name="kind">The media kind used to create the transceiver.</param>
	/// <param name="init">Optional initialization settings for the created transceiver.</param>
	/// <returns>The created <see cref="RtcRtpTransceiver">RTCRtpTransceiver</see>.</returns>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-addtransceiver" />
	public abstract RtcRtpTransceiver AddTransceiver(MediaStreamTrackKind kind, RtcRtpTransceiverInit? init = null);

	/// <summary>
	/// Stops sending media from sender. The <see cref="RtcRtpSender">RTCRtpSender</see> will still appear in
	/// <see cref="GetSenders" />. Doing so will cause future calls to <see cref="CreateOffer" /> to mark the media
	/// description for the corresponding <see cref="RtcRtpTransceiver">transceiver</see> as
	/// <see cref="RtcRtpTransceiverDirection.RecvOnly" /> or <see cref="RtcRtpTransceiverDirection.Inactive" />, as
	/// defined in <see href="https://tools.ietf.org/html/rfc8829#section-5.2.2">[RFC8829] (section 5.2.2.)</see>.
	/// </summary>
	/// <seealso href="https://tools.ietf.org/html/rfc8829#section-5.2.2" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-removetrack" />
	/// <param name="sender">The sender to stop sending media for.</param>
	public abstract void RemoveTrack(RtcRtpSender sender);

	/// <summary>
	/// New incoming media has been negotiated for a specific RTCRtpReceiver, and that receiver's track has been added to
	/// any associated remote MediaStreams.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-ontrack" />
	public abstract event EventHandler<RtcTrackEventArgs> OnTrack;

	#endregion 5 RTP Media API


	#region 6 Peer-to-peer Data API

	/// <summary>
	/// Creates a new RtcDataChannel object with the given label.
	/// The RtcDataChannelInit object can be used to configure properties of the underlying channel such as data
	/// reliability.
	/// </summary>
	/// <param name="label">The new channel's label is set to this value.</param>
	/// <param name="dataChannelInit">Optional parameters with wich to initialize the new data channel.</param>
	/// <returns></returns>
	public abstract RtcDataChannel CreateDataChannel(string label, RtcDataChannelInit? dataChannelInit = null);

	/// <summary>
	/// Fired when a data channel is created by the peer.
	/// </summary>
	public abstract event EventHandler<RtcDataChannelEventArgs> OnDataChannel;

	#endregion
}

/// <summary>
/// Event data containing a media stream.
/// </summary>
public class MediaStreamEventArgs : EventArgs
{
	/// <summary>
	/// Initializes media stream event data.
	/// </summary>
	/// <param name="stream">The associated media stream.</param>
	public MediaStreamEventArgs(MediaStream stream)
	{
		Stream = stream;
	}

	/// <summary>
	/// Gets the associated media stream.
	/// </summary>
	public MediaStream Stream { get; }
}

/// <summary>
/// Event data containing an ICE candidate.
/// </summary>
public class RtcIceCandidateEventArgs : EventArgs
{
	/// <summary>
	/// Initializes ICE candidate event data.
	/// </summary>
	/// <param name="candidate">The candidate associated with the event.</param>
	/// <param name="url">The ICE server URL used to gather the candidate, if available.</param>
	public RtcIceCandidateEventArgs(RtcIceCandidate candidate, string? url = null)
	{
		Candidate = candidate;
		Url = url;
	}

	/// <summary>
	/// Gets the ICE candidate associated with the event.
	/// </summary>
	public RtcIceCandidate Candidate { get; }

	/// <summary>
	/// Gets the ICE server URL used to gather the candidate, if available.
	/// </summary>
	public string? Url { get; }
}

/// <summary>
/// Event data describing an ICE candidate error.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcpeerconnectioniceerrorevent" />
public class RtcIceCandidateErrorEventArgs : EventArgs
{
	/// <summary>
	/// Initializes ICE candidate error event data with payload fields aligned to
	/// <c>RTCPeerConnectionIceErrorEvent</c>.
	/// </summary>
	/// <param name="address">The local address used when attempting ICE communication, if available.</param>
	/// <param name="port">The local port used when attempting ICE communication, if available.</param>
	/// <param name="url">The URL of the ICE server that produced the error.</param>
	/// <param name="errorCode">The ICE error code.</param>
	/// <param name="errorText">The ICE error text.</param>
	public RtcIceCandidateErrorEventArgs(string? address, ushort? port, string url, ushort errorCode, string errorText)
	{
		Address = address;
		Port = port;
		Url = url;
		ErrorCode = errorCode;
		ErrorText = errorText;
	}

	/// <summary>
	/// Gets the local address used for ICE communication, if available.
	/// </summary>
	public string? Address { get; }

	/// <summary>
	/// Gets the local port used for ICE communication, if available.
	/// </summary>
	public ushort? Port { get; }

	/// <summary>
	/// Gets the URL of the ICE server that produced the error.
	/// </summary>
	public string Url { get; }

	/// <summary>
	/// Gets the ICE error code.
	/// </summary>
	public ushort ErrorCode { get; }

	/// <summary>
	/// Gets the ICE error text.
	/// </summary>
	public string ErrorText { get; }
}

/// <summary>
/// Event data containing a data channel.
/// </summary>
public class RtcDataChannelEventArgs : EventArgs
{
	/// <summary>
	/// Initializes data channel event data.
	/// </summary>
	/// <param name="channel">The data channel associated with the event.</param>
	public RtcDataChannelEventArgs(RtcDataChannel channel)
	{
		Channel = channel;
	}

	/// <summary>
	/// Gets the data channel associated with the event.
	/// </summary>
	public RtcDataChannel Channel { get; }
}

/// <summary>
/// Event data for incoming remote track events.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#event-track" />
public class RtcTrackEventArgs : EventArgs
{
	/// <summary>
	/// Initializes track event data.
	/// </summary>
	/// <param name="type">The media type associated with the event.</param>
	/// <param name="receiver">The receiver for the remote track.</param>
	/// <param name="track">The remote media track.</param>
	/// <param name="streams">The remote media streams associated with the track.</param>
	/// <param name="transceiver">The RTP transceiver associated with the track.</param>
	public RtcTrackEventArgs(string type, RtcRtpReceiver receiver, MediaStreamTrack track,
		IEnumerable<MediaStream> streams, RtcRtpTransceiver transceiver)
	{
		Type = type;
		Receiver = receiver;
		Track = track;
		Streams = streams.ToList();
		Transceiver = transceiver;
	}

	/// <summary>
	/// Gets the media type associated with the event.
	/// </summary>
	public string Type { get; }

	/// <summary>
	/// Gets the RTP receiver associated with the track.
	/// </summary>
	public RtcRtpReceiver Receiver { get; }

	/// <summary>
	/// Gets the remote media track associated with the event.
	/// </summary>
	public MediaStreamTrack Track { get; }

	/// <summary>
	/// Gets the remote media streams associated with the track.
	/// </summary>
	public IReadOnlyList<MediaStream> Streams { get; }

	/// <summary>
	/// Gets the transceiver associated with the event.
	/// </summary>
	public RtcRtpTransceiver Transceiver { get; }
}

/// <summary>
/// Represents a failure to create an SDP offer or answer.
/// </summary>
public class CreateSessionDescriptionFailure : Exception
{
	/// <summary>
	/// Initializes a new instance of the exception with an error message.
	/// </summary>
	/// <param name="message">The failure message.</param>
	public CreateSessionDescriptionFailure(string message)
		: base(message)
	{
	}
}