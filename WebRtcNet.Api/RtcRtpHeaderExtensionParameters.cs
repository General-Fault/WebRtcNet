namespace WebRtcNet
{
    /// <summary>
    /// The RtcRtpHeaderExtensionParameters enables an application to determine whether a header extension is configured for
    /// use within an <see cref="IRtcRtpSender">RTCRtpSender</see> or <see cref="IRtcRtpReceiver">RTCRtpReceiver</see>. For an
    /// <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see> transceiver, an application can determine the "direction"
    /// parameter (defined in <see href="https://datatracker.ietf.org/doc/html/rfc5285#section-5">Section 5 of [RFC5285]</see>)
    /// of a header extension as follows without having to parse SDP:
    /// <list type="">
    /// <item>sendonly: The header extension is only included in <see cref="IRtcRtpTransceiver.Sender"/>.<see cref="IRtcRtpSender.GetParameters">GetParameters()</see>.<see cref="RtcRtpParameters.HeaderExtensions">HeaderExtensions</see>.</item>
    /// <item>recvonly: The header extension is only included in  <see cref="IRtcRtpTransceiver.Receiver"/>.<see cref="IRtcRtpReceiver.GetParameters">GetParameters()</see>.<see cref="RtcRtpParameters.HeaderExtensions">HeaderExtensions</see>.</item>
    /// <item>sendrecv: The header extension is included in both  <see cref="IRtcRtpTransceiver.Sender"/>.<see cref="IRtcRtpSender.GetParameters">GetParameters()</see>.<see cref="RtcRtpParameters.HeaderExtensions">HeaderExtensions</see> and  <see cref="IRtcRtpTransceiver.Receiver"/>.<see cref="IRtcRtpReceiver.GetParameters">GetParameters()</see>.<see cref="RtcRtpParameters.HeaderExtensions">HeaderExtensions</see>.</item>
    /// <item>inactive: The header extension is included in neither  <see cref="IRtcRtpTransceiver.Sender"/>.<see cref="IRtcRtpSender.GetParameters">GetParameters()</see>.<see cref="RtcRtpParameters.HeaderExtensions">HeaderExtensions</see>. nor  <see cref="IRtcRtpTransceiver.Receiver"/>.<see cref="IRtcRtpReceiver.GetParameters">GetParameters()</see>.<see cref="RtcRtpParameters.HeaderExtensions">HeaderExtensions</see>.</item>
    /// </list>
    /// </summary>
    /// <seealso cref="RtcRtpParameters"/>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpheaderextensionparameters"/>
    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5285#section-5"/>
    public class RtcRtpHeaderExtensionParameters
    {
        public RtcRtpHeaderExtensionParameters(string uri, ushort id, bool encrypted)
        {
            Uri = uri;
            Id = id;
            Encrypted = encrypted;
        }

        /// <summary>
        /// The URI of the RTP header extension, as defined in <see href="https://datatracker.ietf.org/doc/html/rfc5285">[RFC5285]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpheaderextensionparameters-uri"/>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5285"/>
        public string Uri { get; }

        /// <summary>
        /// The value put in the RTP packet to identify the header extension.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpheaderextensionparameters-id"/>
        public ushort Id { get; }

        /// <summary>
        /// Whether the header extension is encrypted or not.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpheaderextensionparameters-encrypted"/>
        public bool Encrypted { get; }
    }
}