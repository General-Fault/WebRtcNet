namespace WebRtcNet
{
    /// <summary>
    /// This describes an ICE candidate.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicecandidate-interface"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute"/>
    public interface IRtcIceCandidate
    {
        /// <summary>
        /// his carries the <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see> as
        /// defined in section <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-15.1">15.1 of [RFC5245]</see>.
        /// If this RTCIceCandidate represents an end-of-candidates indication or a peer reflexive remote candidate, candidate
        /// is an empty string.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-candidate"/>
        string Candidate { get; }

        /// <summary>
        /// If not null, this contains the media stream "identification-tag" defined in
        /// <see href="https://tools.ietf.org/html/rfc5888">[RFC5888]</see> for the media component this candidate is
        /// associated with.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-sdpmid"/>
        string SdpMid { get; }

        /// <summary>
        /// If not null, this indicates the index (starting at zero) of the
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-media-description">media description</see> in the SDP this candidate
        /// is associated with.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-sdpmlineindex"/>
        ushort SdpMLineIndex { get; }

        /// <summary>
        /// A unique identifier that allows ICE to correlate candidates that appear on multiple
        /// <see cref="IRtcIceTransport">RTCIceTransports</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-foundation"/>
        string Foundation { get; }

        /// <summary>
        /// The assigned network component of the candidate (<see cref="RtcIceComponent.Rtp">Rtp</see> or
        /// <see cref="RtcIceComponent.Rtcp">Rtcp</see>). This corresponds to the component-id field in
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>, decoded to the string
        /// representation as defined in <see cref="RtcIceComponent"/>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-component"/>
        RtcIceComponent Component { get; }

        /// <summary>
        /// The assigned priority of the candidate.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-priority"/>
        ulong? Priority { get; }

        /// <summary>
        /// The address of the candidate, allowing for IPv4 addresses, IPv6 addresses, and fully qualified domain names
        /// (FQDNs). This corresponds to the connection-address field in
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>.
        /// <para>Remote candidates may be exposed, for instance via
        /// <see cref="IRtcIceTransport.GetSelectedCandidatePair">[[SelectedCandidatePair]]</see>.
        /// <see cref="RtcIceCandidatePair.Remote">Remote</see>. Once a <see cref="IRtcPeerConnection">RTCPeerConnection</see>
        /// instance learns on an address by the web application using
        /// <see cref="IRtcPeerConnection.AddIceCandidate">AddIceCandidate</see>, the address value will be exposed in in any
        /// RTCIceCandidate of the <see cref="IRtcPeerConnection">RTCPeerConnection</see> instance representing a remote
        /// candidate with that newly learnt address.</para>
        /// </summary>
        /// <remarks>
        /// <para>NOTE The addresses exposed in candidates gathered via ICE and made visible to the application in
        /// RTCIceCandidate instances can reveal more information about the device and the user (e.g.location, local network
        /// topology) than the user might have expected in a non-WebRTC enabled browser.</para>
        /// <para>These addresses are always exposed to the application, and potentially exposed to the communicating party,
        /// and can be exposed without any specific user consent(e.g. for peer connections used with data channels, or to
        /// receive media only).</para>
        /// <para>These addresses can also be used as temporary or persistent cross-origin states, and thus contribute to the
        /// fingerprinting surface of the device. (This is a fingerprinting vector.)</para>
        /// <para>Applications can avoid exposing addresses to the communicating party, either temporarily or permanently, by
        /// forcing the ICE Agent to report only relay candidates via the
        /// <see cref="RtcIceTransportPolicy">IceTransportPolicy</see> member of <see cref="RtcConfiguration"/>.</para>
        /// <para>To limit the addresses exposed to the application itself, browsers or applications can offer their users
        /// different policies regarding sharing local addresses, as defined in
        /// <see href="https://tools.ietf.org/html/rfc8827">[RFC8828]</see>.</para>
        /// </remarks>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-address"/>
        string Address { get; }

        /// <summary>
        /// The protocol of the candidate (<see cref="RtcIceProtocol.Udp">udp</see>/<see cref="RtcIceProtocol.Tcp">tcp</see>).
        /// This corresponds to the <c>transport</c> field in
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-protocol"/>
        RtcIceProtocol? Protocol { get; }

        /// <summary>
        /// The port of the candidate.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-type"/>
        ushort? Port { get; }

        /// <summary>
        /// The type of the candidate. This corresponds to the <c>candidate-types</c> field in
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-type"/>
        RtcIceCandidateType? Type { get; }

        /// <summary>
        /// If protocol is <see cref="RtcIceProtocol.Tcp">tcp</see>, TcpType represents the type of TCP candidate. Otherwise,
        /// TcpType is null. This corresponds to the <c>tcp-type</c> field in
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-tcptype"/>
        RtcIceTcpCandidateType? TcpType { get; }

        /// <summary>
        /// For a candidate that is derived from another, such as a relay or reflexive candidate, the relatedAddress is the IP
        /// address of the candidate that it is derived from. For host candidates, the relatedAddress is null. This
        /// corresponds to the <c>rel-address</c> field in
        /// <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-relatedaddress"/>
        string RelatedAddress { get; }

        /// <summary>
        /// For a candidate that is derived from another, such as a relay or reflexive candidate, the relatedPort is the port
        /// of the candidate that it is derived from. For host candidates, the relatedPort is null. This corresponds to the
        /// <c>rel-port</c> field in <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-relatedport"/>
        ushort? RelatedPort { get; }

        /// <summary>
        /// This carries the <c>ufrag</c> as defined in section
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-15.4">15.4 of [RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-usernamefragment"/>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245"/>
        string UsernameFragment { get; }
    }

    /// <summary>
    /// The RtcIceTcpCandidateType represents the type of the ICE TCP candidate, as defined in
    /// <see href="https://datatracker.ietf.org/doc/html/rfc6544">[RFC6544]</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicetcpcandidatetype-enum"/>
    /// <seealso cref="IRtcIceCandidate.TcpType"/>
    public enum RtcIceTcpCandidateType
    {
        /// <summary>
        /// An "active" TCP candidate is one for which the transport will attempt to open an outbound connection but will not
        /// receive incoming connection requests.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetcpcandidatetype-active"/>
        Active,

        /// <summary>
        /// A "passive" TCP candidate is one for which the transport will receive incoming connection attempts but not attempt
        /// a connection.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetcpcandidatetype-passive"/>
        Passive,

        /// <summary>
        /// An "so" candidate is one for which the transport will attempt to open a connection simultaneously with its peer.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicetcpcandidatetype-so"/>
        So
    }

    /// <summary>
    /// The RTCIceCandidateType represents the type of the ICE candidate, as defined in
    /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-15.1">[RFC5245] section 15.1</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcicecandidatetype-enum"/>
    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5245#section-15.1"/>
    /// <seealso cref="IRtcIceCandidate.Type"/>
    public enum RtcIceCandidateType
    {
        /// <summary>
        /// A host candidate, as defined in Section
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.1">4.1.1.1 of [RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidatetype-host"/>
        Host,

        /// <summary>
        /// A server reflexive candidate, as defined in 
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.2">4.1.1.2 of [RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidatetype-srflx"/>
        Srflx,

        /// <summary>
        /// A peer reflexive candidate, as defined in
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-4.1.1.2">4.1.1.2 of [RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidatetype-prflx"/>
        Prflx,

        /// <summary>
        /// A relay candidate, as defined in Section
        /// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-7.1.3.2.1">7.1.3.2.1 of [RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidatetype-relay"/>
        Relay
    }

    /// <summary>
    /// The protocol types used for the ICE connection
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtciceprotocol-enum"/>
    /// <seealso cref="IRtcIceCandidate.Protocol"/>
    public enum RtcIceProtocol
    {
        /// <summary>
        /// A UDP candidate, as described in <see href="https://tools.ietf.org/html/rfc5245">[RFC5245]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceprotocol-udp"/>
        Udp,

        /// <summary>
        /// A TCP candidate, as described in <see href="https://tools.ietf.org/html/rfc6544">[RFC6544]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtciceprotocol-tcp"/>
        Tcp
    }
}
