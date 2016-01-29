using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebRtcNet
{
    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcicegatheringstate-enum"/>
    /// </summary>
    public enum RtcGatheringState
    {
        /// <summary>
        /// The object was just created, and no networking has occurred yet.
        /// </summary>
        New,

        /// <summary>
        /// The ICE engine is in the process of gathering candidates for this RTCPeerConnection.
        /// </summary>
        Gathering,

        /// <summary>
        /// The ICE engine has completed gathering.
        /// </summary>
        Complete
    };

    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtciceconnectionstate-enum"/>
    /// </summary>
    public enum RtcIceConnectionState
    {
        /// <summary>
        /// The ICE Agent is gathering addresses and / or waiting 
        /// for remote candidates to be supplied.
        /// </summary>
        New,

        /// <summary>
        /// The ICE Agent has received remote candidates on at least one component, 
        /// and is checking candidate pairs but has not yet found a connection.
        /// In addition to checking, it may also still be gathering.
        /// </summary>
        Checking,

        /// <summary>
        /// The ICE Agent has found a usable connection for all components but is 
        /// still checking other candidate pairs to see if there is a better 
        /// connection.It may also still be gathering.
        /// </summary>
        Connected,

        /// <summary>
        /// The ICE Agent has finished gathering and checking and found a connection for all components.
        /// </summary>
        Completed,

        /// <summary>
        /// The ICE Agent is finished checking all candidate pairs and failed to find a connection for at least one component.
        /// </summary>
        Failed,

        /// <summary>
        /// Liveness checks have failed for one or more components.
        /// This is more aggressive than failed, and may trigger 
        /// intermittently(and resolve itself without action) on a flaky network.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The ICE Agent has shut down and is no longer responding to STUN requests.
        /// </summary>
        Closed
    };

    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#idl-def-RTCSignalingState"/>
    /// </summary>
    public enum RtcSignalingState
    {
        /// <summary>
        /// There is no offer­answer exchange in progress. 
        /// This is also the initial state in which case the local and remote descriptions are empty.
        /// </summary>
        Stable,

        /// <summary>
        /// A local description, of type "offer", has been successfully applied.
        /// </summary>
        HaveLocalOffer,

        /// <summary>
        /// A remote description, of type "offer", has been successfully applied.
        /// </summary>
        HaveRemoteOffer,

        /// <summary>
        /// A remote description of type "offer" has been successfully applied and a 
        /// local description of type "pranswer" has been successfully applied
        /// </summary>
        HaveLocalPrAnswer,

        /// <summary>
        /// A local description of type "offer" has been successfully applied 
        /// and a remote description of type "pranswer" has been successfully applied.
        /// </summary>
        HaveRemotePrAnswer,

        /// <summary>
        /// The connection is closed.
        /// </summary>
        Closed
    };

    /// <Summary>
    /// A .Net implementation of the WebRTC RTCPeerConnection Interface (W3C Working Draft 10 February 2015)
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcpeerconnection-interface"/>
    /// <seealso href="http://datatracker.ietf.org/doc/draft-ietf-rtcweb-jsep/"/>
    /// </Summary>
    public interface IRtcPeerConnection
    {
        /// <summary>
        /// The createOffer method generates a blob of SDP that contains an RFC 3264 offer with the supported configurations for the session, including descriptions of the local 
        /// MediaStreamTracks attached to this RTCPeerConnection, the codec/RTP/RTCP options supported by this implementation, and any candidates that have been gathered by the ICE Agent.
        /// The options parameter may be supplied to provide additional control over the offer generated.
        ///
        /// As an offer, the generated SDP will contain the full set of capabilities supported by the session (as opposed to an answer, which will include only a specific negotiated subset to use); 
        /// for each SDP line, the generation of the SDP must follow the appropriate process for generating an offer. In the event createOffer is called after the session is established, createOffer 
        /// will generate an offer that is compatible with the current session, incorporating any changes that have been made to the session since the last complete offer-answer exchange, such as 
        /// addition or removal of tracks. If no changes have been made, the offer will include the capabilities of the current local description as well as any additional capabilities that could be 
        /// negotiated in an updated offer.
        /// 
        /// Session descriptions generated by createOffer must be immediately usable by setLocalDescription without causing an error as long as setLocalDescription is called reasonably soon.If a system 
        /// has limited resources(e.g.a finite number of decoders), createOffer needs to return an offer that reflects the current state of the system, so that setLocalDescription will succeed when 
        /// it attempts to acquire those resources.The session descriptions must remain usable by setLocalDescription without causing an error until at least the end of the fulfillment callback of the 
        /// returned promise. Calling this method is needed to get the ICE user name fragment and password.
        /// 
        /// The value for certificates in the RTCConfiguration for the RTCPeerConnection is used to produce a set of certificate fingerprints. These certificate fingerprints are used in the 
        /// construction of SDP and as input to requests for identity assertions.
        /// 
        /// If the RTCPeerConnection is configured to generate Identity assertions by calling setIdentityProvider, then the session description shall contain an appropriate assertion.If the identity 
        /// provider is unable to produce an identity assertion, the call to createOffer must be rejected with a DOMError that has a name of IdpError.
        /// 
        /// If this RTCPeerConnection object is closed before the SDP generation process completes, the user agent must suppress the result and not resolve or reject the returned promise.
        /// 
        /// If the SDP generation process completed successfully, the user agent must resolve the returned promise with a newly created RTCSessionDescription object, representing the generated offer.
        /// 
        /// If the SDP generation process failed for any other reason, the user agent must reject the returned promise with an DOMError object of type TBD as its argument.
        /// <seealso href="http://w3c.github.io/webrtc-pc/#widl-RTCPeerConnection-createOffer-Promise-RTCSessionDescription--RTCOfferOptions-options"/>
        /// </summary>
        Task<RtcSessionDescription> CreateOffer(RtcOfferOptions options = null);

        /// <summary>
        /// Generates an <seealso href="http://tools.ietf.org/html/rfc3264">[SDP]</seealso> answer with the supported configuration 
        /// for the session that is compatible with the parameters in the remote configuration. Like CreateOffer, the returned blob 
        /// contains descriptions of the local MediaStreams attached to this RTCPeerConnection, the codec/RTP/RTCP options negotiated 
        /// for this session, and any candidates that have been gathered by the ICE Agent. The options parameter may be supplied to 
        /// provide additional control over the generated answer.
        /// </summary>
        Task<RtcSessionDescription> CreateAnswer();

        /// <summary>
        /// The setLocalDescription() method instructs the RtcPeerConnection to apply the supplied RtcSessionDescription as the local description.
        /// </summary>
        /// <param name="description"></param>
        Task SetLocalDescription(RtcSessionDescription description);

        /// <summary>
        /// The local RTCSessionDescription that was successfully set using SetLocalDescription(), 
        /// plus any local candidates that have been generated by the ICE Agent since then.
        /// A null object will be returned if the local description has not yet been set.
        /// </summary>
        RtcSessionDescription LocalDescription { get; }

        /// <summary>
        /// The setRemoteDescription() method instructs the RTCPeerConnection to apply the supplied RTCSessionDescription as the remote 
        /// offer or answer. This API changes the local media state.
        /// </summary>
        /// <param name="description"></param>
        Task SetRemoteDescription(RtcSessionDescription description);

        /// <summary>
        /// The RemoteDescription that was successfully set using SetRemoteDescription(), 
        /// plus any remote candidates that have been supplied via AddIceCandidate() since then.
        /// A null object will be returned if the remote description has not yet been set.
        /// </summary>
        RtcSessionDescription RemoteDescription { get; }

        /// <summary>
        /// The signaling state of the RtcPeerConnection.
        /// </summary>
        RtcSignalingState SignalingState { get; }

        /// <summary>
        /// The updateIce method updates the ICE Agent process of gathering local candidates and pinging remote candidates.
        /// This call may result in a change to the state of the ICE Agent, and may result in a change to media state if it results in 
        /// connectivity being established.
        /// </summary>
        /// <param name="configuration"></param>
        void UpdateIce(RtcConfiguration configuration);

        /// <summary>
        /// The addIceCandidate() method provides a remote candidate to the ICE Agent. 
        /// In addition to being added to the remote description, connectivity checks will be sent to the new candidates 
        /// as long as the ICE Transports setting is not set to none. This call will result in a change to the connection 
        /// state of the ICE Agent, and may result in a change to media state if it results in different connectivity being established.
        /// </summary>
        /// <param name="candidate"></param>
        Task AddIceCandidate(RtcIceCandidate candidate);

        /// <summary>
        /// The gathering state of the RtcPeerConnection ICE Agent.
        /// </summary>
        RtcGatheringState IceGatheringState { get; }

        /// <summary>
        /// The ICE connection state of the RtcPeerConnection ICE Agent.
        /// </summary>
        RtcIceConnectionState IceConnectionState { get; }

        /// <summary>
        /// This attribute indicates whether the remote peer is able to accept trickled ICE candidates [TRICKLE-ICE]. 
        /// The value is determined based on whether a remote description indicates support for trickle ICE, as defined in 
        /// Section 4.1.9 of [RTCWEB-JSEP]. Prior to the completion of setRemoteDescription, this value is null.
        /// </summary>
        bool CanTrickleIceCandidates { get; }

        /// <summary>
        /// Gets or sets the RtcConfiguration object representing the current configuration of this RtcPeerConnection object.
        /// Setting the configuration updates the ICE Agent process of gathering local candidates and pinging remote candidates.
        /// This call may result in a change to the state of the ICE Agent, and may result in a change to media state if it results 
        /// in connectivity being established.
        /// </summary>
        RtcConfiguration Configuration { get; set; }

        ///// <summary>
        ///// Returns a sequence of MediaStream objects representing the streams that are currently sent with this RtcPeerConnection object.
        ///// </summary>
        IEnumerable<IMediaStream> LocalStreams { get; }

        ///// <summary>
        ///// Returns a sequence of MediaStream objects representing the streams that are currently received with this RtcPeerConnection object.
        ///// </summary>
        IEnumerable<IMediaStream> RemoteStreams { get; }

        ///// <summary>
        ///// If a MediaStream object, with an id equal to streamId, exists in this RTCPeerConnection object's stream sets 
        ///// (local streams set or remote streams set), then the getStreamById() returns that MediaStream object. 
        ///// The method returns null if no stream matches the streamId argument.
        ///// </summary>
        ///// <param name="streamId"></param>
        IMediaStream GetStreamById(string streamId);

        ///// <summary>
        ///// Adds a new stream to the RtcPeerConnection.
        ///// </summary>
        ///// <param name="stream"></param>
        void AddStream(IMediaStream stream);

        ///// <summary>
        ///// Removes the given stream from the RtcPeerConnection.
        ///// </summary>
        ///// <param name="stream"></param>
        void RemoveStream(IMediaStream stream);

        /// <summary>
        /// A new stream has been added to the remote streams set.
        /// It is called any time a MediaStream is added by the remote peer. This will be fired only as a 
        /// result of setRemoteDescription. Onnaddstream happens as early as possible after the setRemoteDescription.
        /// This callback does not wait for a given media stream to be accepted or rejected via SDP negotiation.
        /// </summary>
        event EventHandler<MediaStreamEventArgs> OnAddStream;

        /// <summary>
        /// A stream has been removed from the remote streams set.
        /// It is called any time a MediaStream is removed by the remote peer. This will be fired only as a result of setRemoteDescription.
        /// </summary>
        event EventHandler<MediaStreamEventArgs> OnRemoveStream;

        /// <summary>
        /// Destroys the RtcPeerConnection ICE Agent, abruptly ending any active ICE processing and any active streaming, 
        /// and releasing any relevant resources (e.g. TURN permissions). 
        /// Sets the SignalingState to Closed.
        /// </summary>
        void Close();

        /// <summary>
        /// Session negotiation needs to be done at some point in the near future.
        /// </summary>
        event EventHandler OnNegotiationNeeded;

        /// <summary>
        /// A new RtcIceCandidate is made available.
        /// </summary>
        event EventHandler<RtcIceCandidateEventArgs> OnIceCandidate;

        /// <summary>
        /// The RtcPeerConnection signalingState has changed. 
        /// This state change is the result of either setLocalDescription() or setRemoteDescription() being invoked.
        /// </summary>
        event EventHandler OnSignalingStateChange;

        /// <summary>
        /// The RtcPeerConnection ice connection state has changed.
        /// </summary>
        event EventHandler OnIceConnectionStateChange;

        /// <summary>
        /// The RtcPeerConnection ice gathering state has changed.
        /// </summary>
        event EventHandler OnGatheringStateChange;

        #region 5 Peer-to-peer Data API

        /// <summary>
        /// Creates a new IRtcDataChannel object with the given label. 
        /// The RtcDataChannelInit object can be used to configure properties of the underlying channel such as data reliability.
        /// </summary>
        /// <param name="label">The new channel's label is set to this value.</param>
        /// <param name="dataChannelInit">Optional parameters with wich to initialize the new data channel.</param>
        /// <returns></returns>
        IRtcDataChannel CreateDataChannel(string label, RtcDataChannelInit dataChannelInit = null);

        /// <summary>
        /// Fired when a data channel is created by the peer.
        /// </summary>
        event EventHandler<RtcDataChannelEventArgs> OnDataChannel;

        #endregion

        #region 6 Peer-to-peer DTMF
        /// <summary>
        /// The CreateDtmfSender() method creates an IRtcDtmfSender that references the given IMediaStreamTrack. 
        /// The MediaStreamTrack must be an element of a MediaStream that's currently in the RTCPeerConnection object's local streams set; if 
        /// not, throws an InvalidParameter exception and abort these steps.
        /// </summary>
        /// <param name="track">The track with wich to associate the new DTMF sender.</param>
        /// <returns></returns>
        IRtcDtmfSender CreateRtcDtmfSender(IMediaStreamTrack track);
        #endregion

        #region 7 Statistics Model
        /// <summary>
        /// Gathers stats for the given selector and reports the result asynchronously.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<IRtcStatsReport> GetStats(IMediaStreamTrack selector = null);
        #endregion

        #region 8 Identity
        /// <summary>
        /// Sets the identity provider to be used for a given RTCPeerConnection object. 
        /// Applications need not make this call; if the browser is already configured for an IdP, then that configured IdP will be used to get an assertion.
        /// This example shows how to configure the identity provider and protocol.
        /// <example>
        /// pc.setIdentityProvider("example.com", "default", "alice@example.com");
        /// </example>
        /// This example shows how to consume identity assertions inside a Web application.
        /// var identity = await pc.PeerIdentity;
        /// Console.WriteLine($"Idp={identity.Idp} identity={identity.name}");
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="protocol"></param>
        /// <param name="username"></param>
        void SetIdentityProvider(string provider, string protocol = null, string username = null);

        /// <summary>
        /// Initiates the process of obtaining an identity assertion. Applications need not make this call. 
        /// It is merely intended to allow them to start the process of obtaining identity assertions before a call is initiated. 
        /// If an identity is needed, either because the browser has been configured with a default identity provider or because the 
        /// setIdentityProvider() method was called, then an identity will be automatically requested when an offer or answer is created.
        /// </summary>
        /// <returns></returns>
        void GetIdentityAssertion();

        /// <summary>
        /// Contains the peer identity assertion information if an identity assertion was provided and verified. 
        /// Once this value is set to a non-null value, it cannot change.
        /// </summary>
        RtcIdentityAssertion PeerIdentity { get; }

        event EventHandler<RtcIdentityEventArgs> OnIdentityResult;
        event EventHandler OnPeerIdentity;
        event EventHandler<RtcIdentityErrorEventArgs> OnIdpAssertiOnError;
        event EventHandler<RtcIdentityErrorEventArgs> OnIdpValidatiOnError;
        #endregion
    };

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
        public RtcIceCandidateEventArgs(RtcIceCandidate candidate)
        {
            Candidate = candidate;
        }

        RtcIceCandidate Candidate { get; }
    }

    public class RtcIdentityEventArgs : EventArgs
    {
        RtcIdentityEventArgs(string assertion)
        {
            Assertion = assertion;
        }

        string Assertion { get; }
    }

    public class RtcIdentityErrorEventArgs : EventArgs
    {
        RtcIdentityErrorEventArgs(string idp, string protocol, string loginUrl)
        {
            Idp = idp;
            Protocol = protocol;
            LoginUrl = loginUrl;
        }

        string Idp { get; }
        string Protocol { get; }
        string LoginUrl { get; }
    }

    public class RtcDataChannelEventArgs : EventArgs
    {
        public RtcDataChannelEventArgs(IRtcDataChannel channel)
        {
            Channel = channel;
        }

        public IRtcDataChannel Channel { get; }
    }

    public class CreateSessionDescriptionFailue : Exception
    {
        public CreateSessionDescriptionFailue(string message)
            : base(message)
        { }
    }
}
