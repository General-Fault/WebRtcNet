using System;
using System.Threading.Tasks;
using WebRtcNet.Media;

namespace WebRtcNet;

/// <summary>
/// The RtcRtpSender interface allows an application to control how a given <see cref="MediaStreamTrack"/> is encoded
/// and transmitted to a remote peer. When <see cref="RtcRtpSender.SetParameters"/> is called on an RTCRtpSender object,
/// the encoding is changed appropriately.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender"/>
public abstract class RtcRtpSender
{
	/// <summary>
	/// Initializes the RTP sender wrapper.
	/// </summary>
	protected RtcRtpSender()
	{
	}

	/// <summary>
	/// Returns the native RTP sender interface used by WebRtcInterop.
	/// </summary>
	/// <remarks>
	/// This method is intended for internal use by WebRtcInterop implementations only.
	/// </remarks>
	/// <param name="throwOnDisposed">True to throw when the sender has already been disposed.</param>
	public abstract IntPtr GetNativeRtpSenderHandle(bool throwOnDisposed);

	/// <summary>
	/// The Track property is the <see cref="MediaStreamTrack">track</see> associated with this RTCRtpSender object. If
	/// the track is ended, or if the track's output is disabled, i.e. the track is
	/// <see cref="MediaStreamTrack.Enabled">disabled</see> and/or <see cref="MediaStreamTrack.Muted">muted</see>, the
	/// RTCRtpSender will send black frames (video) and will not send audio. In the case of video, the RTCRtpSender should
	/// send one black frame per second. If Track is null then the RTCRtpSender does not send.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-track"/>
	public abstract MediaStreamTrack? Track { get; }

	/// <summary>
	/// The Transport property is the transport over which media from a <see cref="MediaStreamTrack">track</see> is sent
	/// in the form of RTP packets. Prior to construction of the RTCDtlsTransport object, the Transport property will be
	/// null. When bundling is used, multiple RTCRtpSender objects will share one transport and will all send RTP and RTCP
	/// over the same transport.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-transport"/>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtlstransport"/>
	public abstract RtcDtlsTransport? Transport { get; }

	/// <summary>
	/// The GetCapabilities() method returns the most optimistic view of the capabilities of the system for sending media
	/// of the given <see cref="MediaStreamTrackKind">kind</see>. It does not reserve any resources, ports, or other state
	/// but is meant to provide a way to discover the types of capabilities of the host including which codecs may be
	/// supported. If the system has no capabilities corresponding to the value of the kind argument, GetCapabilities
	/// returns null.
	/// </summary>
	/// <param name="kind">The type of media device for which to request capabilities.</param>
	/// <returns>The RTP capabilities for the requested media <paramref name="kind"/>.</returns>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-getcapabilities"/>
	public static RtcRtpCapabilities? GetCapabilities(MediaStreamTrackKind kind)
	{
		throw new NotSupportedException();
	}

	/// <summary>
	/// The SetParameters method updates how <see cref="MediaStreamTrack">track</see> is encoded and transmitted to a
	/// remote peer, with optional sender-side options.
	/// </summary>
	/// <param name="parameters">An object that describes the encoding and transmitting parameters.</param>
	/// <param name="options">Optional sender-side options supplied to the spec's updated SetParameters entry point.</param>
	/// <returns>A task that completes when the parameters have been applied.</returns>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-setparameters"/>
	public abstract Task SetParameters(RtcRtpSendParameters parameters, RtcSetParameterOptions? options = null);

	/// <summary>
	/// The GetParameters() method returns the RTCRtpSender object's current parameters for how track is encoded and
	/// transmitted to a remote <see cref="RtcRtpReceiver">RTCRtpReceiver</see>.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-getparameters"/>
	/// <returns>An object that describes the encoding and transmitting parameters.</returns>
	public abstract RtcRtpSendParameters GetParameters();

	/// <summary>
	/// Attempts to replace the RTCRtpSender's current <see cref="MediaStreamTrack">track</see> with another
	/// <see cref="MediaStreamTrack">track</see> provided (or with a null track), without renegotiation.
	/// </summary>
	/// <param name="withTrack">The new track to be used by the sender, or null to stop sending without renegotiation.</param>
	/// <returns>A task that completes once the new track has been applied.</returns>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-replacetrack"/>
	public abstract Task ReplaceTrack(MediaStreamTrack? withTrack);

	/// <summary>
	/// Sets the <see cref="MediaStream">MediaStreams</see> to be associated with this sender's
	/// <see cref="RtcRtpSender.Track">track</see>.
	/// </summary>
	/// <param name="streams">One or more streams to be applied to this sender's
	/// <see cref="RtcRtpSender.Track">track</see>.</param>
	public abstract void SetStreams(params MediaStream[] streams);

	/// <summary>
	/// Gathers <see cref="RtcStatsReport">statistics</see> for this sender and reports the result asynchronously.
	/// </summary>
	/// <returns>A task that completes when the <see cref="RtcStatsReport">statistics</see> for this sender have been
	/// gathered.</returns>
	public abstract Task<RtcStatsReport> GetStats();

	/// <summary>
	/// Get an RtcDtmfSender for sending DTMF tones to a peer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-dtmf"/>
	public abstract RtcDtmfSender? Dtmf { get; }
}

/// <summary>
/// Optional parameters accepted by the updated RTCRtpSender.SetParameters overload.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtpsender-setparameters"/>
public sealed record RtcSetParameterOptions
{
}