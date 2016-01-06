using System.Collections.Generic;

namespace WebRtcNet
{

    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcicetransportpolicy-enum"/>
    /// </summary>
    public enum RtcIceTransportPolicy
    {
        /// <summary>
        /// The ICE engine must not send or receive any packets at this point.
        /// </summary>
        None,

        /// <summary>
        /// The ICE engine must only use media relay candidates such as candidates 
        /// passing through a TURN server. This can be used to reduce leakage of 
        /// IP addresses in certain use cases.
        /// </summary>
        Relay,

        /// <summary>
        /// The ICE engine may use any type of candidates when this value is specified.
        /// </summary>
        All,
    };

    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcbundlepolicy-enum"/>
    /// </summary>
    public enum RtcBundlePolicy
    {
        /// <summary>
        /// Gather ICE candidates for each media type in use (audio, video, and data). 
        /// If the remote endpoint is not BUNDLE-aware, negotiate only one audio and video 
        /// track on separate transports.
        /// </summary>
        Balanced,

        /// <summary>
        /// Gather ICE candidates for each track.If the remote endpoint is 
        /// not BUNDLE - aware, negotiate all media tracks on separate transports.
        /// </summary>
        MaxCompat,

        /// <summary>
        /// Gather ICE candidates for only one track. If the remote endpoint is 
        /// not BUNDLE-aware, negotiate only one media track.
        /// </summary>
        MaxBundle
    };

    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#rtcconfiguration-type"/>
    /// </summary>
    public class RtcConfiguration
    {
        public RtcConfiguration(IEnumerable<RtcIceServer> servers)
        {
            IceServers = new List<RtcIceServer>(servers);

        }

        /// A list containing URIs of servers available to be used by ICE, such as STUN and TURN server.
        public List<RtcIceServer> IceServers;

        /// Indicates which candidates the ICE engine is allowed to use.
        public RtcIceTransportPolicy IceTransportPolicy;

        /// Indicates which BundlePolicy to use. Defaults to "Balanced"
        public RtcBundlePolicy BundlePolicy;

        /// Sets the target peer identity for the RTCPeerConnection. The RTCPeerConnection will establish 
        /// a connection to a remote peer unless it can be successfully authenticated with the provided name.
        public string PeerIdentity;
    };
}
