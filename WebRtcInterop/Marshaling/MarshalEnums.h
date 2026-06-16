#pragma once

#include <map>

template <typename native_type, typename managed_type>
managed_type marshal_mapped_native_type(const std::map<const native_type, const managed_type>& map, const native_type& from)
{
	auto entry = map.find(from);
	if (entry == map.end())
	{
		throw gcnew System::InvalidCastException("Unable to convert native value to managed type.");
	}

	return entry->second;
}

template <typename managed_type, typename native_type>
native_type marshal_mapped_managed_type(const std::map<const native_type, const managed_type>& map, const managed_type% from)
{
	for (auto [key, value] : map)
	{
		if (value == from) return key;
	}

	throw gcnew System::InvalidCastException("Unable to convert managed value to native type.");
}
