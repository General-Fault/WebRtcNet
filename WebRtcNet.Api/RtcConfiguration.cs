using System.Collections.Generic;
using System.Linq;

namespace WebRtcNet;

/// <summary>
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#rtcicetransportpolicy-enum" />
public enum RtcIceTransportPolicy
{
	/// <summary>
	///     The ICE engine must not send or receive any packets at this point.
	/// </summary>
	None,

	/// <summary>
	///     The ICE engine must only use media relay candidates such as candidates
	///     passing through a TURN server. This can be used to reduce leakage of
	///     IP addresses in certain use cases.
	/// </summary>
	Relay,

	/// <summary>
	///     The ICE engine may use any type of candidates when this value is specified.
	/// </summary>
	All
}

/// <summary>
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#rtcbundlepolicy-enum" />
public enum RtcBundlePolicy
{
	/// <summary>
	///     Gather ICE candidates for each media type in use (audio, video, and data).
	///     If the remote endpoint is not BUNDLE-aware, negotiate only one audio and video
	///     track on separate transports.
	/// </summary>
	Balanced,

	/// <summary>
	///     Gather ICE candidates for each track. If the remote endpoint is
	///     not BUNDLE-aware, negotiate all media tracks on separate transports.
	/// </summary>
	MaxCompat,

	/// <summary>
	///     Gather ICE candidates for only one track. If the remote endpoint is
	///     not BUNDLE-aware, negotiate only one media track.
	/// </summary>
	MaxBundle
}

/// <summary>
///     Configuration dictionary for creating and updating a peer connection.
/// </summary>
/// <seealso href="http://www.w3.org/TR/webrtc/#rtcconfiguration-type" />
public sealed record RtcConfiguration
{
	public RtcConfiguration(IEnumerable<RtcIceServer> servers = null)
	{
		IceServers = servers?.ToList() ?? [];
	}

	/// <summary>
	///     A list containing URIs of servers available to be used by ICE, such as STUN and TURN server.
	/// </summary>
	public List<RtcIceServer> IceServers { get; set; } = [];

	/// <summary>
	///     Indicates which candidates the ICE engine is allowed to use.
	/// </summary>
	public RtcIceTransportPolicy IceTransportPolicy { get; set; } = RtcIceTransportPolicy.All;

	/// <summary>
	///     Indicates which BundlePolicy to use. Defaults to <see cref="RtcBundlePolicy.Balanced" />.
	/// </summary>
	public RtcBundlePolicy BundlePolicy { get; set; } = RtcBundlePolicy.Balanced;

	/// <summary>
	///     Sets the target peer identity for the RTCPeerConnection.
	/// </summary>
	public string PeerIdentity { get; set; } = string.Empty;
}