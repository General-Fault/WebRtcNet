#pragma once

WEBRTCNET_START
public ref class MediaTrackCapabilities
{
public:
	property Int32 Width;
	property Int32 Height;
	property Double AspectRatio;
	property Double FrameRate;
	property String^ FacingMode;
	property Double Volume;
	property Int32 SampleRate;
	property Int32 SampleSize;
	property IEnumerable<Boolean>^ EchoCancellation;
			 
	property Double Latency;
	property Int32 ChannelCount;
	property String^ DeviceId;
	property String^ GroupId;
};

WEBRTCNET_END