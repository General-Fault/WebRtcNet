using System;
using System.Collections.Generic;

namespace WebRtcNet;

/// <summary>
/// Used to set the initial state of an RtcDataChannel on construction.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit" />
public record RtcDataChannelInit
{
	/// <summary>
	/// Overrides the default selection of id for this channel.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit-id" />
	public ushort? Id { get; init; } = null;

	/// <summary>
	/// Limits the time during which the channel will transmit or retransmit data if not acknowledged, in milliseconds. This value may be
	/// clamped if it exceeds the maximum value supported by the platform.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit-maxpacketlifetime" />
	public ushort? MaxPacketLifeTime { get; init; } = null;

	/// <summary>
	/// Limits the number of times a channel will retransmit data if not successfully delivered. This value may be clamped
	/// if it exceeds the maximum value supported by the platform.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit-maxretransmits" />
	public ushort? MaxRetransmits { get; init; } = null;

	/// <summary>
	/// The default value of false tells the platform to announce the channel in-band and instruct the other peer to
	/// dispatch a corresponding RtcDataChannel object. If set to true, it is up to the application to negotiate the
	/// channel and create a RtcDataChannel object with the same id at the other peer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit-negotiated" />
	public bool Negotiated { get; init; } = false;

	/// <summary>
	/// If set to false, data is allowed to be delivered out of order. The default value of true, guarantees that data will
	/// be delivered in order.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit-ordered" />
	public bool Ordered { get; init; } = true;

	/// <summary>
	/// Subprotocol name used for this channel.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelinit-protocol" />
	public string Protocol { get; init; } = string.Empty;
}

/// <summary>
/// Represents the possible states
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannelstate" />
public enum RtcDataChannelState
{
	/// <summary>
	/// Attempting to establish the underlying data transport. This is the initial state of a RtcDataChannel object created
	/// with createDataChannel().
	/// </summary>
	Connecting,

	/// <summary>
	/// The underlying data transport is established and communication is possible. This is the initial state of a
	/// RtcDataChannel object dispatched as a part of a RtcDataChannelEvent.
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
}

/// <summary>
/// The RtcDataChannel interface represents a bidirectional data channel between two peers. An RtcDataChannel is
/// created via a <see cref="RtcPeerConnection.CreateDataChannel">factory method</see> on an
/// <see cref="RtcPeerConnection">RtcPeerConnection</see> object. The messages sent between the endpoints are described
/// in <see href="https://tools.ietf.org/html/rfc8831">[RFC8831]</see> and
/// <see href="https://tools.ietf.org/html/rfc8832">[RFC8832]</see>.
/// </summary>
/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel" />
/// <seealso href="https://tools.ietf.org/html/rfc8831" />
/// <seealso href="https://tools.ietf.org/html/rfc8832" />
public abstract class RtcDataChannel
{
	/// <summary>
	/// Initializes the data channel wrapper.
	/// </summary>
	protected RtcDataChannel()
	{
	}

	/// <summary>
	/// Returns the native data channel handle used by WebRtcInterop.
	/// </summary>
	/// <remarks>
	/// This method is intended for internal use by WebRtcInterop implementations only.
	/// </remarks>
	/// <param name="throwOnDisposed">True to throw when the data channel has already been disposed.</param>
	public abstract IntPtr GetNativeDataChannelHandle(bool throwOnDisposed);

	/// <summary>
	/// The Label represents a label that can be used to distinguish this RtcDataChannel object from other RtcDataChannel
	/// objects. Applications are allowed to create multiple RtcDataChannel objects with the same label.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-label" />
	public abstract string Label { get; }

	/// <summary>
	/// Ordered returns true if the RtcDataChannel is ordered, and false if other of order delivery is allowed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-ordered" />
	public abstract bool Ordered { get; }

	/// <summary>
	/// MaxPacketLifeTime returns the length of the time window (in milliseconds) during which transmissions and
	/// retransmissions may occur in unreliable mode, or null if unset.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-maxpacketlifetime" />
	public abstract ushort? MaxPacketLifeTime { get; }

	/// <summary>
	/// MaxRetransmits returns the maximum number of retransmissions that are attempted in unreliable mode, or null if
	/// unset.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-maxretransmits" />
	public abstract ushort? MaxRetransmits { get; }

	/// <summary>
	/// Protocol returns the name of the sub-protocol used with this RtcDataChannel if any, or the empty string otherwise.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-protocol" />
	public abstract string Protocol { get; }

	/// <summary>
	/// Negotiated returns true if this RtcDataChannel was negotiated by the application, or false otherwise.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-negotiated" />
	public abstract bool Negotiated { get; }

	/// <summary>
	/// The Id returns the id for this RtcDataChannel. The id was either assigned by the user agent at channel creation
	/// time or selected by the script.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-id" />
	public abstract ushort? Id { get; }

	/// <summary>
	/// ReadyState represents the state of the RtcDataChannel object.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-readystate" />
	public abstract RtcDataChannelState ReadyState { get; }

	/// <summary>
	/// The BufferedAmount returns the number of bytes of application data (UTF-8 text and binary data) that have been
	/// queued using <see cref="Send(IEnumerable{byte})">Send</see> but that, as of the last time the event loop started
	/// executing a task, had not yet been transmitted to the network. (This thus includes any text sent during the
	/// execution of the current task, regardless of whether the user agent is able to transmit text asynchronously with
	/// script execution.) This does not include framing overhead incurred by the protocol, or buffering done by the
	/// operating system or network hardware. If the channel is closed, this BufferedAmount value will only increase with
	/// each call to the <see cref="Send(IEnumerable{byte})">Send()</see> method (the attribute does not reset to
	/// zero once the channel closes).
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-bufferedamount" />
	/// <seealso cref="Send(string)" />
	/// <seealso cref="Send(IEnumerable{byte})" />
	public abstract ulong BufferedAmount { get; }

	/// <summary>
	/// </summary>
	/// <seealso cref="BufferedAmount" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-RtcDataChannel-bufferedamountlowthreshold" />
	public abstract ulong BufferedAmountLowThreshold { get; set; }

	/// <summary>
	/// This BinaryType controls how binary data is exposed to scripts. See the
	/// <seealso href="http://www.w3.org/TR/websockets/">[WEBSOCKETS-API]</seealso> for more information.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-datachannel-binarytype" />
	public abstract string BinaryType { get; set; }

	/// <summary>
	/// The RtcDataChannel object's underlying data transport has been established (or re-established).
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-RtcDataChannel-onopen" />
	public abstract event EventHandler OnOpen;

	/// <summary>
	/// An error has occurred.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onerror" />
	public abstract event EventHandler<RtcErrorEventArgs> OnError;

	/// <summary>
	/// The data channel is transitioning from <see cref="RtcDataChannelState.Open" /> to
	/// <see cref="RtcDataChannelState.Closing" />.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onclosing" />
	public abstract event EventHandler OnClosing;

	/// <summary>
	/// The RtcDataChannel object's underlying data transport has been closed.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onclose" />
	public abstract event EventHandler OnClose;

	/// <summary>
	/// A message was successfully received.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onmessage" />
	public abstract event EventHandler<MessageEventArgs> OnMessage;

	/// <summary>
	/// The RTCDataChannel object's <see cref="BufferedAmount" /> decreases from above its
	/// <see cref="BufferedAmountLowThreshold" /> to less than or equal to its <see cref="BufferedAmountLowThreshold" />.
	/// </summary>
	/// <seealso cref="BufferedAmountLowThreshold" />
	/// <seealso cref="BufferedAmount" />
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-onbufferedamountlow" />
	public abstract event EventHandler OnBufferedAmountLow;

	/// <summary>
	/// Closes the RtcDataChannel. It may be called regardless of whether the RtcDataChannel object was created by this
	/// peer or the remote peer.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-close" />
	public abstract void Close();

	/// <summary>
	/// Send string data through the data channel to a peer.
	/// </summary>
	/// <param name="data"></param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
	public abstract void Send(string data);

	/// <summary>
	/// Send byte data through the data channel to a peer.
	/// </summary>
	/// <param name="data">An enumeration of bytes to send to the peer.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
	public abstract void Send(IEnumerable<byte> data);

	/// <summary>
	/// Send byte data through the data channel to a peer.
	/// </summary>
	/// <param name="data">An array of bytes to send to the peer.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
	public abstract void Send(byte[] data);

	/// <summary>
	/// Send byte data through the data channel to a peer.
	/// </summary>
	/// <param name="data">A segment of bytes to send to the peer.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
	public virtual void Send(ArraySegment<byte> data)
	{
		if (data.Array is null)
			throw new ArgumentException("ArraySegment must reference a backing array.", nameof(data));

		if (data.Offset == 0 && data.Count == data.Array.Length)
		{
			Send(data.Array);
			return;
		}

		var payload = new byte[data.Count];
		Array.Copy(data.Array, data.Offset, payload, 0, data.Count);
		Send(payload);
	}

	/// <summary>
	/// Send byte data through the data channel to a peer.
	/// </summary>
	/// <param name="data">A read-only collection of bytes to send to the peer.</param>
	/// <seealso href="https://www.w3.org/TR/webrtc/#dom-rtcdatachannel-send" />
	public virtual void Send(IReadOnlyList<byte> data)
	{
		if (data is null)
			throw new ArgumentNullException(nameof(data));

		if (data is byte[] bytes)
		{
			Send(bytes);
			return;
		}

		var payload = new byte[data.Count];
		for (var i = 0; i < data.Count; i++)
			payload[i] = data[i];

		Send(payload);
	}
}

/// <summary>
/// Arguments for the <see cref="RtcDataChannel.OnMessage" /> event.
/// </summary>
/// <seealso cref="RtcDataChannel.OnMessage" />
public class MessageEventArgs : EventArgs
{
	/// <summary>
	/// Initializes message event arguments raised by <see cref="RtcDataChannel.OnMessage" />.
	/// </summary>
	/// <param name="data">The received message payload.</param>
	/// <param name="origin">The origin associated with the message.</param>
	/// <param name="lastEventId">The last event identifier associated with the message.</param>
	public MessageEventArgs(object data, string origin, string lastEventId)
	{
		Data = data;
		Origin = origin;
		LastEventId = lastEventId;
	}

	/// <summary>
	/// Gets the received message payload.
	/// </summary>
	public object Data { get; }

	/// <summary>
	/// Gets the origin associated with the message.
	/// </summary>
	public string Origin { get; }

	/// <summary>
	/// Gets the last event identifier associated with the message.
	/// </summary>
	public string LastEventId { get; }

	//public IEnumerable<MessagePort> Ports;
}