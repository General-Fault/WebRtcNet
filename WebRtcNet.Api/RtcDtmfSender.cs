using System;

namespace WebRtcNet;

/// <summary>
/// Sends DTMF tones for an RTP sender associated with an audio track.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#rtcdtmfsender-interface" />
public abstract class RtcDtmfSender
{
	/// <summary>
	/// Initializes the DTMF sender wrapper.
	/// </summary>
	protected RtcDtmfSender()
	{
	}

	/// <summary>
	/// Indicates if the RTCDTMFSender is capable of sending DTMF.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtmfsender-caninsertdtmf"/>
	public abstract bool CanInsertDtmf { get; }

	/// <summary>
	/// An RtcDtmfSender object's InsertDtmf method is used to send DTMF tones.
	/// The tones parameter is treated as a series of characters. The characters 0 through 9, A through D, #, and * generate the 
	/// associated DTMF tones. The characters a to d are equivalent to A to D. The character ',' indicates a delay of 2 seconds 
	/// before processing the next character in the tones parameter. All other characters must be considered unrecognized.
	/// The duration parameter indicates the duration in ms to use for each character passed in the tones parameters. The duration 
	/// cannot be more than 6000 ms or less than 40 ms. The default duration is 100 ms for each tone.
	/// The interToneGap parameter indicates the gap between tones. It must be at least 30 ms.The default value is 70 ms.
	/// Ther duration and interToneGap times may increase to cause the times that DTMF start and stop to align with the 
	/// boundaries of RTP packets but it will not increase either of them by more than the duration of a single RTP audio packet.
	/// </summary>
	/// <param name="tones">A series of characters that represent DTMF tones to be sent.</param>
	/// <param name="duration">The duration in milliseconds to use for each character passed in the tones parameters.</param>
	/// <param name="interToneGap">The duration in milliseconds for the gap between tones.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtmfsender-insertdtmf"/>
	public abstract void InsertDtmf(string tones, uint duration = 100, uint interToneGap = 70);

	/// <summary>
	/// Fired for each tone as it is played out.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtmfsender-ontonechange"/>
	public abstract event EventHandler<RtcDtmfToneChangedEventArgs> OnToneChange;

	/// <summary>
	/// A list of the tones remaining to be played out. For the syntax, content, and interpretation of this list, see 
	/// <see cref="InsertDtmf"/>
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-RTCDTMFSender-tonebuffer"/>
	public abstract string ToneBuffer { get; }
}

/// <summary>
/// Event data for <see cref="RtcDtmfSender.OnToneChange" />.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#event-dtmfsender-tonechange" />
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtmftonechangeevent-tone" />
public class RtcDtmfToneChangedEventArgs : EventArgs
{
	/// <summary>
	/// Initializes tone change event arguments.
	/// </summary>
	/// <param name="tone">The tone value that changed.</param>
	public RtcDtmfToneChangedEventArgs(string tone)
	{
		Tone = tone;
	}

	/// <summary>
	/// Gets the tone value for the event.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdtmftonechangeevent-tone" />
	public string Tone { get; }
}