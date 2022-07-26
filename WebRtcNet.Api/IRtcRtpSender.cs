using System.Threading.Tasks;
using WebRtcNet.Media;

namespace WebRtcNet
{
    /// <summary>
    /// The IRtcRtpSender interface allows an application to control how a given <see cref="IMediaStreamTrack"/> is encoded
    /// and transmitted to a remote peer. When <see cref="IRtcRtpSender.SetParameters"/> is called on an RTCRtpSender object,
    /// the encoding is changed appropriately.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender"/>
    public interface IRtcRtpSender
    {
        /// <summary>
        /// The Track property is the track that is associated with this RTCRtpSender object. If track is ended, or if the
        /// track's output is disabled, i.e. the track is <see cref="IMediaStreamTrack.Enabled">disabled</see> and/or
        /// <see cref="IMediaStreamTrack.Muted">muted</see>, the RTCRtpSender will send black frames (video) and will not send
        /// audio. In the case of video, the RTCRtpSender should send one black frame per second. If Track is null then the
        /// RTCRtpSender does not send.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-track"/>
        IMediaStreamTrack Track { get; }

        /// <summary>
        /// The Transport property is the transport over which media from a <see cref="IMediaStreamTrack">track</see> is sent
        /// in the form of RTP packets. Prior to construction of the RTCDtlsTransport object, the Transport property will be
        /// null. When bundling is used, multiple RTCRtpSender objects will share one transport and will all send RTP and RTCP
        /// over the same transport.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-transport"/>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransport"/>
        IRtcDtlsTransport Transport { get; }

        /*
        /// <summary>
        /// The GetCapabilities() method returns the most optimistic view of the capabilities of the system for sending media
        /// of the given <see cref="MediaStreamTrackKind">kind</see>. It does not reserve any resources, ports, or other state
        /// but is meant to provide a way to discover the types of capabilities of the browser including which codecs may be
        /// supported. If the system has no capabilities corresponding to the value of the kind argument, GetCapabilities
        /// returns null.
        /// </summary>
        /// <param name="kind">The type of media device for which to request capabilities.</param>
        /// <returns>A MediaTrackCapabilities object describing the capabilities of the requested device kind</returns>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-getcapabilities"/>
        static RtcRtpCapabilities getCapabilities(MediaStreamTrackKind kind);
        */

        /// <summary>
        /// The SetParameters method updates how <see cref="IMediaStreamTrack">track</see> is encoded and transmitted to a
        /// remote peer.
        /// </summary>
        /// <param name="parameters">An object that describes the encoding and transmitting parameters.</param>
        /// <returns>A task that completes when the parameters have been applied.</returns>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-setparameters"/>
        Task SetParameters(RtcRtpSendParameters parameters);

        /// <summary>
        /// The GetParameters() method returns the RTCRtpSender object's current parameters for how track is encoded and
        /// transmitted to a remote <see cref="IRtcRtpReceiver">RTCRtpReceiver</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-getparameters"/>
        /// <returns>An object that describes the encoding and transmitting parameters.</returns>
        RtcRtpSendParameters GetParameters();

        /// <summary>
        /// Attempts to replace the RTCRtpSender's current <see cref="IMediaStreamTrack">track</see> with another
        /// <see cref="IMediaStreamTrack">track</see> provided (or with a null track), without renegotiation.
        /// </summary>
        /// <param name="withTrack">The new track to be used by the sender.</param>
        /// <returns>A task that completes once the new track has been applied.</returns>
        Task ReplaceTrack(IMediaStreamTrack withTrack);

        /// <summary>
        /// Sets the <see cref="IMediaStream">MediaStreams</see> to be associated with this sender's
        /// <see cref="IRtcRtpSender.Track">track</see>.
        /// </summary>
        /// <param name="streams">One or more streams to be applied to this sender's
        /// <see cref="IRtcRtpSender.Track">track</see>.</param>
        void SetStreams(params IMediaStream[] streams);

        /// <summary>
        /// Gathers <see cref="IRtcStatsReport">statistics</see> for this sender and reports the result asynchronously.
        /// </summary>
        /// <returns>A task that completes when the <see cref="IRtcStatsReport">statistics</see> for this sender have been
        /// gathered.</returns>
        Task<IRtcStatsReport> GetStats();

        /// <summary>
        /// Get an RtcDtmfSender for sending DTMF tones to a peer.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-dtmf"/>
        IRtcDtmfSender Dtmf { get; }
    }
}