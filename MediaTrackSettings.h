#pragma once

WEBRTCNET_START

public ref class MediaTrackSettings
{
public:
	MediaTrackSettings() { }

	property Int32 Width;
	property Int32 Height;
	property Double AspectRatio;
	property Double FrameRate;
	property String ^ facingMode;
	property Double Volume;
	property Int32 SampleRate;
	property Int32 SampleSize;
	property Boolean EchoCancellation;
	property Double Latency;
	property Int32 ChannelCount;
	property String ^ deviceId;
	property String ^ groupId;
};

WEBRTCNET_END