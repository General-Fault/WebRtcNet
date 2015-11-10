#pragma once

WEBRTCNET_START

ref class MediaTrackSupportedConstraints
{
public:

	MediaTrackSupportedConstraints()
	{
	}

	property Boolean Width;

	property Boolean Height;
	property Boolean AspectRatio;
	property Boolean FrameRate;
	property Boolean FacingMode;
	property Boolean Volume;
	property Boolean SampleRate;
	property Boolean SampleSize;
	property Boolean EchoCancellation;
	property Boolean Latency;
	property Boolean ChannelCount;
	property Boolean DeviceId;
	property Boolean GroupId;
};

WEBRTCNET_END