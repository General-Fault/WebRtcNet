#pragma once

#include "..\RtcDataChannel.h"

#include "talk\app\webrtc\datachannelinterface.h"
#include "webrtc\base\scoped_ref_ptr.h"

#include <msclr\auto_gcroot.h>
using namespace msclr;

WebRtcObservers_Start

private class DataChannelObserver : public webrtc::DataChannelObserver
{
public:
	// The data channel state have changed.
	virtual void OnStateChange();

	//  A data buffer was successfully received.
	virtual void OnMessage(const webrtc::DataBuffer & buffer);

	// The data channel's buffered_amount has changed.
	virtual void OnBufferedAmountChange(uint64_t previous_amount);

	DataChannelObserver( WebRtcInterop::RtcDataChannel ^ dataChannel, webrtc::DataChannelInterface * nativeDataChannel );
	~DataChannelObserver();

	gcroot<WebRtcInterop::RtcDataChannel ^> _dataChannel;
	rtc::scoped_refptr<webrtc::DataChannelInterface> _nativeDataChannel;
};

WebRtcObservers_End