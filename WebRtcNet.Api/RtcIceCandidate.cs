namespace WebRtcNet;

/// <summary>
/// Describes an ICE candidate. This is an immutable data transfer object aligned with the W3C
/// <see href="https://www.w3.org/TR/webrtc/#rtcicecandidate-interface">RTCIceCandidate interface</see>.
/// </summary>
/// <remarks>
/// Users construct <see cref="RtcIceCandidate"/> from signaling data (the <c>candidate</c>, <c>sdpMid</c>,
/// <c>sdpMLineIndex</c>, and <c>usernameFragment</c> received from the remote peer) and pass them to
/// <see cref="IRtcPeerConnection.AddIceCandidate"/>. The remaining parsed fields are populated only when a
/// candidate is delivered by the runtime (e.g. via <see cref="IRtcPeerConnection.OnIceCandidate"/>).
/// </remarks>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcicecandidate-interface"/>
public sealed record RtcIceCandidate
{
	/// <summary>
	/// Creates an <see cref="RtcIceCandidate"/> from the data received from a remote peer via signaling.
	/// Corresponds to the <c>RTCIceCandidateInit</c> dictionary in the W3C spec.
	/// </summary>
	/// <param name="candidate">The <c>candidate-attribute</c> string, or empty for end-of-candidates.</param>
	/// <param name="sdpMid">The media stream identification-tag, or null.</param>
	/// <param name="sdpMLineIndex">The zero-based index of the media description in the SDP, or null.</param>
	/// <param name="usernameFragment">The ICE username fragment (ufrag), or null.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-constructor"/>
	public RtcIceCandidate(string candidate, string? sdpMid = null, ushort? sdpMLineIndex = null, string? usernameFragment = null)
	{
		Candidate = candidate;
		SdpMid = sdpMid;
		SdpMLineIndex = sdpMLineIndex;
		UsernameFragment = usernameFragment;
	}

	/// <summary>
	/// Constructor used by interop layers to produce a fully-populated candidate from a native
	/// <c>webrtc::IceCandidateInterface</c>.
	/// </summary>
	public RtcIceCandidate(
		string candidate, string? sdpMid, ushort? sdpMLineIndex, string? usernameFragment,
		string? foundation, RtcIceComponent? component, uint? priority,
		string? address, RtcIceProtocol? protocol, ushort? port,
		RtcIceCandidateType? type, RtcIceTcpCandidateType? tcpType,
		string? relatedAddress, ushort? relatedPort)
	{
		Candidate = candidate;
		SdpMid = sdpMid;
		SdpMLineIndex = sdpMLineIndex;
		UsernameFragment = usernameFragment;
		Foundation = foundation;
		Component = component;
		Priority = priority;
		Address = address;
		Protocol = protocol;
		Port = port;
		Type = type;
		TcpType = tcpType;
		RelatedAddress = relatedAddress;
		RelatedPort = relatedPort;
	}

	/// <summary>
	/// The <see href="https://www.w3.org/TR/webrtc/#dfn-candidate-attribute">candidate-attribute</see> string as defined
	/// in section <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-15.1">15.1 of [RFC5245]</see>.
	/// An empty string signals end-of-candidates or a peer reflexive remote candidate.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-candidate"/>
	public string Candidate { get; init; }

	/// <summary>
	/// The media stream identification-tag defined in
	/// <see href="https://tools.ietf.org/html/rfc5888">[RFC5888]</see> for the media component this candidate is
	/// associated with, or null.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-sdpmid"/>
	public string? SdpMid { get; init; }

	/// <summary>
	/// The zero-based index of the media description in the SDP this candidate is associated with, or null.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-sdpmlineindex"/>
	public ushort? SdpMLineIndex { get; init; }

	/// <summary>
	/// A unique identifier that allows ICE to correlate candidates that appear on multiple
	/// <see cref="IRtcIceTransport">RTCIceTransports</see>, or null for user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-foundation"/>
	public string? Foundation { get; init; }

	/// <summary>
	/// The network component of the candidate (<see cref="RtcIceComponent.Rtp"/> or
	/// <see cref="RtcIceComponent.Rtcp"/>), or null for user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-component"/>
	public RtcIceComponent? Component { get; init; }

	/// <summary>
	/// The assigned priority of the candidate, or null for user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-priority"/>
	public uint? Priority { get; init; }

	/// <summary>
	/// The IP address or FQDN of the candidate, or null for user-created candidates.
	/// </summary>
	/// <remarks>
	/// These addresses can reveal device location and local network topology and contribute to the device fingerprint.
	/// See the <see href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-address">W3C privacy note</see>.
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-address"/>
	public string? Address { get; init; }

	/// <summary>
	/// The transport protocol of the candidate, or null for user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-protocol"/>
	public RtcIceProtocol? Protocol { get; init; }

	/// <summary>
	/// The port of the candidate, or null for user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-port"/>
	public ushort? Port { get; init; }

	/// <summary>
	/// The type of the candidate as defined by the <c>candidate-types</c> field, or null for user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-type"/>
	public RtcIceCandidateType? Type { get; init; }

	/// <summary>
	/// The TCP candidate type when <see cref="Protocol"/> is <see cref="RtcIceProtocol.Tcp"/>, otherwise null.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-tcptype"/>
	public RtcIceTcpCandidateType? TcpType { get; init; }

	/// <summary>
	/// The IP address of the candidate this was derived from, or null for host candidates and user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-relatedaddress"/>
	public string? RelatedAddress { get; init; }

	/// <summary>
	/// The port of the candidate this was derived from, or null for host candidates and user-created candidates.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-relatedport"/>
	public ushort? RelatedPort { get; init; }

	/// <summary>
	/// The ICE username fragment (ufrag) as defined in
	/// <see href="https://datatracker.ietf.org/doc/html/rfc5245#section-15.4">section 15.4 of [RFC5245]</see>, or null.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcicecandidate-usernamefragment"/>
	public string? UsernameFragment { get; init; }
}

/// <summary>
/// The RtcIceTcpCandidateType represents the type of the ICE TCP candidate, as defined in
/// <see href="https://datatracker.ietf.org/doc/html/rfc6544">[RFC6544]</see>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcicetcpcandidatetype-enum"/>
/// <seealso cref="RtcIceCandidate.TcpType"/>
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
/// <seealso cref="RtcIceCandidate.Type"/>
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
/// <seealso cref="RtcIceCandidate.Protocol"/>
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
