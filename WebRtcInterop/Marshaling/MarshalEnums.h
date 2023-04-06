#pragma once

#include <map>

template <typename native_type, typename managed_type>
managed_type marshal_mapped_native_type(const std::map<const native_type, const managed_type>& map, const native_type& from)
{
	auto entry = map.find(from);
	if (entry == map.end())
	{
		throw gcnew System::InvalidCastException(System::String::Format("Unable to convert {0} value '{1}' to {2}",
		                                                native_type::typeid->FullName, static_cast<int>(from), managed_type::typeid->FullName));
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

	throw gcnew System::InvalidCastException(System::String::Format("Unable to convert {0} value '{1}' to {2}",
	                                                managed_type::typeid->FullName, Enum::GetName(managed_type::typeid, from),
	                                                native_type::typeid->FullName));
}
