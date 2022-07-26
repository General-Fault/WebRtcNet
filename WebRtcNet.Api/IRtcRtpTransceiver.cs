using System.Collections.Generic;

namespace WebRtcNet
{
    /// <summary>
    /// The IRtcRtpTransceiver interface represents a combination of an <see cref="IRtcRtpSender">RTCRtpSender</see> and an
    /// <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> that share a common
    /// <see cref="IRtcIceCandidate.SdpMid">media stream "identification-tag"</see>. As defined in
    /// <see href="https://tools.ietf.org/html/rfc8829#section-3.4.1">[RFC8829] (section 3.4.1.)</see>, an RTCRtpTransceiver
    /// is said to be associated with a <see href="https://tools.ietf.org/html/rfc4566">media description</see> if its
    /// <see cref="Mid"/> property is non-null and matches a
    /// <see cref="IRtcIceCandidate.SdpMid">media stream "identification-tag"</see> in the
    /// <see href="https://tools.ietf.org/html/rfc4566">media description</see>; otherwise it is said to be disassociated with
    /// that <see href="https://tools.ietf.org/html/rfc4566">media description</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver"/>
    public interface IRtcRtpTransceiver
    {
        /// <summary>
        /// The mid attribute is the <see cref="IRtcIceCandidate.SdpMid">media stream "identification-tag"</see> negotiated and
        /// present in the local and remote descriptions.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtptransceiver-mid"/>
        string Mid { get; }

        /// <summary>
        /// The sender attribute exposes the <see cref="IRtcRtpSender">RTCRtpSender</see> corresponding to the RTP media that
        /// may be sent with mid = <see cref="Mid"/>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver-sender"/>
        IRtcRtpSender Sender { get; }

        /// <summary>
        /// The receiver attribute is the <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> corresponding to the RTP media that
        /// may be received with mid = <see cref="Mid"/>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver-receiver"/>
        IRtcRtpReceiver Receiver { get; }

        /// <summary>
        /// As defined in <see hgref="https://tools.ietf.org/html/rfc8829#section-4.2.4">[RFC8829] (section 4.2.4.)</see>, the
        /// direction attribute indicates the preferred direction of this transceiver, which will be used in calls to
        /// <see cref="IRtcPeerConnection.CreateOffer"/> and <see cref="IRtcPeerConnection.CreateAnswer"/>. An update of
        /// directionality does not take effect immediately. Instead, future calls to
        /// <see cref="IRtcPeerConnection.CreateOffer"/> and <see cref="IRtcPeerConnection.CreateAnswer"/> mark the
        /// corresponding <see href="https://tools.ietf.org/html/rfc4566">media description</see> as
        /// <seealso cref="RtcRtpTransceiverDirection.SendRecv"/>, <seealso cref="RtcRtpTransceiverDirection.SendOnly"/>,
        /// <seealso cref="RtcRtpTransceiverDirection.RecvOnly"/> or <seealso cref="RtcRtpTransceiverDirection.Inactive"/> as
        /// defined in <see href="https://tools.ietf.org/html/rfc8829">[RFC8829]</see> (
        /// <see href="https://tools.ietf.org/html/rfc8829#section-5.2.2">section 5.2.2</see>. and
        /// <see href="https://tools.ietf.org/html/rfc8829#section-5.3.2">section 5.3.2</see>.)
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver-direction"/>
        RtcRtpTransceiverDirection Direction { get; set; }

        /// <summary>
        /// As defined in <see href="https://tools.ietf.org/html/rfc8829#section-4.2.5">[RFC8829] (section 4.2.5.)</see>, the
        /// CurrentDirection property indicates the current direction negotiated for this transceiver. The value of
        /// CurrentDirection is independent of the value of <see cref="RtcRtpEncodingParameters.Active"/> since one cannot be
        /// deduced from the other. If this transceiver has never been represented in an offer/answer exchange, the value is
        /// null. If the transceiver is stopped, the value is "stopped".
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver-currentdirection"/>
        RtcRtpTransceiverDirection? CurrentDirection { get; }

        /// <summary>
        /// Irreversibly marks the transceiver as stopping, unless it is already
        /// <see cref="RtcRtpTransceiverDirection.Stopped"/>. This will immediately cause the transceiver's
        /// <see cref="Sender">Sender</see> to no longer send, and its <see cref="Receiver">Receiver</see> to no longer
        /// receive. Calling Stop() also updates the negotiation-needed flag for the
        /// <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> associated
        /// <see cref="IRtcPeerConnection">RTCPeerConnection</see>.
        /// <para>A stopping transceiver will cause future calls to <see cref="IRtcPeerConnection.CreateOffer"/> to generate a
        /// zero port in the <see href="https://tools.ietf.org/html/rfc4566">media description</see> for the corresponding
        /// transceiver, as defined in
        /// <see href="https://tools.ietf.org/html/rfc8829#section-4.2.1">[RFC8829] (section 4.2.1.)</see>. However, to avoid
        /// problems with <see href="https://tools.ietf.org/html/rfc8829">[RFC8843]</see>, a transceiver that is stopping but
        /// not <see cref="RtcRtpTransceiverDirection.Stopped"/>, will not affect
        /// <see cref="IRtcPeerConnection.CreateAnswer"/>.</para>
        /// <para>A <see cref="RtcRtpTransceiverDirection.Stopped"/> transceiver will cause future calls to
        /// <see cref="IRtcPeerConnection.CreateOffer"/> or <see cref="IRtcPeerConnection.CreateAnswer"/> to generate a zero
        /// port in the <see href="https://tools.ietf.org/html/rfc4566">media description</see> for the corresponding
        /// transceiver, as defined in
        /// <see href="https://tools.ietf.org/html/rfc8829#section-4.2.1">[RFC8829] (section 4.2.1.)</see>.</para>
        /// <para>The transceiver will remain in the stopping state, unless it becomes
        /// <see cref="RtcRtpTransceiverDirection.Stopped"/> by <see cref="IRtcPeerConnection.SetRemoteDescription"/>
        /// processing a rejected m-line in a remote offer or answer.</para>
        /// <para>NOTE: A transceiver that is stopping but not <see cref="RtcRtpTransceiverDirection.Stopped"/> will always
        /// need negotiation. In practice, this means that calling Stop() on a transceiver will cause the transceiver to become
        /// <see cref="RtcRtpTransceiverDirection.Stopped"/> eventually, provided negotiation is allowed to complete on both
        /// ends.</para>
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver-stop"/>
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiver-setcodecpreferences"/>
        /// <param name="codecs"></param>
        void SetCodecPreferences(IEnumerable<RtcRtpCodecCapability> codecs);
    }

    /// <summary>
    /// The RtcRtpCodecCapability provides information about codec capabilities. Only capability combinations that would
    /// utilize distinct payload types in a generated SDP offer are provided. For example:
    /// <list type="number">
    ///     <item>Two H.264/AVC codecs, one for each of two supported packetization-mode values.</item>
    ///     <item>Two CN codecs with different clock rates.</item>
    /// </list>
    /// </summary>
    /// <see href="https://www.w3.org/TR/webrtc/#rtcrtpcodeccapability"/>
    public class RtcRtpCodecCapability
    {
        /// <summary>
        /// The codec MIME media type/subtype. Valid media types and subtypes are listed in
        /// <see href="https://www.iana.org/assignments/rtp-parameters/rtp-parameters.xhtml#rtp-parameters-2">[IANA-RTP-2]</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodeccapability-mimetype"/>
        public string MimeType { get; set; }

        /// <summary>
        /// The codec clock rate expressed in Hertz.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodeccapability-clockrate"/>
        public ulong ClockRate { get; set; }

        /// <summary>
        /// If present, indicates the maximum number of channels (mono=1, stereo=2).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodeccapability-channels"/>
        public ushort? Channels { get; set; }

        /// <summary>
        /// The "format specific parameters" field from the <c>a=fmtp</c> line in the SDP corresponding to the codec, if one exists.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcodeccapability-sdpfmtpline"/>
        public string SdpFmtpLine { get; set; }

        public RtcRtpCodecCapability(string mimeType, ulong clockRate)
        {
            MimeType = mimeType;
            ClockRate = clockRate;
        }
    }


    /// <summary>
    /// The direction of an RTCRtpTransceiver
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverdirection"/>
    /// <seealso cref="IRtcRtpTransceiver.Direction"/>
    /// <seealso cref="IRtcRtpTransceiver.CurrentDirection"/>
    public enum RtcRtpTransceiverDirection
    {
        /// <summary>
        /// The <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> <see cref="IRtcRtpSender">RTCRtpSender</see>
        /// <see cref="IRtcRtpTransceiver.Sender">Sender</see> will offer to send RTP, and will send RTP if the remote peer
        /// accepts and <see cref="IRtcRtpTransceiver.Sender">Sender</see>.
        /// <see cref="IRtcRtpSender.GetParameters">GetParameters()</see>.
        /// <see cref="RtcRtpSendParameters.Encodings">Encodings[i]</see>.
        /// <see cref="RtcRtpEncodingParameters.Active">Active</see> is true for any value of i. The
        /// <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> <see cref="IRtcRtpSender">RTCRtpSender</see> will offer
        /// to receive RTP, and will receive RTP if the remote peer accepts.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverdirection-sendrecv"/>
        SendRecv,

        /// <summary>
        /// The <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> <see cref="IRtcRtpSender">RTCRtpSender</see>
        /// <see cref="IRtcRtpTransceiver.Sender">Sender</see> will offer to send RTP, and will send RTP if the remote peer
        /// accepts and <see cref="IRtcRtpTransceiver.Sender">Sender</see>.
        /// <see cref="IRtcRtpSender.GetParameters">GetParameters()</see>.
        /// <see cref="RtcRtpSendParameters.Encodings">Encodings[i]</see>.
        /// <see cref="RtcRtpEncodingParameters.Active">Active</see> is true for any value of i. The
        /// <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> will
        /// not offer to receive RTP, and will not receive RTP
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverdirection-sendonly"/>
        SendOnly,

        /// <summary>
        /// The <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> <see cref="IRtcRtpSender">RTCRtpSender</see> will
        /// not offer to send RTP, and will not send RTP. The  <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see>
        /// <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> will offer to receive RTP, and will receive RTP if the remote
        /// peer accepts.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverdirection-recvonly"/>
        RecvOnly,

        /// <summary>
        /// The <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see> <see cref="IRtcRtpSender">RTCRtpSender</see> will not
        /// offer to send RTP, and will not send RTP. The <see cref="IRtcRtpTransceiver">RTCRtpTransceiver's</see>
        /// <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> will not offer to receive RTP, and will not receive RTP.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverdirection-inactive"/>
        Inactive,

        /// <summary>
        /// The <see cref="IRtcRtpTransceiver">RTCRtpTransceiver</see> will neither send nor receive RTP. It will generate a
        /// zero port in the offer. In answers, its <see cref="IRtcRtpSender">RTCRtpSender</see> will not offer to send RTP,
        /// and its <see cref="IRtcRtpReceiver">RTCRtpReceiver</see> will not offer to receive RTP. This is a terminal state.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverdirection-stopped"/>
        Stopped
    }
}