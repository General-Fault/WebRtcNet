using System;

namespace WebRtcNet.Media;

public partial class MediaTrackConstraints
{
	/// <summary>
	/// Generic ideal/exact constraint container for value types.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainbooleanparameters" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainedouble" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainulong" />
	public class Constraint<T> where T : struct
	{
		/// <summary>
		/// Preferred value.
		/// </summary>
		public T? Ideal;

		/// <summary>
		/// Required exact value.
		/// </summary>
		public T? Exact;

		/// <summary>
		/// Gets whether this constraint contains a required exact value.
		/// </summary>
		public bool IsRequired => Exact.HasValue;

		/// <summary>
		/// Creates a constraint with an exact value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public Constraint(T value)
		{
			Exact = value;
		}

		/// <summary>
		/// Converts a constraint to a scalar value by preferring <see cref="Exact" /> and then <see cref="Ideal" />.
		/// </summary>
		/// <param name="from">Constraint to convert.</param>
		/// <returns>Resolved scalar value.</returns>
		public static implicit operator T(Constraint<T> from)
		{
			return from.Exact ?? from.Ideal.GetValueOrDefault();
		}

		/// <summary>
		/// Converts a scalar value to an exact-value constraint.
		/// </summary>
		/// <param name="from">Scalar value.</param>
		/// <returns>Constraint containing the value as <see cref="Exact" />.</returns>
		public static implicit operator Constraint<T>(T from)
		{
			return new Constraint<T>(from);
		}
	}

	/// <summary>
	/// String constraint container representing preferred and exact values.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constraindomstringparameters" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constraindomstring" />
	public class StringConstraint
	{
		/// <summary>
		/// Preferred string value.
		/// </summary>
		public string? Ideal;

		/// <summary>
		/// Required exact string value.
		/// </summary>
		public string? Exact;

		/// <summary>
		/// Gets whether this constraint contains a required exact value.
		/// </summary>
		public bool IsRequired => Exact is not null;

		/// <summary>
		/// Creates a string constraint with an exact value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public StringConstraint(string value)
		{
			Exact = value;
		}

		/// <summary>
		/// Converts a string constraint to a scalar string by preferring <see cref="Exact" /> and then <see cref="Ideal" />.
		/// </summary>
		/// <param name="from">Constraint to convert.</param>
		/// <returns>Resolved string value.</returns>
		public static implicit operator string(StringConstraint from)
		{
			return from.Exact ?? from.Ideal ?? string.Empty;
		}

		/// <summary>
		/// Converts a string to an exact-value string constraint.
		/// </summary>
		/// <param name="from">String value.</param>
		/// <returns>Constraint containing the value as <see cref="Exact" />.</returns>
		public static implicit operator StringConstraint(string from)
		{
			return new StringConstraint(from);
		}
	}

	/// <summary>
	/// Generic ranged constraint supporting ideal, exact, minimum, and maximum bounds.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainulongrange" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainedoublerange" />
	public class RangeConstraint<T> where T : struct, IComparable<T>
	{
		private T? _ideal;
		private T? _exact;
		private T? _min;
		private T? _max;

		/// <summary>
		/// Preferred value.
		/// </summary>
		public T? Ideal
		{
			get => _ideal;
			set
			{
				_ideal = value;
				ValidateState();
			}
		}

		/// <summary>
		/// Required exact value.
		/// </summary>
		public T? Exact
		{
			get => _exact;
			set
			{
				_exact = value;
				ValidateState();
			}
		}

		/// <summary>
		/// Inclusive minimum bound.
		/// </summary>
		public T? Min
		{
			get => _min;
			set
			{
				_min = value;
				ValidateState();
			}
		}

		/// <summary>
		/// Inclusive maximum bound.
		/// </summary>
		public T? Max
		{
			get => _max;
			set
			{
				_max = value;
				ValidateState();
			}
		}

		/// <summary>
		/// Gets whether this constraint contains required bounds or an exact value.
		/// </summary>
		public bool IsRequired => Exact.HasValue || Min.HasValue || Max.HasValue;

		/// <summary>
		/// Creates an empty range constraint.
		/// </summary>
		public RangeConstraint()
		{
		}

		/// <summary>
		/// Creates a range constraint with an exact value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public RangeConstraint(T value)
		{
			Exact = value;
		}

		/// <summary>
		/// Creates a range constraint from a simple ideal/exact constraint container.
		/// </summary>
		/// <param name="value">Source constraint.</param>
		public RangeConstraint(Constraint<T> value)
		{
			Exact = value.Exact;
			Ideal = value.Ideal;
		}

		/// <summary>
		/// Creates a range constraint using minimum, maximum, and ideal values.
		/// </summary>
		/// <param name="min">Inclusive minimum bound.</param>
		/// <param name="max">Inclusive maximum bound.</param>
		/// <param name="ideal">Preferred value.</param>
		public RangeConstraint(T min, T max, T ideal)
		{
			Min = min;
			Max = max;
			Ideal = ideal;
		}

		/// <summary>
		/// Creates a range constraint from a capability value range.
		/// </summary>
		/// <param name="valueRange">Value range source.</param>
		public RangeConstraint(ValueRange<T> valueRange)
		{
			Min = valueRange.Min;
			Max = valueRange.Max;
			if (Max.Equals(Min))
				Exact = Max;
		}

		/// <summary>
		/// Converts the constraint to a scalar by preferring exact, then ideal, then bounds.
		/// </summary>
		/// <param name="from">Constraint to convert.</param>
		/// <returns>Resolved scalar value.</returns>
		public static implicit operator T(RangeConstraint<T> from)
		{
			return from.Exact ?? from.Ideal ?? from.Min ?? from.Max ?? default(T);
		}

		/// <summary>
		/// Converts a scalar value to an exact-value range constraint.
		/// </summary>
		/// <param name="from">Scalar value.</param>
		/// <returns>Range constraint containing the value as <see cref="Exact" />.</returns>
		public static implicit operator RangeConstraint<T>(T from)
		{
			return new RangeConstraint<T>(from);
		}

		/// <summary>
		/// Validates a single value assignment for a specific property.
		/// </summary>
		/// <param name="value">Value being validated.</param>
		/// <param name="propertyName">Name of the property being validated.</param>
		protected virtual void ValidateValue(T value, string propertyName)
		{
		}

		/// <summary>
		/// Validates the current constraint state, including value-level rules and cross-property bounds consistency.
		/// </summary>
		protected void ValidateState()
		{
			if (Min.HasValue)
				ValidateValue(Min.Value, nameof(Min));

			if (Max.HasValue)
				ValidateValue(Max.Value, nameof(Max));

			if (Min.HasValue && Max.HasValue && Min.Value.CompareTo(Max.Value) > 0)
				throw new ArgumentOutOfRangeException(nameof(Min), "Min must be less than or equal to Max.");

			if (Exact.HasValue)
			{
				ValidateValue(Exact.Value, nameof(Exact));
				ValidateWithinBounds(Exact.Value, nameof(Exact));
			}

			if (Ideal.HasValue)
			{
				ValidateValue(Ideal.Value, nameof(Ideal));
				ValidateWithinBounds(Ideal.Value, nameof(Ideal));
			}
		}

		private void ValidateWithinBounds(T value, string propertyName)
		{
			if (Min.HasValue && value.CompareTo(Min.Value) < 0)
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} cannot be less than Min.");

			if (Max.HasValue && value.CompareTo(Max.Value) > 0)
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} cannot be greater than Max.");
		}
	}

	/// <summary>
	/// Unsigned integer range constraint.
	/// </summary>
	/// <remarks>
	/// This base unsigned range type accepts zero and larger values. Use
	/// <see cref="PositiveUIntRangeConstraint" /> for strictly positive semantics.
	/// </remarks>
	public class UIntRangeConstraint : RangeConstraint<uint>
	{
		/// <summary>
		/// Creates an empty unsigned range constraint.
		/// </summary>
		public UIntRangeConstraint()
		{
		}

		/// <summary>
		/// Creates a constraint with an exact unsigned value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public UIntRangeConstraint(uint value)
			: base(value)
		{
		}

		/// <summary>
		/// Creates a constraint from min/max/ideal values.
		/// </summary>
		/// <param name="min">Inclusive minimum bound.</param>
		/// <param name="max">Inclusive maximum bound.</param>
		/// <param name="ideal">Preferred value.</param>
		public UIntRangeConstraint(uint min, uint max, uint ideal)
			: base(min, max, ideal)
		{
		}

		/// <summary>
		/// Creates a constraint from a value range.
		/// </summary>
		/// <param name="valueRange">Value range source.</param>
		public UIntRangeConstraint(ValueRange<uint> valueRange)
			: base(valueRange)
		{
		}

		/// <summary>
		/// Converts an unsigned value to an exact-value unsigned range constraint.
		/// </summary>
		/// <param name="from">Unsigned value.</param>
		/// <returns>Constraint containing the value as <see cref="RangeConstraint{T}.Exact" />.</returns>
		public static implicit operator UIntRangeConstraint(uint from)
		{
			return new UIntRangeConstraint(from);
		}
	}

	/// <summary>
	/// Unsigned integer range constraint that disallows zero.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainulongrange" />
	public sealed class PositiveUIntRangeConstraint : UIntRangeConstraint
	{
		/// <summary>
		/// Creates a strictly positive unsigned range constraint.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="UIntRangeConstraint" />, this variant rejects 0 and requires values to be
		/// greater than or equal to 1.
		/// </remarks>
		public PositiveUIntRangeConstraint()
		{
		}

		/// <summary>
		/// Creates a strictly positive unsigned range constraint with an exact value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public PositiveUIntRangeConstraint(uint value)
			: base(value)
		{
		}

		/// <summary>
		/// Creates a strictly positive unsigned range constraint with bounds and an ideal value.
		/// </summary>
		/// <param name="min">Inclusive minimum bound.</param>
		/// <param name="max">Inclusive maximum bound.</param>
		/// <param name="ideal">Preferred value.</param>
		public PositiveUIntRangeConstraint(uint min, uint max, uint ideal)
			: base(min, max, ideal)
		{
		}

		/// <summary>
		/// Creates a strictly positive unsigned range constraint from a value range.
		/// </summary>
		/// <param name="valueRange">Value range source.</param>
		public PositiveUIntRangeConstraint(ValueRange<uint> valueRange)
			: base(valueRange)
		{
		}

		/// <summary>
		/// Validates values for strictly positive semantics in addition to base unsigned validation.
		/// </summary>
		/// <param name="value">Value being validated.</param>
		/// <param name="propertyName">Name of the property being validated.</param>
		protected override void ValidateValue(uint value, string propertyName)
		{
			base.ValidateValue(value, propertyName);
			if (value < 1)
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be greater than or equal to 1.");
		}

		/// <summary>
		/// Converts an unsigned value to an exact-value strictly positive unsigned range constraint.
		/// </summary>
		/// <param name="from">Unsigned value.</param>
		/// <returns>Constraint containing the value as <see cref="RangeConstraint{T}.Exact" />.</returns>
		public static implicit operator PositiveUIntRangeConstraint(uint from)
		{
			return new PositiveUIntRangeConstraint(from);
		}
	}

	/// <summary>
	/// Double precision range constraint with finite-number checks.
	/// </summary>
	public class DoubleRangeConstraint : RangeConstraint<double>
	{
		private readonly bool _forbidInfinity;
		private readonly bool _forbidNaN;

		/// <summary>
		/// Creates an empty double range constraint.
		/// </summary>
		/// <param name="forbidInfinity">Rejects positive/negative infinity when <see langword="true" />.</param>
		/// <param name="forbidNaN">Rejects NaN when <see langword="true" />.</param>
		public DoubleRangeConstraint(bool forbidInfinity = true, bool forbidNaN = true)
		{
			_forbidInfinity = forbidInfinity;
			_forbidNaN = forbidNaN;
		}

		/// <summary>
		/// Creates a constraint with an exact double value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		/// <param name="forbidInfinity">Rejects positive/negative infinity when <see langword="true" />.</param>
		/// <param name="forbidNaN">Rejects NaN when <see langword="true" />.</param>
		public DoubleRangeConstraint(double value, bool forbidInfinity = true, bool forbidNaN = true)
		{
			_forbidInfinity = forbidInfinity;
			_forbidNaN = forbidNaN;
			Exact = value;
		}

		/// <summary>
		/// Creates a constraint from min/max/ideal double values.
		/// </summary>
		/// <param name="min">Inclusive minimum bound.</param>
		/// <param name="max">Inclusive maximum bound.</param>
		/// <param name="ideal">Preferred value.</param>
		/// <param name="forbidInfinity">Rejects positive/negative infinity when <see langword="true" />.</param>
		/// <param name="forbidNaN">Rejects NaN when <see langword="true" />.</param>
		public DoubleRangeConstraint(double min, double max, double ideal, bool forbidInfinity = true,
			bool forbidNaN = true)
		{
			_forbidInfinity = forbidInfinity;
			_forbidNaN = forbidNaN;
			Min = min;
			Max = max;
			Ideal = ideal;
		}

		/// <summary>
		/// Creates a constraint from a double value range.
		/// </summary>
		/// <param name="valueRange">Value range source.</param>
		/// <param name="forbidInfinity">Rejects positive/negative infinity when <see langword="true" />.</param>
		/// <param name="forbidNaN">Rejects NaN when <see langword="true" />.</param>
		public DoubleRangeConstraint(ValueRange<double> valueRange, bool forbidInfinity = true, bool forbidNaN = true)
		{
			_forbidInfinity = forbidInfinity;
			_forbidNaN = forbidNaN;
			Min = valueRange.Min;
			Max = valueRange.Max;
			if (Max.Equals(Min))
				Exact = Max;
		}

		/// <summary>
		/// Validates finite-number requirements (NaN/Infinity) for this constraint type.
		/// </summary>
		/// <param name="value">Value being validated.</param>
		/// <param name="propertyName">Name of the property being validated.</param>
		protected override void ValidateValue(double value, string propertyName)
		{
			if (_forbidNaN && double.IsNaN(value))
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} cannot be NaN.");

			if (_forbidInfinity && double.IsInfinity(value))
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} cannot be Infinity.");
			base.ValidateValue(value, propertyName);
		}

		/// <summary>
		/// Converts a double value to an exact-value double range constraint.
		/// </summary>
		/// <param name="from">Double value.</param>
		/// <returns>Constraint containing the value as <see cref="RangeConstraint{T}.Exact" />.</returns>
		public static implicit operator DoubleRangeConstraint(double from)
		{
			return new DoubleRangeConstraint(from);
		}
	}

	/// <summary>
	/// Double range constraint that requires values to be strictly greater than zero.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainedoublerange" />
	public sealed class PositiveDoubleRangeConstraint : DoubleRangeConstraint
	{
		/// <summary>
		/// Creates a strictly positive double range constraint.
		/// </summary>
		/// <remarks>
		/// Values must be strictly greater than 0.
		/// </remarks>
		public PositiveDoubleRangeConstraint()
		{
		}

		/// <summary>
		/// Creates a strictly positive double range constraint with an exact value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public PositiveDoubleRangeConstraint(double value)
			: base(value)
		{
		}

		/// <summary>
		/// Creates a strictly positive double range constraint with bounds and an ideal value.
		/// </summary>
		/// <param name="min">Inclusive minimum bound.</param>
		/// <param name="max">Inclusive maximum bound.</param>
		/// <param name="ideal">Preferred value.</param>
		public PositiveDoubleRangeConstraint(double min, double max, double ideal)
			: base(min, max, ideal)
		{
		}

		/// <summary>
		/// Creates a strictly positive double range constraint from a value range.
		/// </summary>
		/// <param name="valueRange">Value range source.</param>
		public PositiveDoubleRangeConstraint(ValueRange<double> valueRange)
			: base(valueRange)
		{
		}

		/// <summary>
		/// Validates values for strictly positive semantics in addition to base double validation.
		/// </summary>
		/// <param name="value">Value being validated.</param>
		/// <param name="propertyName">Name of the property being validated.</param>
		protected override void ValidateValue(double value, string propertyName)
		{
			base.ValidateValue(value, propertyName);
			if (value <= 0.0)
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be greater than 0.");
		}

		/// <summary>
		/// Converts a double to an exact-value strictly positive double range constraint.
		/// </summary>
		/// <param name="from">Double value.</param>
		/// <returns>Constraint containing the value as <see cref="RangeConstraint{T}.Exact" />.</returns>
		public static implicit operator PositiveDoubleRangeConstraint(double from)
		{
			return new PositiveDoubleRangeConstraint(from);
		}
	}

	/// <summary>
	/// Double range constraint that allows zero but disallows negative values.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainedoublerange" />
	public sealed class NonNegativeDoubleRangeConstraint : DoubleRangeConstraint
	{
		/// <summary>
		/// Creates a non-negative double range constraint.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="PositiveDoubleRangeConstraint" />, this variant allows 0.
		/// </remarks>
		public NonNegativeDoubleRangeConstraint()
		{
		}

		/// <summary>
		/// Creates a non-negative double range constraint with an exact value.
		/// </summary>
		/// <param name="value">Exact value.</param>
		public NonNegativeDoubleRangeConstraint(double value)
			: base(value)
		{
		}

		/// <summary>
		/// Creates a non-negative double range constraint with bounds and an ideal value.
		/// </summary>
		/// <param name="min">Inclusive minimum bound.</param>
		/// <param name="max">Inclusive maximum bound.</param>
		/// <param name="ideal">Preferred value.</param>
		public NonNegativeDoubleRangeConstraint(double min, double max, double ideal)
			: base(min, max, ideal)
		{
		}

		/// <summary>
		/// Creates a non-negative double range constraint from a value range.
		/// </summary>
		/// <param name="valueRange">Value range source.</param>
		public NonNegativeDoubleRangeConstraint(ValueRange<double> valueRange)
			: base(valueRange)
		{
		}

		/// <summary>
		/// Validates values for non-negative semantics in addition to base double validation.
		/// </summary>
		/// <param name="value">Value being validated.</param>
		/// <param name="propertyName">Name of the property being validated.</param>
		protected override void ValidateValue(double value, string propertyName)
		{
			base.ValidateValue(value, propertyName);
			if (value < 0.0)
				throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be greater than or equal to 0.");
		}

		/// <summary>
		/// Converts a double to an exact-value non-negative double range constraint.
		/// </summary>
		/// <param name="from">Double value.</param>
		/// <returns>Constraint containing the value as <see cref="RangeConstraint{T}.Exact" />.</returns>
		public static implicit operator NonNegativeDoubleRangeConstraint(double from)
		{
			return new NonNegativeDoubleRangeConstraint(from);
		}
	}
}