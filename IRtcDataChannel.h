#pragma once

WEBRTCNET_START

public ref class RtcDataChannelInit
{
	property Boolean Ordered { Boolean get() { throw gcnew NotImplementedException(); } void set(Boolean value) { throw gcnew NotImplementedException(); } }
	property UInt32 MaxPacketLifeTime { UInt32 get() { throw gcnew NotImplementedException(); } void set(UInt32 value) { throw gcnew NotImplementedException(); } }
	property UInt32 MaxRetransmits { UInt32 get() { throw gcnew NotImplementedException(); } void set(UInt32 value) { throw gcnew NotImplementedException(); } }
	property String ^ Protocol { String ^ get() { throw gcnew NotImplementedException(); } void set(String ^ value) { throw gcnew NotImplementedException(); } }
	property Boolean Negotiated { Boolean get() { throw gcnew NotImplementedException(); } void set(Boolean value) { throw gcnew NotImplementedException(); } }
	property UInt32 Id { UInt32 get() { throw gcnew NotImplementedException(); } void set(UInt32 value) { throw gcnew NotImplementedException(); } }
};

public enum RtcDataChannelState 
{
	/// Attempting to establish the underlying data transport. 
	/// This is the initial state of a RTCDataChannel object created with createDataChannel().
	Connecting,

	/// The underlying data transport is established and communication is possible. 
	/// This is the initial state of a RTCDataChannel object dispatched as a part of a RTCDataChannelEvent.
	Open,

	/// The procedure to close down the underlying data transport has started.
	Closing,

	/// The underlying data transport has been closed or could not be established.
	Closed
};

public ref class MessageEventArgs : EventArgs
{
	property Object ^ Data { Object ^ get() { throw gcnew NotImplementedException(); }  }
	property String ^ Origin { String ^ get() { throw gcnew NotImplementedException(); } }
	property String ^ LastEventId { String ^ get() { throw gcnew NotImplementedException(); } }
	//property IEnumerable<MessagePort ^> ^ Ports { IEnumerable<MessagePort ^> ^ get() { throw gcnew NotImplementedException(); } }
};

public interface class IRtcDataChannel
{
	/// The Label represents a label that can be used to distinguish this RTCDataChannel object from other 
	/// RTCDataChannel objects. Applications are allowed to create multiple RTCDataChannel objects with the same label. 
	property String ^ Label { String ^ get(); }
	
	/// Ordered returns true if the RTCDataChannel is ordered, and false if other of order delivery is allowed. 
	property Boolean Ordered { Boolean get(); }

	/// MaxPacketLifeTime returns the length of the time window (in milliseconds) during which transmissions and 
	/// retransmissions may occur in unreliable mode, or null if unset. 
	property Nullable<UInt32> MaxPacketLifeTime { Nullable<UInt32> get(); }
	
	/// MaxRetransmits returns the maximum number of retransmissions that are attempted in unreliable mode, 
	/// or null if unset. 
	property Nullable<UInt32> MaxRetransmits { Nullable<UInt32> get(); }
	
	/// Protocol returns the name of the sub-protocol used with this RTCDataChannel if any, or the empty string otherwise. 
	property String ^ Protocol { String ^ get(); }
	
	/// Negotiated returns true if this RTCDataChannel was negotiated by the application, or false otherwise. 
	property Boolean Negotiated { Boolean get(); }

	/// The Id returns the id for this RTCDataChannel. The id was either assigned by the user agent at channel 
	/// creation time or selected by the script. 
	property UInt32 Id { UInt32 get(); }
	
	/// ReadyState represents the state of the RTCDataChannel object. 
	property RtcDataChannelState ReadyState { RtcDataChannelState get(); }
	
	/// The BufferedAmount returns the number of bytes of application data (UTF-8 text and binary data) that have 
	/// been queued using Send() but that, as of the last time the event loop started executing a task, had not yet 
	/// been transmitted to the network. (This thus includes any text sent during the execution of the current task, 
	/// regardless of whether the user agent is able to transmit text asynchronously with script execution.) 
	/// This does not include framing overhead incurred by the protocol, or buffering done by the operating system or 
	/// network hardware. If the channel is closed, this BufferedAmount value will only increase with each call to the Send() 
	/// method (the attribute does not reset to zero once the channel closes).
	property UInt32 BufferedAmount { UInt32 get(); }
	
	/// The BufferedAmountLowThreshold gets or sets the threshold at which the bufferedAmount is considered to be low. 
	/// When the bufferedAmount decreases from above this threshold to equal or below it, the bufferedamountlow event fires. 
	/// The bufferedAmountLowThreshold is initially zero on each new RTCDataChannel, but the application may change its value at any time.
	property UInt32 BufferedAmountLowThreshold { UInt32 get(); void set(UInt32 value); }

	/// This BinaryType controls how binary data is exposed to scripts. See the [WEBSOCKETS-API] for more information.
	property String ^ BinaryType { String ^ get(); void set(String ^ value); }

	/// The RTCDataChannel object's underlying data transport has been established (or re-established).
	event EventHandler ^ OnOpen;

	/// Fired when the RTCDataChannel object's BufferedAmount decreases from above its BufferedAmountLowThreshold 
	/// to less than or equal to its BufferedAmountLowThreshold
	event EventHandler ^ OnBufferedAmountLow;


	event EventHandler ^ OnError;

	/// The RTCDataChannel object's underlying data transport has been closed.
	event EventHandler ^ OnClose;

	/// A message was successfully received.
	event EventHandler<MessageEventArgs ^> ^ OnMessage;

	/// Closes the RTCDataChannel. It may be called regardless of whether the RTCDataChannel object was created by this peer or the remote peer.
	void Close();

	void Send(String ^ data);
	void Send(array<Byte> ^ data);
};

WEBRTCNET_END