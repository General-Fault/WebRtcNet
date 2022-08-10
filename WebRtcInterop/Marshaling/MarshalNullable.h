#pragma once

#include "absl/types/optional.h"

namespace msclr::interop
{
	using namespace System;

	/**
	 * \brief Copy a native optional value to a managed Nullable
	 * \tparam TManaged The managed type represented by the Nullable
	 * \tparam TNative The native type represented by the optional.
	 * \param from The optional value to be marshaled into a managed Nullable value type.
	 * \return A managed nullable marshaled from the native optional.
	 */
	template <class TManaged, class TNative>
	Nullable<TManaged> marshal_as(const absl::optional<TNative>& from)
	{
		Nullable<TManaged> to;

		if (from.has_value())
		{
			if constexpr (std::is_convertible_v<TNative, TManaged>)
			{
				to = from.value();
			}
			else
			{
				to = marshal_as<TManaged>(from.value());
			}
		}
		return to;
	};


	/**
	 * \brief Copy a managed Nullable to a native optional.
	 * \tparam TNative The native type represented by the optional.
	 * \tparam TManaged The managed type represented by the Nullable
	 * \param from The Nullable value to be marshaled into a native optional.
	 * \return A native optional marshaled from the managed Nullable;
	 */
	template <class TNative, class TManaged>
	absl::optional<TNative> marshal_as(Nullable<TManaged> from)
	{
		if (from.HasValue)
		{
			if constexpr (std::is_convertible_v<TManaged, TNative>)
			{
				
				return safe_cast<TNative>(from.Value);
			}
			else
			{
				return absl::optional<TNative>(marshal_as<TNative>(from.Value));
			}
		}
		return absl::optional<TNative>();
	};
}