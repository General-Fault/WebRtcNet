#pragma once

namespace rtc
{
	template <class T>
	class scoped_refptr;
}

namespace WebRtcInterop
{
	using namespace rtc;

	template <class T>
	private ref class ManagedScopedRefPtr sealed
	{
	public:
		using element_type = T;

		ManagedScopedRefPtr() : ptr_(nullptr)
		{}

		ManagedScopedRefPtr(std::nullptr_t) : ptr_(nullptr)
		{} 

		explicit ManagedScopedRefPtr(T* p) : ptr_(p)
		{
			if (ptr_)
				ptr_->AddRef();
		}

		ManagedScopedRefPtr(const ManagedScopedRefPtr<T>% r) : ptr_(r.Get())
		{
			if (ptr_)
				ptr_->AddRef();
		}

		~ManagedScopedRefPtr()
		{
			(*this).!ManagedScopedRefPtr();
		}

		explicit operator bool() { return ptr_ != nullptr; }

		//void swap(ManagedScopedRefPtr<T>% r)  { swap(&r.ptr_); }


		//template <typename U>
		//static bool operator==(const ManagedScopedRefPtr<T>% a,
		//                const ManagedScopedRefPtr<U>% b)
		//{
		//	return a.Get() == b.get();
		//}

		//template <typename U>
		//static bool operator!=(const ManagedScopedRefPtr<T>% a,
		//                const ManagedScopedRefPtr<U>% b)
		//{
		//	return !(a == b);
		//}

		//static bool operator==(const ManagedScopedRefPtr<T>% a, std::nullptr_t)
		//{
		//	return a.Get() == nullptr;
		//}

		//static bool operator!=(const ManagedScopedRefPtr<T>% a, std::nullptr_t)
		//{
		//	return !(a == nullptr);
		//}

		//static bool operator==(std::nullptr_t, const ManagedScopedRefPtr<T>% a)
		//{
		//	return a.Get() == nullptr;
		//}

		//static bool operator!=(std::nullptr_t, const ManagedScopedRefPtr<T>% a)
		//{
		//	return !(a == nullptr);
		//}

		//// Comparison with raw pointer.
		//template <typename U>
		//static bool operator==(const ManagedScopedRefPtr<U>^ a, const U* b)
		//{
		//	return a.Get() == b;
		//}

		//template <typename U>
		//static bool operator!=(const ManagedScopedRefPtr<U> a, const U* b)
		//{
		//	return !(a == b);
		//}

		//// Ordered comparison, needed for use as a std::map key.
		//template <typename U>
		//static bool operator<(const ManagedScopedRefPtr<U>^ a, const ManagedScopedRefPtr<U>% b)
		//{
		//	return a.Get() < b.Get();
		//}

	internal:
		T* Get() { return ptr_; }
		//static T& operator*(ManagedScopedRefPtr<T>^ a) { return *a->Get(); }
		T* operator->() { return ptr_; }

		// Returns the (possibly null) raw pointer, and makes the ManagedScopedRefPtr hold a
		// null pointer, all without touching the reference count of the underlying
		// pointed-to object. The object is still reference counted, and the caller of
		// release() is now the proud owner of one reference, so it is responsible for
		// calling Release() once on the object when no longer using it.
		T* Release()
		{
			T* retVal = ptr_;
			ptr_ = nullptr;
			return retVal;
		}

		void operator=(T* p)
		{
			// AddRef first so that self assignment should work
			if (p)
				p->AddRef();
			if (ptr_)
				ptr_->Release();
			ptr_ = p;
			//return *this;
		}

		void Swap(T** pp)
		{
			T* p = ptr_;
			ptr_ = *pp;
			*pp = p;
		}

		ManagedScopedRefPtr(const scoped_refptr<T>& r) : ptr_(r.get())
		{
			if (ptr_)
				ptr_->AddRef();
		}


		!ManagedScopedRefPtr()
		{
			if (ptr_)
				ptr_->Release();

			ptr_ = nullptr;
		}

	protected:
		T* ptr_;
	};
}
