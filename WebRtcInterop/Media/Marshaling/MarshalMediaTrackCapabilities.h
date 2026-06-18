#pragma once

#include <msclr/marshal.h>

#include "../../Marshaling/MarshalMedia.h"

namespace msclr::interop
{
	/// <summary>
	/// Marshals a nullable value to a ValueRange&lt;T&gt; by setting both Min and Max to that value.
	/// </summary>
	template <typename T>
	inline WebRtcNet::ValueRange<T>^ MarshalToValueRange(System::Nullable<T> value)
	{
		auto range = gcnew WebRtcNet::ValueRange<T>();
		if (value.HasValue)
		{
			range->Min = value;
			range->Max = value;
		}
		return range;
	}

	/// <summary>
	/// Marshals a (min, max) pair to a ValueRange&lt;T&gt;.
	/// </summary>
	template <typename T>
	inline WebRtcNet::ValueRange<T>^ MarshalToValueRange(System::Nullable<T> min, System::Nullable<T> max)
	{
		auto range = gcnew WebRtcNet::ValueRange<T>();
		range->Min = min;
		range->Max = max;
		return range;
	}

}
