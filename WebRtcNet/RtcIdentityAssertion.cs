namespace WebRtcNet
{
    /// <summary>
    /// <seealso href="http://www.w3.org/TR/webrtc/#idl-def-RTCIdentityAssertion"/>
    /// </summary>
    public struct RtcIdentityAssertion
    {
        public RtcIdentityAssertion(string idp, string name)
        {
            Idp = idp;
            Name = name;
        }

        /// <summary>
        /// The domain name of the identity provider that validated this identity.
        /// </summary>
        string Idp { get; }

        /// <summary>
        /// An RFC5322-conformant <seealso href="https://tools.ietf.org/html/rfc5322">[RFC5322]</seealso>
        /// representation of the verified peer identity. This identity will have been verified via 
        /// the procedures described in [RTCWEB-SECURITY-ARCH].
        /// </summary>
        string Name { get; }
    }
}
