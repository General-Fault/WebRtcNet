#pragma once

#include <vector>
#include <map>

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

namespace msclr {namespace interop 
{
	//copy a vector to into an IEnumerable
	template<class TManaged, class TNative>
	inline System::Collections::Generic::IEnumerable<TManaged> ^ marshal_vector_as(const std::vector<TNative> & from)
	{
		auto to = gcnew System::Collections::Generic::List<TManaged>();
		for (auto item : from)
		{
			to->Add(marshal_as<TManaged>(item));
		}
		return safe_cast<System::Collections::Generic::IEnumerable<TManaged>^>(to);
	};


	//Copy a map into an IDictionary
	template<class TManagedKey, class TManagedValue, class TNativeKey, class TNativeValue>
	inline System::Collections::Generic::IDictionary<TManagedKey, TManagedValue> ^ marshal_map_as(const std::map<TNativeKey, TNativeValue> & from)
	{
		auto to = gcnew System::Collections::Generic::Dictionary<TManagedKey, TManagedValue>();
		for (auto kv : from)
		{
			auto key = marshal_as<TManagedKey>(kv.first);
			auto value = marshal_as<TManagedValue>(kv.second);
			to->Add(key, value);
		}
		return safe_cast<IDictionary<TManagedKey, TManagedValue>^>(to);
	};

	//Copy an IEnumerable into a new vector.
	template<class TNative, class TManaged>
	inline std::vector<TNative> marshal_enumerable_as(System::Collections::Generic::IEnumerable<TManaged> ^ const & from)
	{
		std::vector<TNative> to;

		for each(TManaged item in from)
		{
			TNative native = marshal_as<TNative>(item);
			to.push_back(native);
		}

		return to;
	};
}}