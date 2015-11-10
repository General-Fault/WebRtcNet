#pragma once

WEBRTCNET_START

public ref class RtcIceCandidate
{
public:
	property String ^ Candidate { String ^ get(); void set(String ^ value); }
	property String ^ SdpMid { String ^ get(); void set(String ^ value); }
	property Nullable<UInt16> SdpLineIndex { Nullable<UInt16> get(); void set(Nullable<UInt16> value); }

	String^ ToString() override;
};

WEBRTCNET_END