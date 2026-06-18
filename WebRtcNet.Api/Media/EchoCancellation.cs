using System;

namespace WebRtcNet.Media;

/// <summary>
/// Named echo-cancellation modes indicating which pipeline performs the cancellation.
/// </summary>
/// <remarks>
/// <para>
/// These modes are mutually exclusive: <see cref="Software"/> opens the audio device in raw
/// (unprocessed) mode and runs WebRTC's AEC3 algorithm in the application layer, while
/// <see cref="System"/> opens the device in default mode and relies on the OS audio engine or
/// driver to apply echo cancellation before samples reach the application.
/// </para>
/// <para>
/// Running both simultaneously causes artifacts because each canceller assumes it is the only
/// one in the chain. A boolean constraint value of <see langword="true"/> attempts
/// <see cref="System"/> first and falls back to <see cref="Software"/> if the OS does not
/// provide echo cancellation for the selected device.
/// </para>
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation" />
public enum EchoCancellationMode
{
	/// <summary>
	/// Echo cancellation is performed by WebRTC's software APM (AEC3) in the application layer.
	/// The audio device is opened in raw (unprocessed) mode, bypassing OS audio effects.
	/// Always available regardless of hardware or OS capabilities.
	/// </summary>
	Software,

	/// <summary>
	/// Echo cancellation is performed by the OS audio engine or audio driver.
	/// The audio device is opened in default mode, which allows the OS to apply its own
	/// processing pipeline. Availability is device- and driver-dependent.
	/// </summary>
	System
}

/// <summary>
/// Represents a single echo-cancellation value, either a boolean toggle or a mode string.
/// </summary>
/// <remarks>
/// This models the spec's <c>boolean or DOMString</c> union while preserving forward compatibility
/// for unknown future mode strings.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#def-constraint-echoCancellation" />
public readonly record struct EchoCancellationValue
{
	private readonly bool? _booleanValue;
	private readonly string _modeValue;

	/// <summary>
	/// Creates a boolean echo-cancellation value.
	/// </summary>
	/// <param name="value">
	/// The requested or reported boolean state. When used as a constraint, <see langword="true"/>
	/// attempts <see cref="EchoCancellationMode.System"/> first and falls back to
	/// <see cref="EchoCancellationMode.Software"/> if OS-level echo cancellation is unavailable
	/// for the selected device.
	/// </param>
	public EchoCancellationValue(bool value)
	{
		_booleanValue = value;
		_modeValue = string.Empty;
	}

	/// <summary>
	/// Creates a mode-based echo-cancellation value from a known mode enum member.
	/// </summary>
	/// <param name="mode">The standardized echo-cancellation mode.</param>
	public EchoCancellationValue(EchoCancellationMode mode)
	{
		_booleanValue = null;
		_modeValue = mode switch
		{
			EchoCancellationMode.Software => "software",
			EchoCancellationMode.System => "system",
			_ => throw new ArgumentOutOfRangeException(nameof(mode))
		};
	}

	/// <summary>
	/// Creates a mode-based echo-cancellation value from a raw mode string.
	/// </summary>
	/// <param name="mode">
	/// The mode value. Known values include <c>all</c> and <c>remote-only</c>. Unknown non-empty
	/// values are preserved for forward compatibility.
	/// </param>
	public EchoCancellationValue(string mode)
	{
		if (mode is null) throw new ArgumentNullException(nameof(mode));
		if (string.IsNullOrWhiteSpace(mode)) throw new ArgumentException("Mode value must not be empty.", nameof(mode));

		_booleanValue = null;
		_modeValue = mode;
	}

	/// <summary>
	/// Returns <see langword="true" /> when this value is represented as a boolean.
	/// </summary>
	public bool IsBoolean => _booleanValue.HasValue;

	/// <summary>
	/// Returns <see langword="true" /> when this value is represented as a mode string.
	/// </summary>
	public bool IsMode => !string.IsNullOrEmpty(_modeValue);

	/// <summary>
	/// Gets the boolean value when <see cref="IsBoolean" /> is <see langword="true" />;
	/// otherwise <see langword="null" />.
	/// </summary>
	public bool? BooleanValue => _booleanValue;

	/// <summary>
	/// Gets the raw mode string when <see cref="IsMode" /> is <see langword="true" />;
	/// otherwise <see langword="null" />.
	/// </summary>
	public string ModeValue => _modeValue ?? string.Empty;

	/// <summary>
	/// Gets the parsed known mode when <see cref="ModeValue" /> is a standard mode; otherwise
	/// <see langword="null" />.
	/// </summary>
	public EchoCancellationMode? Mode
		=> ModeValue switch
		{
			"software" => EchoCancellationMode.Software,
			"system" => EchoCancellationMode.System,
			_ => null
		};

	/// <summary>
	/// Returns the normalized serialized representation used by the spec.
	/// </summary>
	/// <returns>
	/// <c>true</c>/<c>false</c> for boolean values, the raw mode string for mode values, or
	/// <see cref="string.Empty" /> for an unspecified default instance.
	/// </returns>
	public override string ToString()
	{
		if (_booleanValue.HasValue) return _booleanValue.Value ? bool.TrueString : bool.FalseString;

		return ModeValue;
	}

	/// <summary>
	/// Converts a boolean to an <see cref="EchoCancellationValue" />.
	/// </summary>
	/// <param name="from">Boolean echo-cancellation value.</param>
	/// <returns>An <see cref="EchoCancellationValue" /> with boolean representation.</returns>
	public static implicit operator EchoCancellationValue(bool from)
	{
		return new EchoCancellationValue(from);
	}

	/// <summary>
	/// Converts a known mode enum value to an <see cref="EchoCancellationValue" />.
	/// </summary>
	/// <param name="from">Known echo-cancellation mode.</param>
	/// <returns>An <see cref="EchoCancellationValue" /> with mode representation.</returns>
	public static implicit operator EchoCancellationValue(EchoCancellationMode from)
	{
		return new EchoCancellationValue(from);
	}

	/// <summary>
	/// Converts a raw mode string to an <see cref="EchoCancellationValue" />.
	/// </summary>
	/// <param name="from">Mode string value.</param>
	/// <returns>An <see cref="EchoCancellationValue" /> with mode representation.</returns>
	public static implicit operator EchoCancellationValue(string from)
	{
		return new EchoCancellationValue(from);
	}
}

/// <summary>
/// Constraint container for echo cancellation, including optional exact and ideal values.
/// </summary>
/// <remarks>
/// Mirrors the spec's <c>ConstrainBooleanOrDOMString</c> behavior for a strongly typed C# API.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-constrainbooleanordomstring" />
public sealed class EchoCancellationConstraint
{
	/// <summary>
	/// Initializes an empty constraint where both <see cref="Ideal" /> and <see cref="Exact" />
	/// are unspecified.
	/// </summary>
	public EchoCancellationConstraint()
	{
	}

	/// <summary>
	/// The preferred target value.
	/// </summary>
	public EchoCancellationValue? Ideal { get; set; }

	/// <summary>
	/// The required exact value.
	/// </summary>
	public EchoCancellationValue? Exact { get; set; }

	/// <summary>
	/// Initializes a constraint with an exact value.
	/// </summary>
	/// <param name="value">The required value.</param>
	public EchoCancellationConstraint(EchoCancellationValue value)
	{
		Exact = value;
	}

	/// <summary>
	/// Converts a value into a constraint with <see cref="Exact" /> set.
	/// </summary>
	/// <param name="from">Echo-cancellation value.</param>
	/// <returns>An <see cref="EchoCancellationConstraint" /> with <see cref="Exact" /> populated.</returns>
	public static implicit operator EchoCancellationConstraint(EchoCancellationValue from)
	{
		return new EchoCancellationConstraint(from);
	}

	/// <summary>
	/// Converts a boolean into a constraint with <see cref="Exact" /> set.
	/// </summary>
	/// <param name="from">Boolean echo-cancellation value.</param>
	/// <returns>An <see cref="EchoCancellationConstraint" /> with <see cref="Exact" /> populated.</returns>
	public static implicit operator EchoCancellationConstraint(bool from)
	{
		return new EchoCancellationConstraint(from);
	}

	/// <summary>
	/// Converts a known mode into a constraint with <see cref="Exact" /> set.
	/// </summary>
	/// <param name="from">Known echo-cancellation mode.</param>
	/// <returns>An <see cref="EchoCancellationConstraint" /> with <see cref="Exact" /> populated.</returns>
	public static implicit operator EchoCancellationConstraint(EchoCancellationMode from)
	{
		return new EchoCancellationConstraint(from);
	}

	/// <summary>
	/// Converts a raw mode string into a constraint with <see cref="Exact" /> set.
	/// </summary>
	/// <param name="from">Mode string value.</param>
	/// <returns>An <see cref="EchoCancellationConstraint" /> with <see cref="Exact" /> populated.</returns>
	public static implicit operator EchoCancellationConstraint(string from)
	{
		return new EchoCancellationConstraint(from);
	}
}