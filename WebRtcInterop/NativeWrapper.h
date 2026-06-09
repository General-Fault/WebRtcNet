#pragma once

namespace WebRtcInterop
{
#include <api/scoped_refptr.h>

	template <typename T>
	public ref class NativeWrapper
	{
	public:
		NativeWrapper(T* native)
			: rp_native_(new webrtc::scoped_refptr<T>(native))
		{
		}

		NativeWrapper(webrtc::scoped_refptr<T> rp_native)
			: rp_native_(new webrtc::scoped_refptr<T>(rp_native))
		{
		}

		~NativeWrapper() { this->!NativeWrapper(); }

		bool HasValue() { return rp_native_ != nullptr && rp_native_->get() != nullptr; }
		T* Get() { return rp_native_ == nullptr ? nullptr : rp_native_->get(); }
		webrtc::scoped_refptr<T> GetScopedRef() { return rp_native_ == nullptr ? webrtc::scoped_refptr<T>() : *rp_native_; }

		explicit operator bool() { return HasValue(); }

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
