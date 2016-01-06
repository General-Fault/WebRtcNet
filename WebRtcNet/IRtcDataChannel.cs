using System;

namespace WebRtcNet
{

    /// <summary>
    /// <seealso href="http://w3c.github.io/webrtc-pc/#idl-def-RTCDataChannelInit"/>
    /// </summary>
    public class RtcDataChannelInit
    {
        /// <summary>
        /// If set to false, data is allowed to be delivered out of order. 
        /// The default value of true, guarantees that data will be delivered in order.
        /// </summary>
        public bool Ordered = true;

        /// <summary>
        /// Limits the time during which the channel will transmit or retransmit data if not acknowledged. 
        /// This value may be clamped if it exceeds the maximum value supported by the user agent.
        /// </summary>
        public uint? MaxPacketLifeTime;

        /// <summary>
        /// Limits the number of times a channel will retransmit data if not successfully delivered. 
        /// This value may be clamped if it exceeds the maximum value supported by the user agent..
        /// </summary>
        public uint? MaxRetransmits;

        /// <summary>
        /// Subprotocol name used for this channel.
        /// </summary>
        public string Protocol;

        /// <summary>
        /// The default value of false tells the user agent to announce the channel in-band and instruct the 
        /// other peer to dispatch a corresponding RTCDataChannel object. If set to true, it is up to the application 
        /// to negotiate the channel and create a RTCDataChannel object with the same id at the other peer.
        /// </summary>
        public bool Negotiated;

        /// <summary>
        /// Overrides the default selection of id for this channel.
        /// </summary>
        public ushort? Id;
    };

    /// <summary>
    /// <seealso href="http://w3c.github.io/webrtc-pc/#idl-def-RTCDataChannelState"/>
    /// </summary>
    public enum RtcDataChannelState
    {
        /// <summary>
        /// Attempting to establish the underlying data transport. 
        /// This is the initial state of a RTCDataChannel object created with createDataChannel().
        /// </summary>
        Connecting,

        /// <summary>
        /// The underlying data transport is established and communication is possible. 
        /// This is the initial state of a RTCDataChannel object dispatched as a part of a RTCDataChannelEvent.
        /// </summary>
        Open,

        /// <summary>
        /// The procedure to close down the underlying data transport has started.
        /// </summary>
        Closing,

        /// <summary>
        /// The underlying data transport has been closed or could not be established.
        /// </summary>
        Closed
    };

    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(object data, string origin, string lastEventId)
        {
            Data = data;
            Origin = origin;
            LastEventId = lastEventId;
        }

        public object Data { get; }

        public string Origin { get; }

        public string LastEventId { get; }

        //public IEnumerable<MessagePort> Ports;
    };

    /// <summary>
    /// <seealso href="http://w3c.github.io/webrtc-pc/#rtcdatachannel"/>
    /// </summary>
    public interface IRtcDataChannel
    {
        /// <summary>
        /// The Label represents a label that can be used to distinguish this RTCDataChannel object from other 
        /// RTCDataChannel objects. Applications are allowed to create multiple RTCDataChannel objects with the same label. 
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Ordered returns true if the RTCDataChannel is ordered, and false if other of order delivery is allowed. 
        /// </summary>
        bool Ordered { get; }

        /// <summary>
        /// MaxPacketLifeTime returns the length of the time window (in milliseconds) during which transmissions and 
        /// retransmissions may occur in unreliable mode, or null if unset. 
        /// </summary>
        uint? MaxPacketLifeTime { get; }

        /// <summary>
        /// MaxRetransmits returns the maximum number of retransmissions that are attempted in unreliable mode, 
        /// or null if unset. 
        /// </summary>
        uint? MaxRetransmits { get; }

        /// <summary>
        /// Protocol returns the name of the sub-protocol used with this RTCDataChannel if any, or the empty string otherwise. 
        /// </summary>
        string Protocol { get; }

        /// <summary>
        /// Negotiated returns true if this RTCDataChannel was negotiated by the application, or false otherwise. 
        /// </summary>
        bool Negotiated { get; }

        /// <summary>
        /// The Id returns the id for this RTCDataChannel. The id was either assigned by the user agent at channel 
        /// creation time or selected by the script. 
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// ReadyState represents the state of the RTCDataChannel object. 
        /// </summary>
        RtcDataChannelState ReadyState { get; }

        /// <summary>
        /// The BufferedAmount returns the number of bytes of application data (UTF-8 text and binary data) that have 
        /// been queued using Send() but that, as of the last time the event loop started executing a task, had not yet 
        /// been transmitted to the network. (This thus includes any text sent during the execution of the current task, 
        /// regardless of whether the user agent is able to transmit text asynchronously with script execution.) 
        /// This does not include framing overhead incurred by the protocol, or buffering done by the operating system or 
        /// network hardware. If the channel is closed, this BufferedAmount value will only increase with each call to the Send() 
        /// method (the attribute does not reset to zero once the channel closes).
        /// </summary>
        uint BufferedAmount { get; }

        /// <summary>
        /// This BinaryType controls how binary data is exposed to scripts. See the 
        /// <seealso href="http://www.w3.org/TR/websockets/">[WEBSOCKETS-API]</seealso> for more information.
        /// </summary>
        string BinaryType { get; set; }

        /// <summary>
        /// The RTCDataChannel object's underlying data transport has been established (or re-established).
        /// </summary>
        event EventHandler OnOpen;

        /// <summary>
        /// An error has occurred.
        /// </summary>
        event EventHandler OnError;

        /// <summary>
        /// The RTCDataChannel object's underlying data transport has been closed.
        /// </summary>
        event EventHandler OnClose;

        /// <summary>
        /// A message was successfully received.
        /// </summary>
        event EventHandler<MessageEventArgs> OnMessage;

        /// <summary>
        /// Closes the RTCDataChannel. It may be called regardless of whether the RTCDataChannel object was created by this peer or the remote peer.
        /// </summary>
        void Close();

        void Send(string data);
        void Send(byte[] data);
    };
}
