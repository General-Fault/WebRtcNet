#pragma once

#include <map>

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

namespace msclr::interop
{
	using namespace System;
	using namespace Collections::Generic;

	/**
	 * \brief Marshal a native std::pair into a managed KeyValuePair.
	 * \tparam TManagedKey The managed type of the key in the KeyValuePair to be marshaled from the native pair first value.
	 * \tparam TManagedValue The managed type of the value in the KeyValuePair to be marshaled from the native pair second value.
	 * \tparam TNativeFirst The native type of the first value in the native pair.
	 * \tparam TNativeSecond The native type of the second value in the native pair.
	 * \param from The native std::pair to marshal.
	 * \return A managed KeyValuePair containing a key and value marshaled from the native pair.
	 */
	template <typename TManagedKey, typename TManagedValue, typename TNativeFirst, typename TNativeSecond>
	KeyValuePair<TManagedKey, TManagedValue>^ marshal_as(const std::pair<TNativeFirst, TNativeSecond>& from)
	{
		auto kvp = gcnew KeyValuePair<TManagedKey, TManagedValue>();

		TManagedKey key;
		if constexpr (std::is_convertible_v<TNativeFirst, TManagedKey>)
		{
			key = from.first;
		}
		else
		{
			key = marshal_as<TManagedKey>(from.first);
		}

		TManagedValue value;
		if constexpr (std::is_convertible_v<TNativeSecond, TManagedValue>)
		{
			value = from.second;
		}
		else
		{
			value = marshal_as<TManagedValue>(from.second);
		}

		return gcnew KeyValuePair<TManagedKey, TManagedValue>(key, value);
	}

	/**
	 * \brief Marshal a managed KeyValuePair into a native pair.
	 * \tparam TNativeFirst The native type of the first value to be marshaled from the managed KeyValuePair key.
	 * \tparam TNativeSecond the native type of the second value to be marshaled from the managed KeyValuePair value.
	 * \tparam TManagedKey the managed type of the key in the managed KeyValuePair.
	 * \tparam TManagedValue the managed type of the value in the managed KeyValuePair.
	 * \param from The managed KeyValuePair to marshal.
	 * \return A native std::pair containing a first and second value marshaled from the Key and Value of the managed KeyValuePair.
	 */
	template <typename TNativeFirst, typename TNativeSecond, typename TManagedKey, typename TManagedValue>
	std::pair<TNativeFirst, TNativeSecond>& marshal_as(KeyValuePair<TManagedKey, TManagedValue> from)
	{
		std::pair<TNativeFirst, TNativeValue> pair{};
		if (from == nullptr) return pair;

		if constexpr (std::is_convertible_v<TManagedKey, TNativeFirst>)
		{
			pair.first = from->Key;
		}
		else
		{
			pair.first = marshal_as<TNativeFirst>(from->Key);
		}

		if constexpr (std::is_convertible_v<TManagedValue, TNativeSecond>)
		{
			pair.second = from->Value;
		}
		else
		{
			pair.second = marshal_as<TNativeSecond>(from->Value);
		}
		return pair;
	}

	/**
	 * \brief Marshal a std::map into an IDictionary
	 * \tparam TManagedKey The managed type of the key in the IDictionary.
	 * \tparam TManagedValue The managed type of the value in the IDictionary.
	 * \tparam TNativeKey The native type of the key in the std::map.
	 * \tparam TNativeValue the native type of the value in the std::map.
	 * \param from a std::map to marshal.
	 * \return An IDictionary std::map containing key value pairs marshaled from the IDictionary.
	 */
	template <class TManagedKey, class TManagedValue, class TNativeKey, class TNativeValue>
	IDictionary<TManagedKey, TManagedValue>^ marshal_as(const std::map<TNativeKey, TNativeValue>& from)
	{
		auto to = gcnew Dictionary<TManagedKey, TManagedValue>();
		for (auto kv : from)
		{
			auto kvp = marshal_as<TManagedKey, TManagedValue>(kv);
			to->Add(kvp->Key, kvp->Value);
		}
		return to;
	};

	/**
	 * \brief Marshal a std::map into an IDictionary
	 * \tparam TNativeKey The native type of the key in the std::map.
	 * \tparam TNativeValue the native type of the value in the std::map.
	 * \tparam TManagedKey The managed type of the key in the IDictionary.
	 * \tparam TManagedValue The managed type of the value in the IDictionary.
	 * \param from An IDictionary to marshal.
	 * \return A std::map containing key value pairs marshaled from the std::map.
	 */
	template <class TNativeKey, class TNativeValue, class TManagedKey, class TManagedValue>
	std::map<TNativeKey, TNativeValue>& marshal_as(IDictionary<TManagedKey, TManagedValue>^ from)
	{
		std::map<TNativeKey, TNativeValue> to{};
		for each (auto kv in from)
		{
			auto pair = marshal_as<TNativeKey, TNativeValue>(kv);
			to.insert(pair);
		}
		return to;
	};


	/**
	 * \brief Marshal items in a native collection into a managed list.
	 * \tparam TManaged The type contained returned managed list.
	 * \tparam TNativeCol The type of native collection.
	 * \tparam TNative The value type contained in the native collection
	 * \param from The native object to marshal as an IList<TManaged>
	 * \return An IList containing the objects in the native collection marshaled to TManaged types.
	 */
	template <typename TManaged,
	          template <typename...> class TNativeCol,
	          typename TNative,
	          typename = std::enable_if_t<!std::is_convertible_v<TNativeCol<TNative>, std::string>>>
	IList<TManaged>^ marshal_as(TNativeCol<TNative> from)
	{
		auto to = gcnew List<TManaged>();
		for (auto item : from)
		{
			TManaged managed;
			if constexpr (std::is_convertible_v<TManaged, TNative>)
			{
				managed = item;
			}
			else
			{
				managed = marshal_as<TManaged>(item);
			}
			to->Add(managed);
		}
		return to;
	}


	/**
	 * \brief Marshal items from a managed IEnumerable into a native collection.
	 * \tparam TNative The native type contained within the collection.
	 * \tparam TNativeCol The type of native collection to marshal.
	 * \tparam TManaged The type of managed object in the IEnumerable.
	 * \param from The managed IEnumerable to marshal as a TNativeCol<TNative> collection.
	 * \return An IEnumerable containing managed objects marshaled to TManaged types from the native collection.
	 */
	template <template <typename...> class TNativeCol,
	          typename TNative,
	          typename TManaged>
	TNativeCol<TNative> marshal_as(IEnumerable<TManaged>^ from)
	{
		TNativeCol<TNative> to{};
		if (from == nullptr) return to;

		for each (auto item in from)
		{
			if constexpr (std::is_convertible_v<TNative, TManaged>)
			{
				to.push_back(item);
			}
			else
			{
				to.push_back(marshal_as<TNative>(item));
			}
		}
		return to;
	};


	/**
	 * \brief Marshal items from a managed array into a native array. If the managed array is smaller than NativeArraySize, then
	 * the items at the end of the native array are uninitialized. If the managed array is longer than NativeArraySize, only
	 * the first NativeArraySize elements are marshaled.
	 * \tparam TNative The native type contained within the native array.
	 * \tparam TManaged The type of managed object in managed array.
	 * \tparam NativeArraySize The size of the native array to be returned. 
	 * \param from The managed array to marshal as a std::array<TNative, NativeArraySize> collection.
	 * \return A native std::array of size NativeArraySize, containing managed objects marshaled to TNative types from the managed array.
	 */
	template <template <typename, size_t> class TNativeArray,
	          typename TNative,
	          size_t NativeArraySize,
	          typename TManaged>
	TNativeArray<TNative, NativeArraySize> marshal_as(array<TManaged>^ from)
	{
		TNativeArray<TNative, NativeArraySize> to{};
		if (from == nullptr) return to;

		for (int i = 0; i < from->Length && i < to.size(); i++)
		{
			if constexpr (std::is_convertible_v<TNative, TManaged>)
			{
				to[i] = from[i];
			}
			else
			{
				to[i] = marshal_as<TNative>(from[i]);
			}
		}

		return to;
	};


	/**
	 * \brief Marshal items from a native std::array into a managed array of the same size.
	 * \tparam TManaged The type of managed object in managed array.
	 * \tparam TNative The native type contained within the native array.
	 * \tparam NativeArraySize The size of the native array to be marshaled.
	 * \param from The managed array to marshal as a std::array<TNative, NativeArraySize> collection.
	 * \return A native std::array of size NativeArraySize,containing managed objects marshaled to TNative types from the managed array.
	 */
	template <typename TManaged,
	          typename TNative, size_t NativeArraySize>
	array<TManaged>^ marshal_as(const std::array<TNative, NativeArraySize>& from)
	{
		auto to = gcnew array<TManaged>(NativeArraySize);

		for (int i = 0; i < from.size(); i++)
		{
			if constexpr (std::is_convertible_v<TNative, TManaged>)
			{
				to[i] = from[i];
			}
			else
			{
				to[i] = marshal_as<TManaged>(from[i]);
			}
		}

		return to;
	};
}
