using System;

namespace WebRtcNet
{
    public interface IRtcDtmfSender
    {
        /// <summary>
        /// Indicates if the RTCDTMFSender is capable of sending DTMF.
        /// </summary>
        bool CanInsertDtmf { get; }

        /// <summary>
        /// An IRtcDtmfSender object's InsertDtmf method is used to send DTMF tones.
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
        /// <param name="duration">The duration in ms to use for each character passed in the tones parameters.</param>
        /// <param name="interToneGap">The duration in ms for the gap between tones</param>
        void InsertDtmf(string tones, long duration = 100, long interToneGap = 70);

        /// <summary>
        /// The IMediaStreamTrack given as argument to the IRtcPeerConnection.CreateDtmfSender() method.
        /// </summary>
        IMediaStreamTrack Track { get; }

        /// <summary>
        /// Fired for each tone as it is played out.
        /// </summary>
        event EventHandler<RtcDtmfToneChangedEventArgs> OnToneChange;

        /// <summary>
        /// A list of the tones remaining to be played out. For the syntax, content, and interpretation of this list, see 
        /// <see cref="InsertDtmf"/>
        /// </summary>
        string ToneBuffer { get; }

        /// <summary>
        /// The current tone duration value. This value will be the value last set via the InsertDtmf() method, 
        /// or the default value of 100 ms if InsertDtmf() was called without specifying the duration.
        /// </summary>
        int Duration { get; }

        /// <summary>
        /// the current value of the between-tone gap. This value will be the value last set via the InsertDtmf() method, or the 
        /// default value of 70 ms if InsertDtmf() was called without specifying the interToneGap.
        /// </summary>
        int InterToneGap { get; }
    }

    public class RtcDtmfToneChangedEventArgs : EventArgs
    {
        public RtcDtmfToneChangedEventArgs(string tone)
        {
            Tone = tone;
        }

        public string Tone { get; }
    }
}
