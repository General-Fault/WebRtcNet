namespace WebRtcNet;

/// <summary>
/// Represents a minimum/maximum range for values of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The value type represented by the range.</typeparam>
public class ValueRange<T> where T : struct
{
	/// <summary>
	/// The upper bound of the range.
	/// </summary>
	public T Max;

	/// <summary>
	/// The lower bound of the range.
	/// </summary>
	public T Min;

	/// <summary>
	/// Initializes a range where <see cref="Min" /> and <see cref="Max" /> are equal to <paramref name="value" />.
	/// </summary>
	/// <param name="value">The single value for both bounds.</param>
	public ValueRange(T value)
	{
		Min = Max = value;
	}

	/// <summary>
	/// Initializes a range with independent minimum and maximum bounds.
	/// </summary>
	/// <param name="min">The lower bound.</param>
	/// <param name="max">The upper bound.</param>
	public ValueRange(T min, T max)
	{
		Min = min;
		Max = max;
	}

	/// <summary>
	/// Converts a range to its <see cref="Max" /> value.
	/// </summary>
	/// <param name="from">The range instance.</param>
	/// <returns>The range's upper bound.</returns>
	public static implicit operator T(ValueRange<T> from)
	{
		return from.Max;
	}

	/// <summary>
	/// Converts a single value to a range where both bounds are that value.
	/// </summary>
	/// <param name="from">The source value.</param>
	/// <returns>A range with equal min/max bounds.</returns>
	public static implicit operator ValueRange<T>(T from)
	{
		return new ValueRange<T>(from);
	}
}