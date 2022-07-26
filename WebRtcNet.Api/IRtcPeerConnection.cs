using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRtcNet.Media;

namespace WebRtcNet
{
    /// <summary>
    ///     Describes the possible states of the PeerConnection connection. Note this is currently identical to
    ///     <see cref="RtcIceTransportState" />.
    /// </summary>
    /// <seealso cref="IRtcPeerConnection.ConnectionState" />
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtciceconnectionstate-enum" />
    public enum RtcIceConnectionState
    {
        /// <summary>
        ///     The ICE Agent is gathering addresses and / or waiting for remote candidates to be supplied.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-new" />
        New,

        /// <summary>
        ///     The ICE Agent has received remote candidates on at least one component, and is checking candidate pairs but has not
        ///     yet found a connection. In addition to checking, it may also still be gathering.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-checking" />
        Checking,

        /// <summary>
        ///     The ICE Agent has found a usable connection for all components but is still checking other candidate pairs to see
        ///     if there is a better connection.It may also still be gathering.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-connected" />
        Connected,

        /// <summary>
        ///     The ICE Agent has finished gathering and checking and found a connection for all components.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-completed" />
        Completed,

        /// <summary>
        ///     The ICE Agent is finished checking all candidate pairs and failed to find a connection for at least one component.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-failed" />
        Failed,

        /// <summary>
        ///     Liveness checks have failed for one or more components. This is more aggressive than failed, and may trigger
        ///     intermittently(and resolve itself without action) on a flaky network.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-disconnected" />
        Disconnected,

        /// <summary>
        ///     The ICE Agent has shut down and is no longer responding to STUN requests.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceconnectionstate-closed" />
        Closed
    }

    /// <summary>
    /// </summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#idl-def-RTCSignalingState" />
    public enum RtcSignalingState
    {
        /// <summary>
        ///     There is no offer­/answer exchange in progress. This is also the initial state in which case the local and remote
        ///     <see cref="RtcSessionDescription">descriptions</see> are empty.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-stable" />
        Stable,

        /// <summary>
        ///     A local description, of type <see cref="RtcSdpType.Offer">offer</see>, has been successfully applied.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-local-offer" />
        HaveLocalOffer,

        /// <summary>
        ///     A remote description, of type <see cref="RtcSdpType.Offer">offer</see>, has been successfully applied.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-remote-offer" />
        HaveRemoteOffer,

        /// <summary>
        ///     A remote description of type <see cref="RtcSdpType.Offer">offer</see> has been successfully applied and a local
        ///     description of type <see cref="RtcSdpType.PrAnswer">pranswer</see> has been successfully applied
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-local-pranswer" />
        HaveLocalPrAnswer,

        /// <summary>
        ///     A local description of type <see cref="RtcSdpType.Offer">offer</see> has been successfully applied and a remote
        ///     description of type <see cref="RtcSdpType.PrAnswer">pranswer</see> has been successfully applied.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-have-remote-pranswer" />
        HaveRemotePrAnswer,

        /// <summary>
        ///     The connection is closed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcsignalingstate-closed" />
        Closed
    }

    /// <summary>
    ///     Represents the possible states of the RtcPeerConnection
    /// </summary>
    /// <see cref="IRtcPeerConnection.ConnectionState" />
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcpeerconnectionstate-enum" />
    public enum RtcPeerConnectionState
    {
        /// <summary>
        ///     The <see cref="IRtcPeerConnection">RtcPeerConnection</see> is closed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-closed" />
        Closed,

        /// <summary>
        ///     The previous state doesn't apply and any <see cref="IRtcIceTransport">RTCIceTransports</see> are in the
        ///     <see cref="RtcIceTransportState.Failed">failed</see> state or any
        ///     <see cref="IRtcDtlsTransport">RTCDtlsTransports</see> are in the
        ///     <see cref="RtcIceTransportState.Failed">failed</see> state.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-failed" />
        Failed,

        /// <summary>
        ///     None of the previous states apply and any <see cref="IRtcIceTransport">RTCIceTransports</see> are in the
        ///     <see cref="RtcIceTransportState.Disconnected">disconnected</see> state.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-disconnected" />
        Disconnected,

        /// <summary>
        ///     None of the previous states apply and all <see cref="IRtcIceTransport">RTCIceTransports</see> are in the
        ///     <see cref="RtcIceTransportState.New">new</see> or <see cref="RtcIceTransportState.Closed">closed</see> state, and
        ///     all <seealso cref="IRtcDtlsTransport">RTCDtlsTransports</seealso> are in the
        ///     <see cref="RtcDtlsTransportState.New">new</see> or <see cref="RtcDtlsTransportState.Closed">closed</see> state, or
        ///     there are no transports.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-new" />
        New,

        /// <summary>
        ///     None of the previous states apply and any <see cref="IRtcIceTransport">RTCIceTransports</see> is in the
        ///     <see cref="RtcIceTransportState.Checking">checking</see> state or any
        ///     <see cref="IRtcDtlsTransport">RTCDtlsIceTransport</see> is in the
        ///     <see cref="RtcDtlsTransportState.Connecting">connecting</see> state.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-connecting" />
        Connecting,

        /// <summary>
        ///     None of the previous states apply and all <see cref="IRtcIceTransport">RTCIceTransports</see> are in the
        ///     <see cref="RtcIceTransportState.Connected">connected</see>,
        ///     <see cref="RtcIceTransportState.Completed">completed</see> or <see cref="RtcIceTransportState.Closed">closed</see>
        ///     state, and all <see cref="IRtcIceTransport">RTCDtlsTransports</see> are in the
        ///     <see cref="RtcDtlsTransportState.Connected">connected</see> or
        ///     <see cref="RtcDtlsTransportState.Closed">closed</see> state.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnectionstate-connected" />
        Connected
    }


    /// <Summary>
    ///     A .Net implementation of the WebRTC RTCPeerConnection Interface (W3C Recommendation 26 January 2021)
    /// </Summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcpeerconnection-interface" />
    /// <seealso href="https://www.w3.org/TR/webrtc/#interface-definition" />
    public interface IRtcPeerConnection
    {
        /// <summary>
        ///     The local <see cref="RtcSessionDescription">RTCSessionDescription</see> that was successfully set using
        ///     <see cref="SetLocalDescription" />, plus any local <see cref="IRtcIceCandidate">candidates</see> that have been
        ///     generated by the ICE Agent since then.  A null object will be returned if the local description has not yet been
        ///     set.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-localdescription" />
        RtcSessionDescription? LocalDescription { get; }

        /// <summary>
        ///     Represents the local description that was successfully negotiated the last time the RTCPeerConnection transitioned
        ///     into the stable state plus any local <see cref="IRtcIceCandidate">candidates</see> that have been generated by the
        ///     ICE Agent since the offer or answer was created.
        /// </summary>
        /// <seealso cref="CreateOffer" />
        /// <seealso cref="CreateAnswer" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-currentlocaldesc" />
        RtcSessionDescription? CurrentLocalDescription { get; }

        /// <summary>
        ///     Represents a local <see cref="RtcSessionDescription">description</see> that is in the process of being negotiated
        ///     plus any local candidates that have been generated by the ICE Agent since the <see cref="CreateOffer">offer</see>
        ///     or <see cref="CreateAnswer">answer</see> was created. If the RTCPeerConnection is in the stable state, the value is
        ///     null.
        /// </summary>
        /// <seealso cref="CreateOffer" />
        /// <seealso cref="CreateAnswer" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-pendinglocaldesc" />
        RtcSessionDescription? PendingLocalDescription { get; }

        /// <summary>
        ///     The <see cref="RtcSessionDescription">RemoteDescription</see> that was successfully set using
        ///     <see cref="SetRemoteDescription" />, plus any remote candidates that have been supplied via
        ///     <see cref="AddIceCandidate" /> since then. A null object will be returned if the remote description has not yet
        ///     been set.
        /// </summary>
        /// <seealso cref="SetRemoteDescription" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-remotedescription" />
        RtcSessionDescription? RemoteDescription { get; }

        /// <summary>
        ///     It represents the last remote <see cref="RtcSessionDescription">description</see> that was successfully negotiated
        ///     the last time the RTCPeerConnection transitioned into the stable state plus any remote candidates that have been
        ///     supplied via <see cref="AddIceCandidate" /> since the offer or answer was created.
        /// </summary>
        /// <seealso cref="CreateOffer" />
        /// <seealso cref="CreateAnswer" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-currentremotedesc" />
        RtcSessionDescription? CurrentRemoteDescription { get; }

        /// <summary>
        ///     It represents a remote description that is in the process of being negotiated, complete with any remote
        ///     <see cref="IRtcIceCandidate">candidates</see> that have been supplied via <see cref="AddIceCandidate" /> since the
        ///     offer or answer was created. If the RTCPeerConnection is in the stable state, the value is null.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-pendingremotedesc" />
        RtcSessionDescription? PendingRemoteDescription { get; }

        /// <summary>
        ///     The signaling state of the RtcPeerConnection.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-signaling-state" />
        RtcSignalingState SignalingState { get; }

        /// <summary>
        ///     The gathering state of the RtcPeerConnection ICE Agent.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-ice-gathering-state" />
        RtcIceGatheringState IceGatheringState { get; }

        /// <summary>
        ///     The ICE connection state of the RtcPeerConnection ICE Agent.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-ice-connection-state" />
        RtcIceConnectionState IceConnectionState { get; }

        /// <summary>
        ///     The current connection state of the RtcPeerConnection and its transports.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-connection-state" />
        RtcPeerConnectionState ConnectionState { get; }

        /// <summary>
        ///     Dictates whether the remote peer is able to accept trickled ICE candidates
        ///     <see href="https://www.w3.org/TR/webrtc/#bib-rfc8829">[RFC8838]</see>. The value is determined based on whether a
        ///     remote description indicates support for trickle ICE, as defined in
        ///     <see href="https://tools.ietf.org/html/rfc8829#section-4.1.17">[RFC8829] (section 4.1.17.)</see>. Prior to the
        ///     completion of setRemoteDescription, this value is null.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-cantrickleicecandidates" />
        bool? CanTrickleIceCandidates { get; }

        /// <summary>
        ///     Gets or sets the <see cref="RtcConfiguration" /> object representing the current configuration of this
        ///     RtcPeerConnection object. Setting the configuration updates the ICE Agent process of gathering local
        ///     <see cref="IRtcIceCandidate">candidates</see> and pinging remote <see cref="IRtcIceCandidate">candidates</see>.
        ///     This call may result in a change to the state of the ICE Agent, and may result in a change to media state if it
        ///     results in connectivity being established.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-getconfiguration" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-setconfiguration" />
        RtcConfiguration Configuration { get; set; }

        /// <summary>
        ///     The CreateOffer method generates a <see cref="RtcSessionDescription" />
        ///     <see href="http://tools.ietf.org/html/rfc3264">[SDP]</see> that contains an
        ///     <see href="https://tools.ietf.org/html/rfc3264">RFC 3264</see> offer with the supported configurations for the
        ///     session, including <see cref="RtcSessionDescription">descriptions</see> of the local
        ///     <see cref="IMediaStreamTrack">MediaStreamTracks</see> attached to this RTCPeerConnection, the codec/RTP/RTCP
        ///     options supported by this implementation, and any <see cref="IRtcIceCandidate">candidates</see> that have been
        ///     gathered by the ICE Agent. The <see cref="RtcOfferOptions">options</see> parameter may be supplied to provide
        ///     additional control over the offer generated.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-createoffer" />
        /// <exception cref="CreateSessionDescriptionFailure" />
        Task<RtcSessionDescription> CreateOffer(RtcOfferOptions options = null);

        /// <summary>
        ///     Generates a <see cref="RtcSessionDescription" /> <see href="http://tools.ietf.org/html/rfc3264">[SDP]</see> answer
        ///     with the supported configuration for the session that is compatible with the parameters in the remote
        ///     configuration. Like <see cref="CreateOffer" />, the returned <see cref="RtcSessionDescription" /> contains
        ///     <see cref="RtcSessionDescription">descriptions</see> of the local <see cref="IMediaStream">MediaStreams</see>
        ///     attached to this RTCPeerConnection, the codec/RTP/RTCP options negotiated for this session, and any
        ///     <see cref="IRtcIceCandidate">candidates</see> that have been gathered by the ICE Agent. The
        ///     <see cref="RtcAnswerOptions">options</see> parameter may be supplied to provide additional control over the
        ///     generated answer.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-createanswer" />
        /// <exception cref="CreateSessionDescriptionFailure" />
        Task<RtcSessionDescription> CreateAnswer(RtcAnswerOptions options = null);

        /// <summary>
        ///     The SetLocalDescription() method instructs the RtcPeerConnection to apply the supplied
        ///     <see cref="RtcSessionDescription">RtcSessionDescription</see> as the
        ///     <see cref="LocalDescription">local description</see>.
        /// </summary>
        /// <param name="description">A session description containing the SDP describing the local session.</param>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-setlocaldescription" />
        Task SetLocalDescription(RtcSessionDescription description);

        /// <summary>
        ///     The SetRemoteDescription() method instructs the RTCPeerConnection to apply the supplied
        ///     <see cref="RtcSessionDescription">RTCSessionDescription</see> as the remote offer or answer. This API changes the
        ///     local media state.
        /// </summary>
        /// <param name="description">
        ///     A session description containing the SDP describing the remote session as received from a
        ///     peer.
        /// </param>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-setremotedescription" />
        Task SetRemoteDescription(RtcSessionDescription description);

        /// <summary>
        ///     The AddIceCandidate() method provides a remote <see cref="IRtcIceCandidate">candidate</see> to the ICE Agent. In
        ///     addition to being added to the <see cref="RemoteDescription">remote description</see>, connectivity checks will be
        ///     sent to the new <see cref="IRtcIceCandidate">candidates</see> as long as the
        ///     <see cref="RtcConfiguration.IceTransportPolicy">ICE Transports setting</see> is not set to
        ///     <see cref="RtcIceTransportPolicy.None" />. This call will result in a change to the connection state of the ICE
        ///     Agent, and may result in a change to media state if it results in different connectivity being established.
        /// </summary>
        /// <param name="candidate">An ICE candidate to add.</param>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-addicecandidate" />
        Task AddIceCandidate(IRtcIceCandidate candidate);

        /// <summary>
        ///     The restartIce method tells the RTCPeerConnection that ICE should be restarted. Subsequent calls to
        ///     <see cref="CreateOffer" /> will create <see cref="RtcSessionDescription">descriptions</see> that will restart ICE,
        ///     as described in section
        ///     <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-9.1.1.1">9.1.1.1 of [RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-restartice" />
        void RestartIce();

        /// <summary>
        ///     Destroys the RtcPeerConnection ICE Agent, abruptly ending any active ICE processing and any active streaming, and
        ///     releasing any relevant resources (e.g. TURN permissions). Sets the <see cref="IceConnectionState" /> and
        ///     <see cref="ConnectionState " /> to Closed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-close" />
        void Close();

        /// <summary>
        ///     Session negotiation needs to be done at some point in the near future.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onnegotiationneeded" />
        event EventHandler OnNegotiationNeeded;

        /// <summary>
        ///     A new RtcIceCandidate is made available.
        /// </summary>
        event EventHandler<RtcIceCandidateEventArgs> OnIceCandidate;

        /// <summary>
        ///     An error occurred creating an IceCandidate.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onicecandidateerror" />
        event EventHandler<RtcIceCandidateErrorEventArgs> OnIceCandidateError;

        /// <summary>
        ///     The RtcPeerConnection <see cref="SignalingState" /> has changed.
        ///     This state change is the result of either setLocalDescription() or setRemoteDescription() being invoked.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onsignalingstatechange" />
        event EventHandler OnSignalingStateChange;

        /// <summary>
        ///     The RtcPeerConnection <see cref="IceConnectionState" /> state has changed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-oniceconnectionstatechange" />
        event EventHandler OnIceConnectionStateChange;

        /// <summary>
        ///     The RtcPeerConnection <see cref="IceGatheringState" /> has changed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onicegatheringstatechange" />
        event EventHandler OnGatheringStateChange;

        /// <summary>
        ///     The RtcPeerConnection <see cref="ConnectionState" /> has changed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-onconnectionstatechange" />
        event EventHandler OnConnectionStateChange;

        #region 8 Statistics Model

        /// <summary>
        ///     Gathers stats for the given selector and reports the result asynchronously.
        /// </summary>
        /// <param name="selector">An <seealso cref="IMediaStreamTrack" /> for which to generate a stats report.</param>
        /// <seealso
        ///     href="https://www.w3.org/TR/webrtc/#widl-RTCPeerConnection-getStats-Promise-RTCStatsReport--MediaStreamTrack-selector" />
        Task<IRtcStatsReport> GetStats(IMediaStreamTrack selector = null);

        #endregion


        #region 5 RTP Media API

        /// <summary>
        ///     Returns a sequence of <see cref="IRtcRtpSender">RTCRtpSender</see> objects representing the RTP senders that belong
        ///     to non-stopped <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see> objects currently attached to this
        ///     RTCPeerConnection object.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-getsenders" />
        IEnumerable<IRtcRtpSender> GetSenders();

        /// <summary>
        ///     Returns a sequence of <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> objects representing the RTP receivers that
        ///     belong to non-stopped <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see> objects currently attached to this
        ///     RTCPeerConnection object.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-getreceivers" />
        IEnumerable<IRtcRtpReceiver> GetReceivers();

        /// <summary>
        ///     Returns a sequence of <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see> objects representing the RTP
        ///     transceivers that are currently attached to this RTCPeerConnection object.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-peerconnection-gettranseceivers" />
        IEnumerable<IRtcRtpTransceiver> GetTransceivers();

        /// <summary>
        ///     Adds a new <see cref="IMediaStreamTrack">track</see> to the RTCPeerConnection, and indicates that it is contained
        ///     in the specified <see cref="IMediaStream">MediaStreams</see>. If an <see cref="IRtcRtpSender">RTCRtpSender</see>
        ///     for track already exists in <see cref="GetSenders">senders</see>, throw an <see cref="InvalidOperationException" />
        ///     .
        /// </summary>
        /// <returns>
        ///     A newly created <see cref="IRtcRtpSender" /> created from the <see cref="IMediaStreamTrack">track</see> and
        ///     <see cref="IMediaStream">streams</see>.
        /// </returns>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-addtrack" />
        IRtcRtpSender AddTrack(IMediaStreamTrack track, params IMediaStream[] streams);

        /// <summary>
        ///     Stops sending media from sender. The <see cref="IRtcRtpSender">RTCRtpSender</see> will still appear in
        ///     <see cref="GetSenders" />. Doing so will cause future calls to <see cref="CreateOffer" /> to mark the media
        ///     description for the corresponding <see cref="IRtcRtpTransceiver">transceiver</see> as
        ///     <see cref="RtcRtpTransceiverDirection.RecvOnly" /> or <see cref="RtcRtpTransceiverDirection.Inactive" />, as
        ///     defined in <see href="https://tools.ietf.org/html/rfc8829#section-5.2.2">[RFC8829] (section 5.2.2.)</see>.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc8829#section-5.2.2" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-removetrack" />
        /// <param name="track">The track to be removed</param>
        void RemoveTrack(IMediaStreamTrack track);

        /// <summary>
        ///     Create a new <see cref="IRtcRtpTransceiver" /> with the specified <see cref="IMediaStreamTrack" /> and add it to
        ///     the set of transceivers. Adding a <see cref="IRtcRtpTransceiver">transceiver</see> will cause future calls to
        ///     <see cref="CreateOffer" /> to add a media description for the corresponding
        ///     <see cref="IRtcRtpReceiver">transceiver</see>, as defined in
        ///     <see href="https://tools.ietf.org/html/rfc8829#section-5.2.2">[RFC8829] (section 5.2.2.)</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-addtransceiver" />
        /// <seealso href="https://tools.ietf.org/html/rfc8829#section-5.2.2" />
        IRtcRtpTransceiver AddTransceiver(IMediaStreamTrack track, IRtcRtpTransceiver transceiver);

        /// <summary>
        ///     Create a new <see cref="IRtcRtpTransceiver" /> of the specified <see cref="MediaStreamTrackKind" /> and add it to
        ///     the set of transceivers. Adding a <see cref="IRtcRtpTransceiver">transceiver</see> will cause future calls to
        ///     <see cref="CreateOffer" /> to add a media description for the corresponding
        ///     <see cref="IRtcRtpReceiver">transceiver</see>, as defined in
        ///     <see href="https://tools.ietf.org/html/rfc8829#section-5.2.2">[RFC8829] (section 5.2.2.)</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-addtransceiver" />
        /// <seealso href="https://tools.ietf.org/html/rfc8829#section-5.2.2" />
        IRtcRtpTransceiver AddTransceiver(MediaStreamTrackKind kind, IRtcRtpTransceiver transceiver);

        /// <summary>
        ///     New incoming media has been negotiated for a specific RTCRtpReceiver, and that receiver's track has been added to
        ///     any associated remote MediaStreams.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcpeerconnection-ontrack" />
        event EventHandler<RtcTrackEventArgs> OnTrack;

        #endregion 5 RTP Media API


        #region 6 Peer-to-peer Data API

        /// <summary>
        ///     Creates a new IRtcDataChannel object with the given label.
        ///     The RtcDataChannelInit object can be used to configure properties of the underlying channel such as data
        ///     reliability.
        /// </summary>
        /// <param name="label">The new channel's label is set to this value.</param>
        /// <param name="dataChannelInit">Optional parameters with wich to initialize the new data channel.</param>
        /// <returns></returns>
        IRtcDataChannel CreateDataChannel(string label, RtcDataChannelInit dataChannelInit = null);

        /// <summary>
        ///     Fired when a data channel is created by the peer.
        /// </summary>
        event EventHandler<RtcDataChannelEventArgs> OnDataChannel;

        #endregion
    }

    public class MediaStreamEventArgs : EventArgs
    {
        public MediaStreamEventArgs(IMediaStream stream)
        {
            Stream = stream;
        }

        public IMediaStream Stream { get; }
    }

    public class RtcIceCandidateEventArgs : EventArgs
    {
        public RtcIceCandidateEventArgs(IRtcIceCandidate candidate)
        {
            Candidate = candidate;
        }

        public IRtcIceCandidate Candidate { get; }
    }

    public class RtcIceCandidateErrorEventArgs : EventArgs
    {
        public RtcIceCandidateErrorEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class RtcDataChannelEventArgs : EventArgs
    {
        public RtcDataChannelEventArgs(IRtcDataChannel channel)
        {
            Channel = channel;
        }

        public IRtcDataChannel Channel { get; }
    }

    public class RtcTrackEventArgs : EventArgs
    {
        public RtcTrackEventArgs(string type, IRtcRtpReceiver receiver, IMediaStreamTrack track,
            IEnumerable<IMediaStream> streams, IRtcRtpTransceiver transceiver)
        {
            Type = type;
            Receiver = receiver;
            Track = track;
            Streams = streams.ToList();
            Transceiver = transceiver;
        }

        public string Type { get; }

        public IRtcRtpReceiver Receiver { get; }

        public IMediaStreamTrack Track { get; }
        public IReadOnlyList<IMediaStream> Streams { get; }

        public IRtcRtpTransceiver Transceiver { get; }
    }

    public class CreateSessionDescriptionFailure : Exception
    {
        public CreateSessionDescriptionFailure(string message)
            : base(message)
        {
        }
    }
}