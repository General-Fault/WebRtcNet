using System;
using System.Collections.Generic;

namespace WebRtcNet
{
    /// <summary>
    ///     Used to set the initial state of an RtcDataChannel on construction.
    /// </summary>
    /// <seealso href="https://w3c.github.io/webrtc-pc/#idl-def-RtcDataChannelInit" />
    public class RtcDataChannelInit
    {
        /// <summary>
        ///     Overrides the default selection of id for this channel.
        /// </summary>
        public ushort? Id;

        /// <summary>
        ///     Limits the time during which the channel will transmit or retransmit data if not acknowledged. This value may be
        ///     clamped if it exceeds the maximum value supported by the user agent.
        /// </summary>
        public uint? MaxPacketLifeTime;

        /// <summary>
        ///     Limits the number of times a channel will retransmit data if not successfully delivered. This value may be clamped
        ///     if it exceeds the maximum value supported by the user agent..
        /// </summary>
        public uint? MaxRetransmits;

        /// <summary>
        ///     The default value of false tells the user agent to announce the channel in-band and instruct the other peer to
        ///     dispatch a corresponding RtcDataChannel object. If set to true, it is up to the application to negotiate the
        ///     channel and create a RtcDataChannel object with the same id at the other peer.
        /// </summary>
        public bool Negotiated;

        /// <summary>
        ///     If set to false, data is allowed to be delivered out of order. The default value of true, guarantees that data will
        ///     be delivered in order.
        /// </summary>
        public bool Ordered = true;

        /// <summary>
        ///     Subprotocol name used for this channel.
        /// </summary>
        public string Protocol;
    }

    /// <summary>
    ///     Represents the possible states
    /// </summary>
    /// <seealso href="http://w3c.github.io/webrtc-pc/#idl-def-RtcDataChannelState" />
    public enum RtcDataChannelState
    {
        /// <summary>
        ///     Attempting to establish the underlying data transport. This is the initial state of a RtcDataChannel object created
        ///     with createDataChannel().
        /// </summary>
        Connecting,

        /// <summary>
        ///     The underlying data transport is established and communication is possible. This is the initial state of a
        ///     RtcDataChannel object dispatched as a part of a RtcDataChannelEvent.
        /// </summary>
        Open,

        /// <summary>
        ///     The procedure to close down the underlying data transport has started.
        /// </summary>
        Closing,

        /// <summary>
        ///     The underlying data transport has been closed or could not be established.
        /// </summary>
        Closed
    }

    /// <summary>
    ///     The RtcDataChannel interface represents a bi-directional data channel between two peers. An RtcDataChannel is
    ///     created via a <see cref="IRtcPeerConnection.CreateDataChannel">factory method</see> on an
    ///     <see cref="IRtcPeerConnection">RtcPeerConnection</see> object. The messages sent between the browsers are described
    ///     in <see href="https://tools.ietf.org/html/rfc8831">[RFC8831]</see> and
    ///     <see href="https://tools.ietf.org/html/rfc8832">[RFC8832]</see>.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/webrtc/#RtcDataChannel" />
    /// <seealso href="https://tools.ietf.org/html/rfc8831" />
    /// <seealso href="https://tools.ietf.org/html/rfc8832" />
    public interface IRtcDataChannel
    {
        /// <summary>
        ///     The Label represents a label that can be used to distinguish this RtcDataChannel object from other RtcDataChannel
        ///     objects. Applications are allowed to create multiple RtcDataChannel objects with the same label.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-label" />
        string Label { get; }

        /// <summary>
        ///     Ordered returns true if the RtcDataChannel is ordered, and false if other of order delivery is allowed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-ordered" />
        bool Ordered { get; }

        /// <summary>
        ///     MaxPacketLifeTime returns the length of the time window (in milliseconds) during which transmissions and
        ///     retransmissions may occur in unreliable mode, or null if unset.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-maxpacketlifetime" />
        uint? MaxPacketLifeTime { get; }

        /// <summary>
        ///     MaxRetransmits returns the maximum number of retransmissions that are attempted in unreliable mode, or null if
        ///     unset.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-maxretransmits" />
        uint? MaxRetransmits { get; }

        /// <summary>
        ///     Protocol returns the name of the sub-protocol used with this RtcDataChannel if any, or the empty string otherwise.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-protocol" />
        string Protocol { get; }

        /// <summary>
        ///     Negotiated returns true if this RtcDataChannel was negotiated by the application, or false otherwise.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-negotiated" />
        bool Negotiated { get; }

        /// <summary>
        ///     The Id returns the id for this RtcDataChannel. The id was either assigned by the user agent at channel creation
        ///     time or selected by the script.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-RtcDataChannel-id" />
        uint Id { get; }

        /// <summary>
        ///     ReadyState represents the state of the RtcDataChannel object.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-readystate" />
        RtcDataChannelState ReadyState { get; }

        /// <summary>
        ///     The BufferedAmount returns the number of bytes of application data (UTF-8 text and binary data) that have been
        ///     queued using <see cref="Send(IEnumerable{byte})">Send</see> but that, as of the last time the event loop started
        ///     executing a task, had not yet been transmitted to the network. (This thus includes any text sent during the
        ///     execution of the current task, regardless of whether the user agent is able to transmit text asynchronously with
        ///     script execution.) This does not include framing overhead incurred by the protocol, or buffering done by the
        ///     operating system or network hardware. If the channel is closed, this BufferedAmount value will only increase with
        ///     each call to the <see cref="Send(IEnumerable{byte})">Send()</see> method (the attribute does not reset to
        ///     zero once the channel closes).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-bufferedamount" />
        /// <seealso cref="Send(string)" />
        /// <seealso cref="Send(IEnumerable{byte})" />
        ulong BufferedAmount { get; }

        /// <summary>
        /// </summary>
        /// <seealso cref="BufferedAmount" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-RtcDataChannel-bufferedamountlowthreshold" />
        ulong? BufferedAmountLowThreshold { get; set; }

        /// <summary>
        ///     This BinaryType controls how binary data is exposed to scripts. See the
        ///     <seealso href="http://www.w3.org/TR/websockets/">[WEBSOCKETS-API]</seealso> for more information.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-binarytype" />
        string BinaryType { get; set; }

        /// <summary>
        ///     The RtcDataChannel object's underlying data transport has been established (or re-established).
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-RtcDataChannel-onopen" />
        event EventHandler OnOpen;

        /// <summary>
        ///     An error has occurred.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onerror" />
        event EventHandler<RtcErrorEventArgs> OnError;

        /// <summary>
        ///     The RtcDataChannel object's underlying data transport has been closed.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onclose" />
        event EventHandler OnClose;

        /// <summary>
        ///     A message was successfully received.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onmessage" />
        event EventHandler<MessageEventArgs> OnMessage;

        /// <summary>
        ///     The RTCDataChannel object's <see cref="BufferedAmount" /> decreases from above its
        ///     <see cref="BufferedAmountLowThreshold" /> to less than or equal to its <see cref="BufferedAmountLowThreshold" />.
        /// </summary>
        /// <seealso cref="BufferedAmountLowThreshold" />
        /// <seealso cref="BufferedAmount" />
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onbufferedamountlow" />
        event EventHandler OnBufferedAmountLow;

        /// <summary>
        ///     Closes the RtcDataChannel. It may be called regardless of whether the RtcDataChannel object was created by this
        ///     peer or the remote peer.
        /// </summary>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-close" />
        void Close();

        /// <summary>
        ///     Send string data through the data channel to a peer.
        /// </summary>
        /// <param name="data"></param>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
        void Send(string data);

        /// <summary>
        ///     Send byte data through the data channel to a peer.
        /// </summary>
        /// <param name="data">An enumeration of bytes to send to the peer.</param>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
        void Send(IEnumerable<byte> data);

        /// <summary>
        ///     Send byte data through the data channel to a peer.
        /// </summary>
        /// <param name="data">An array of bytes to send to the peer.</param>
        /// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
        void Send(byte[] data);
    }

    /// <summary>
    /// Arguments for the <see cref="IRtcDataChannel.OnMessage"/> event. 
    /// </summary>
    /// <seealso cref="IRtcDataChannel.OnMessage"/>
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
    }
}