#pragma once

WEBRTCNET_START

public value struct ConstrainLong
{
	/// The maximum legal value of this property.
	property Nullable<Int32> Max;

	/// The minimum value of this property.
	property Nullable<Int32> Min;

	/// The exact required value for this property.
	property Nullable<Int32> Exact;

	/// The ideal (target) value for this property.
	property Nullable<Int32> Ideal;
};


public value struct ConstrainDouble
{
	/// The maximum legal value of this property.
	property Nullable<Double> Max;

	/// The minimum value of this property.
	property Nullable<Double> Min;

	/// The exact required value for this property.
	property Nullable<Double> Exact;

	/// The ideal (target) value for this property.
	property Nullable<Double> Ideal;
};


public value struct ConstrainBoolean
{
	/// The exact required value for this property.
	property Nullable<Boolean> Exact;

	/// The ideal (target) value for this property.
	property Nullable<Boolean> Ideal;
};


public value struct ConstrainString
{
	/// The exact required value for this property.
	property IEnumerable<String^>^ Exact;

	/// The ideal (target) value for this property.
	property IEnumerable<String^>^ Ideal;
};


public ref class MediaTrackConstraintSet
{
	property ConstrainLong Width;
	property ConstrainLong Height;
	property ConstrainDouble AspectRatio;
	property ConstrainDouble FrameRate;
	property ConstrainString FacingMode;
	property ConstrainDouble Volume;
	property ConstrainLong SampleRate;
	property ConstrainLong SampleSize;
	property ConstrainBoolean EchoCancellation;
	property ConstrainDouble Latency;
	property ConstrainLong ChannelCount;
	property ConstrainString DeviceId;
	property ConstrainString GroupId;
};


public ref class MediaTrackConstraints : MediaTrackConstraintSet
{
public:
	MediaTrackConstraints();
	virtual ~MediaTrackConstraints();

	property IList<MediaTrackConstraintSet^>^ Advanced;
};


WEBRTCNET_END