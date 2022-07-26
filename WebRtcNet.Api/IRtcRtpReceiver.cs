using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebRtcNet.Media;

namespace WebRtcNet
{
    /// <summary>
    /// The RTCRtpReceiver interface allows an application to inspect the receipt of a <see cref="IMediaStreamTrack">MediaStreamTrack</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#rtcrtpreceiver-interface"/>
    public interface IRtcRtpReceiver
    {
        /// <summary>
        /// The Track property is the <see cref="IMediaStreamTrack">track</see> that is associated with this RTCRtpReceiver
        /// object receiver.
        /// Note that <see cref="IMediaStreamTrack.Stop">track.stop()</see> is final, although clones are not affected. Since
        /// <see cref="IRtcRtpReceiver.Track">Receiver.Track</see>.<see cref="IMediaStreamTrack.Stop">Stop()</see> does not
        /// implicitly stop receiver, Receiver Reports continue to be sent.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtpreceiver-track"/>
        IMediaStreamTrack Track { get; }

        /// <summary>
        /// The Transport property is the <see cref="IRtcDtlsTransport">transport</see> over which media for the receiver's
        /// <see cref="IMediaStreamTrack">track</see> is received in the form of RTP packets. Prior to construction of the
        /// <see cref="IRtcDtlsTransport">RTCDtlsTransport</see> object, the Transport property will be null. When bundling
        /// is used, multiple RTCRtpReceiver objects will share one transport and will all receive RTP and RTCP over the same
        /// transport.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpreceiver-transport"/>
        IRtcDtlsTransport Transport { get; }

        /*
        /// <summary>
        /// The GetCapabilities() method returns the most optimistic view of the capabilities of the system for receiving
        /// media of the given <see cref="MediaStreamTrackKind">kind</see>. It does not reserve any resources, ports, or other
        /// state but is meant to provide a way to discover the types of capabilities of the browser including which codecs
        /// may be supported. If the system has no capabilities corresponding to the value of the kind argument,
        /// GetCapabilities returns null.
        /// </summary>
        /// <param name="kind">The type of media device for which to request capabilities.</param>
        /// <returns>A MediaTrackCapabilities object describing the capabilities of the requested device kind</returns>
        /// <seealso cref="https://www.w3.org/TR/webrtc/#dom-rtcrtpreceiver-getcapabilities"/>
        static RtcRtpCapabilities? getCapabilities(MediaDeviceKind kind);
        */

        /// <summary>
        /// The GetParameters() method returns the RTCRtpReceiver object's current parameters for how track is decoded.
        /// NOTE Both the local and remote description may affect this list of codecs. For example, if three codecs are offered
        /// , the receiver will be prepared to receive each of them and will return them all from GetParameters. But if the
        /// remote endpoint only answers with two, the absent codec will no longer be returned by GetParameters as the
        /// receiver no longer needs to be prepared to receive it.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpreceiver-getparameters"/>
        /// <returns></returns>
        /// <returns>An object that describes the encoding and transmitting parameters.</returns>
        RtcRtpReceiveParameters GetParameters();

        /// <summary>
        /// Returns an RtcRtpContributingSource for each unique CSRC identifier received by this RTCRtpReceiver in the last 10
        /// seconds, in descending timestamp order.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpreceiver-getcontributingsources"/>
        /// <returns>A sequence of RtcRtpContributingSource's in descending timestamp order.</returns>
        IEnumerable<RtcRtpContributingSource> GetContributingSources();

        /// <summary>
        /// Returns an RtcRtpSynchronizationSource for each unique SSRC identifier received by this RTCRtpReceiver in the last
        /// 10 seconds, in descending timestamp order.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpreceiver-getsynchronizationsources"/>
        /// <returns>A sequence of RtcRtpSynchronizationSource's in descending timestamp order.</returns>
        IEnumerable<RtcRtpSynchronizationSource> GetSynchronizationSources();

        /// <summary>
        /// Gathers <see cref="IRtcStatsReport">statistics</see> for this receiver and reports the result asynchronously.
        /// </summary>
        /// <returns>A task that completes when the <see cref="IRtcStatsReport">statistics</see> for this receiver have been
        /// gathered.</returns>
        Task<IRtcStatsReport> GetStats();
    }

    /// <summary>
    /// The <see cref="RtcRtpContributingSource"/> and <see cref="RtcRtpSynchronizationSource"/> contain information about a
    /// given contributing source (CSRC) or synchronization source (SSRC) respectively. When an audio or video frame from one
    /// or more RTP packets is delivered to the <see cref="IRtcRtpReceiver">RtcRtpReceiver's</see>
    /// <see cref="IMediaStream">MediaStreamTrack</see>. The information relevant to the RtcRtpSynchronizationSource
    /// corresponding to the SSRC identifier, is updated each time, and if an RTP packet contains CSRC identifiers, then the
    /// information relevant to the RTCRtpContributingSource dictionaries corresponding to those CSRC identifiers is also
    /// updated.
    /// <para>NOTE Even if the MediaStreamTrack is not attached to any sink for playout,
    /// <see cref="IRtcRtpReceiver.GetSynchronizationSources"/> and /// <see cref="IRtcRtpReceiver.GetContributingSources"/>
    /// returns up-to-date information as long as the <see cref="IMediaStreamTrack">track</see> is not ended; sinks are not a
    /// prerequisite for decoding RTP packets.</para>
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcontributingsource"/>
    public class RtcRtpContributingSource
    {
        public RtcRtpContributingSource(DateTime timestamp, ulong source, double audioLevel, ulong rtpTimestamp)
        {
            Timestamp = timestamp;
            Source = source;
            AudioLevel = audioLevel;
            RtpTimestamp = rtpTimestamp;
        }

        /// <summary>
        /// The timestamp indicating the most recent time a frame from an RTP packet, originating from this source, was
        /// delivered to the <see cref="IRtcRtpReceiver">RTCRtpReceiver's</see> <see cref="IMediaStreamTrack">MediaStreamTrack</see>.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcontributingsource-timestamp"/>
        public DateTime Timestamp { get; }

        /// <summary>
        /// The CSRC or SSRC identifier of the contributing or synchronization source.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcontributingsource-source"/>
        public ulong Source { get; }

        /// <summary>
        /// Only present for audio receivers. This is a value between 0..1 (linear), where 1.0 represents 0 dBov, 0 represents
        /// silence, and 0.5 represents approximately 6 dBSPL change in the sound pressure level from 0 dBov.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcontributingsource-audiolevel"/>
        public double AudioLevel { get; }

        /// <summary>
        /// The last RTP timestamp, as defined in <see href="">[RFC3550] Section 5.1</see>, of the media played out at timestamp.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpcontributingsource-rtptimestamp"/>
        /// <seealso href="https://tools.ietf.org/html/rfc3550"/>
        public ulong RtpTimestamp { get; }
    }

    /// <inheritdoc cref="RtcRtpContributingSource"/>
    public class RtcRtpSynchronizationSource : RtcRtpContributingSource
    {
        public RtcRtpSynchronizationSource(DateTime timestamp, ulong source, double audioLevel, ulong rtpTimestamp) 
            : base(timestamp, source, audioLevel, rtpTimestamp)
        {
        }
    }
}