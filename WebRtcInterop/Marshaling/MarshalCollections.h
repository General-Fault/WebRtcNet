#pragma once

#include <vector>
#include <map>

#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>

using namespace System::Collections::Generic;

namespace msclr {namespace interop 
{

//copy a vector to into an IEnumerable
template<class TManaged, class TNative>
inline IEnumerable<TManaged>^ marshal_vector_as(const std::vector<TNative> & from)
{
	auto to = gcnew List<TManaged>();
	for (auto item : from)
	{
		to->Add(marshal_as<TManaged>(item));
	}
	return safe_cast<IEnumerable<TManaged>^>(to);
};


//Copy a map into an IDictionary
template<class TNativeKey, class TNativeValue, class TManagedKey, class TManagedValue>
inline IDictionary<TManagedKey, TManagedValue> ^ marshal_map_as(const std::map<TNativeKey, TNativeValue> & from)
{
	auto to = gcnew Dictionary<TManagedKey, TManagedValue>();
	for (auto kv : from)
	{
		to->Insert(marshal_as<TManagedKey>(kv->first), marshal_as<TManagedValue>(kv->second));
	}
	return safe_cast<IDictionary<TManagedKey, TManagedValue>^>(to);
};

//Copy an IEnumerable into a new vector.
template<class TNative, class TManaged>
inline std::vector<TNative> marshal_enumerable_as(IEnumerable<TManaged> ^ const & from)
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