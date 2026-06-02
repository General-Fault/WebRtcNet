using System.Collections.Generic;
using System.Linq;
using WebRtcNet.Media;

namespace WebRtcNet;

/// <summary>
/// Optional initialization dictionary for <see cref="RtcPeerConnection.AddTransceiver(MediaStreamTrack, RtcRtpTransceiverInit?)"/>
/// and <see cref="RtcPeerConnection.AddTransceiver(MediaStreamTrackKind, RtcRtpTransceiverInit?)"/>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverinit"/>
public sealed record RtcRtpTransceiverInit
{
	/// <summary>
	/// Initializes transceiver initialization settings.
	/// </summary>
	/// <param name="direction">The preferred transceiver direction.</param>
	/// <param name="streams">The streams associated with the sender side of the transceiver.</param>
	/// <param name="sendEncodings">The preferred sender encodings for the transceiver.</param>
	public RtcRtpTransceiverInit(
		RtcRtpTransceiverDirection direction = RtcRtpTransceiverDirection.SendRecv,
		IEnumerable<MediaStream>? streams = null,
		IEnumerable<RtcRtpEncodingParameters>? sendEncodings = null)
	{
		Direction = direction;
		Streams = streams?.ToList() ?? [];
		SendEncodings = sendEncodings?.ToList() ?? [];
	}

	/// <summary>
	/// The preferred transceiver direction.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverinit-direction"/>
	public RtcRtpTransceiverDirection Direction { get; set; } = RtcRtpTransceiverDirection.SendRecv;

	/// <summary>
	/// The streams associated with the sender side of the transceiver.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverinit-streams"/>
	public List<MediaStream> Streams { get; set; } = [];

	/// <summary>
	/// Sender encodings to apply to the transceiver's sender when supported.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcrtptransceiverinit-sendencodings"/>
	public List<RtcRtpEncodingParameters> SendEncodings { get; set; } = [];
}
