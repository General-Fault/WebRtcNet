#pragma once

namespace WebRtcInterop
{
#include <api/scoped_refptr.h>

	template <typename T>
	public ref class NativeWrapper abstract
	{
	public:
		NativeWrapper(T* native)
			: rp_native_(new webrtc::scoped_refptr<T>(native))
		{
		}

		NativeWrapper(webrtc::scoped_refptr<T> rp_native)
			: rp_native_(rp_native)
		{
		}

		~NativeWrapper() { this->!NativeWrapper(); }

		T* Get() { return rp_native_; }

		explicit operator bool() { return rp_native_ != nullptr && rp_native_->get() != nullptr; }
		T* operator->() { return rp_native_; }

	internal:
		!NativeWrapper()
		{
			if (rp_native_ != nullptr)
			{
				delete rp_native_; //calls release;
				rp_native_ = nullptr;
			}
		}

	private:
		webrtc::scoped_refptr<T>* rp_native_;
	};
}
