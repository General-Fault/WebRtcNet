using System;
using System.Collections.Generic;

namespace WebRtcNet
{
    /// <summary>
    /// Describes the role of the ICE agent.
    /// </summary>
    /// <seealso cref="IRtcIceTransport.Role"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicerole"/>
    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245#section-5.2"/>
    public enum RtcIceRole
    {
        /// <summary>
        /// An agent whose role as defined by
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-3">[RFC5245] Section 3</see>, has not yet been
        /// determined.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicerole-unknown"/>
        Unknown,

        /// <summary>
        /// A controlling agent as defined by
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-3">[RFC5245] Section 3</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicerole"/>
        Controlling,

        /// <summary>
        /// A controlled agent as defined by
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-3">[RFC5245] Section 3</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicerole-controlled"/>
        Controlled
    };

    /// <summary>
    /// Represents the different states that the ICE gathering process goes through.
    /// </summary>
    /// <seealso cref="IRtcPeerConnection.IceGatheringState"/>
    /// <seealso cref="IRtcIceTransport.GatheringState"/>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcicegatheringstate-enum"/>
    public enum RtcIceGatheringState
    {
        /// <summary>
        /// The object was just created, and no networking has occurred yet.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicegatheringstate-new"/>
        New,

        /// <summary>
        /// The ICE engine is in the process of gathering candidates for this RTCPeerConnection.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicegatheringstate-gathering"/>
        Gathering,

        /// <summary>
        /// The ICE engine has completed gathering.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicegatheringstate-complete"/>
        Complete
    };

    /// <summary>
    /// Describes how the the possible ways an ICE Transport is used.
    /// </summary>
    /// <seealso cref="IRtcIceTransport.Component"/>
    /// <seealso cref="IRtcIceCandidate.Component"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicecomponent"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#candidate-attribute-grammar"/>
    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.1"/>
    public enum RtcIceComponent
    {
        /// <summary>
        /// The ICE Transport is used for RTP (or RTCP multiplexing), as defined in
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.1">[RFC5245], Section 4.1.1.1</see>.
        /// Protocols multiplexed with RTP (e.g. data channel) share its component ID. This represents the component-id value
        /// 1 when encoded in <see href="https://www.w3.org/TR/webrtc/#candidate-attribute-grammar">candidate-attribute</see>.
        /// </summary>
        /// <seealso cref="IRtcIceCandidate.Candidate"/>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecomponent-rtp"/>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.1"/>
        Rtp,

        /// <summary>
        /// The ICE Transport is used for RTCP as defined by
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.1">[RFC5245], Section 4.1.1.1</see>. This
        /// represents the component-id value 2 when encoded in
        /// <see href="https://www.w3.org/TR/webrtc/#candidate-attribute-grammar">candidate-attribute</see>.
        /// </summary>
        /// <seealso cref="IRtcIceCandidate.Candidate"/>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecomponent-rtcp"/>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.1"/>
        Rtcp
    };


    /// <summary>
    /// Describes the possible states of an Ice transport.
    /// Note this is currently identical to the <see cref="RtcIceConnectionState"/> enum used by
    /// <see cref="IRtcPeerConnection.ConnectionState"/> and the <see cref="RtcDtlsTransportState"/> used by
    /// <see cref="IRtcDtlsTransport.State"/>.
    /// </summary>
    /// <seealso cref="IRtcIceTransport.State"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicetransportstate"/>
    public enum RtcIceTransportState
    {
        /// <summary>
        /// The <see cref="IRtcIceTransport">RTCIceTransport</see> is gathering candidates and/or waiting for remote candidates to
        /// be supplied, and has not yet started checking.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-new"/>
        New,

        /// <summary>
        /// The <see cref="IRtcIceTransport">RTCIceTransport</see> has received at least one remote candidate and is checking
        /// candidate pairs and has either not yet found a connection or consent checks
        /// <see href="https://tools.ietf.org/html/rfc7675">[RFC7675]</see> have failed on all previously successful candidate
        /// pairs. In addition to checking, it may also still be gathering.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-checking"/>
        Checking,

        /// <summary>
        /// The <see cref="IRtcIceTransport">RTCIceTransport</see> has found a usable connection, but is still checking other
        /// candidate pairs to see if there is a better connection. It may also still be gathering and/or waiting for
        /// additional remote candidates. If consent checks <see href="https://tools.ietf.org/html/rfc7675">[RFC7675]</see>
        /// fail on the connection in use, and there are no other successful candidate pairs available, then the state transitions to
        /// <see cref="Checking"/> (if there are candidate pairs remaining to be checked) or <see cref="Disconnected"/> (if
        /// there are no candidate pairs to check, but the peer is still gathering and/or waiting for additional remote candidates).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-connected"/>
        Connected,

        /// <summary>
        /// The <see cref="IRtcIceTransport">RTCIceTransport</see> has finished gathering, received an indication that there
        /// are no more remote candidates, finished checking all candidate pairs and found a connection. If consent checks
        /// <see href="https://tools.ietf.org/html/rfc7675">[RFC7675]</see> subsequently fail on all successful candidate
        /// pairs, the state transitions to <see cref="Failed"/>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-completed"/>
        Completed,

        /// <summary>
        /// The <see cref="IRtcIceTransport">RTCIceTransport</see> has finished gathering, received an indication that there
        /// are no more remote candidates, finished checking all candidate pairs, and all pairs have either failed
        /// connectivity checks or have lost consent. This is a terminal state until ICE is restarted. Since an ICE restart
        /// may cause connectivity to resume, entering the "failed" state does not cause DTLS transports, SCTP associations or
        /// the data channels that run over them to close, or tracks to mute.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-failed"/>
        Failed,

        /// <summary>
        /// The ICE Agent has determined that connectivity is currently lost for this
        /// <see cref="IRtcIceTransport">RTCIceTransport</see>. This is a transient state that may trigger intermittently (and
        /// resolve itself without action) on a flaky network. The way this state is determined is implementation dependent.
        /// Examples include:
        /// <list type="bullet">
        /// <item>Losing the network interface for the connection in use.</item>
        /// <item>Repeatedly failing to receive a response to STUN requests.</item>
        /// </list>
        /// Alternatively, the RTCIceTransport has finished checking all existing candidates pairs and not found a connection
        /// (or consent checks <see href="https://tools.ietf.org/html/rfc7675">[RFC7675]</see> once successful, have now
        /// failed), but it is still gathering and/or waiting for additional remote candidates.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-disconnected"/>
        Disconnected,

        /// <summary>
        /// The <see cref="IRtcIceTransport">RTCIceTransport</see>has shut down and is no longer responding to STUN requests.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransportstate-closed"/>
        Closed
    }

    /// <summary>
    /// A class containing both the local and remote ice candidates for a transport.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicecandidatepair"/>
    /// <seealso cref="IRtcIceTransport.GetSelectedCandidatePair"/>
    public class RtcIceCandidatePair
    {
        public RtcIceCandidatePair(IRtcIceCandidate local, IRtcIceCandidate remote)
        {
            Local = local;
            Remote = remote;
        }

        /// <summary>
        /// The local ICE candidate.
        /// </summary>
        /// <seealso cref="IRtcIceTransport.GetSelectedCandidatePair"/>
        public IRtcIceCandidate Local { get; set; }

        /// <summary>
        /// The remote ICE candidate.
        /// </summary>
        /// <seealso cref="IRtcIceTransport.GetSelectedCandidatePair"/>
        public IRtcIceCandidate Remote { get; set; }
    };

    /// <summary>
    /// Parameters used by and <see cref="IRtcIceTransport">RTCIceTransport</see>.
    /// </summary>
    /// <seealso cref="IRtcIceTransport.GetLocalParameters"/>
    /// <seealso cref="IRtcIceTransport.GetRemoteParameters"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtciceparameters"/>
    public class RtcIceParameters
    {
        public RtcIceParameters(string usernameFragment, string password)
        {
            UsernameFragment = usernameFragment;
            Password = password;
        }


        /// <summary>
        /// The ICE username fragment as defined in
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-7.1.2.3">[RFC5245], Section 7.1.2.3</see>.
        /// </summary>
        /// <seealso href=""/>
        public string UsernameFragment { get; set; }

        /// <summary>
        /// The ICE password as defined in
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-7.1.2.3">[RFC5245], Section 7.1.2.3</see>.
        /// </summary>
        public string Password { get; set; }
    }


    /// <summary>
    /// The IRtcIceTransport interface allows an application access to information about the ICE transport over which packets
    /// are sent and received. In particular, ICE manages peer-to-peer connections which involve state which the application
    /// may want to access. RTCIceTransport objects are constructed as a result of calls to
    /// <see cref="IRtcPeerConnection.SetLocalDescription"/> and <see cref="IRtcPeerConnection.SetRemoteDescription"/>. The
    /// underlying ICE state is managed by the ICE agent; as such, the state of an RTCIceTransport changes when the ICE Agent
    /// provides indications to the user agent as described below. Each RTCIceTransport object represents the ICE transport
    /// layer for the RTP or RTCP component of a specific <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see>, or a group
    /// of <see cref="IRtcRtpTransceiver">RTCRtpTransceivers</see> if such a group has been negotiated via
    /// <see href="https://tools.ietf.org/html/rfc8843">[RFC8843]</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicetransport"/>
    /// <seealso href="https://tools.ietf.org/html/rfc8843"/>
    /// <seealso cref="IRtcDtlsTransport"/>
    public interface IRtcIceTransport
    {
        /// <summary>
        /// The role of this transport.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-icetransport-role"/>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245#section-5.2"/>
        RtcIceRole Role { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-icetransport-component"/>
        RtcIceComponent Component { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-icetransport-state"/>
        RtcIceTransportState State { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-icetransport-gatheringstate"/>
        RtcIceGatheringState GatheringState { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-getlocalcandidates"/>
        IEnumerable<IRtcIceCandidate> GetLocalCandidates();

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-getremotecandidates"/>
        IEnumerable<IRtcIceCandidate> GetRemoteCandidates();

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-getselectedcandidatepair"/>
        RtcIceCandidatePair GetSelectedCandidatePair();

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-getlocalparameters"/>
        RtcIceParameters GetLocalParameters();

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-getremoteparameters"/>
        RtcIceParameters GetRemoteParameters();

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-onstatechange"/>
        event EventHandler OnStateChange;

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-ongatheringstatechange"/>
        event EventHandler OnGatheringStateChange;

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetransport-onselectedcandidatepairchange"/>
        event EventHandler OnSelectedCandidatePairChange;
    };
}