using System;

namespace WebRtcNet.Media;

/// <summary>
/// Represents a single facing-mode value, preserving unknown raw strings for forward compatibility.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-videofacingmodeenum" />
public readonly record struct VideoFacingModeValue
{
	private readonly string _rawValue;

	/// <summary>
	/// Creates a facing-mode value from a known enum value.
	/// </summary>
	/// <param name="value">Known facing-mode value.</param>
	public VideoFacingModeValue(VideoFacingModes value)
	{
		_rawValue = value switch
		{
			VideoFacingModes.User => "user",
			VideoFacingModes.Environment => "environment",
			VideoFacingModes.Left => "left",
			VideoFacingModes.Right => "right",
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		};
	}

	/// <summary>
	/// Creates a facing-mode value from a raw string.
	/// </summary>
	/// <param name="value">Raw facing-mode string value.</param>
	public VideoFacingModeValue(string value)
	{
		if (value is null) throw new ArgumentNullException(nameof(value));
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Facing mode value must not be empty.", nameof(value));

		_rawValue = value;
	}

	/// <summary>
	/// Gets whether <see cref="RawValue" /> maps to a known enum value.
	/// </summary>
	public bool IsKnown => KnownValue.HasValue;

	/// <summary>
	/// Gets the known enum value when recognized; otherwise <see langword="null" />.
	/// </summary>
	public VideoFacingModes? KnownValue => RawValue switch
	{
		"user" => VideoFacingModes.User,
		"environment" => VideoFacingModes.Environment,
		"left" => VideoFacingModes.Left,
		"right" => VideoFacingModes.Right,
		_ => null
	};

	/// <summary>
	/// Gets the raw string representation of this value.
	/// </summary>
	public string RawValue => _rawValue ?? string.Empty;

	/// <summary>
	/// Returns the serialized facing-mode string.
	/// </summary>
	public override string ToString()
	{
		return RawValue;
	}

	/// <summary>
	/// Converts a known enum value to a <see cref="VideoFacingModeValue" />.
	/// </summary>
	public static implicit operator VideoFacingModeValue(VideoFacingModes from)
	{
		return new VideoFacingModeValue(from);
	}

	/// <summary>
	/// Converts a raw string value to a <see cref="VideoFacingModeValue" />.
	/// </summary>
	public static implicit operator VideoFacingModeValue(string from)
	{
		return new VideoFacingModeValue(from);
	}
}

/// <summary>
/// Represents a single resize-mode value, preserving unknown raw strings for forward compatibility.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-videoresizemodeenum" />
public readonly record struct VideoResizeModeValue
{
	private readonly string _rawValue;

	/// <summary>
	/// Creates a resize-mode value from a known enum value.
	/// </summary>
	/// <param name="value">Known resize-mode value.</param>
	public VideoResizeModeValue(VideoResizeModes value)
	{
		_rawValue = value switch
		{
			VideoResizeModes.None => "none",
			VideoResizeModes.CropAndScale => "crop-and-scale",
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		};
	}

	/// <summary>
	/// Creates a resize-mode value from a raw string.
	/// </summary>
	/// <param name="value">Raw resize-mode string value.</param>
	public VideoResizeModeValue(string value)
	{
		if (value is null) throw new ArgumentNullException(nameof(value));
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Resize mode value must not be empty.", nameof(value));

		_rawValue = value;
	}

	/// <summary>
	/// Gets whether <see cref="RawValue" /> maps to a known enum value.
	/// </summary>
	public bool IsKnown => KnownValue.HasValue;

	/// <summary>
	/// Gets the known enum value when recognized; otherwise <see langword="null" />.
	/// </summary>
	public VideoResizeModes? KnownValue => RawValue switch
	{
		"none" => VideoResizeModes.None,
		"crop-and-scale" => VideoResizeModes.CropAndScale,
		_ => null
	};

	/// <summary>
	/// Gets the raw string representation of this value.
	/// </summary>
	public string RawValue => _rawValue ?? string.Empty;

	/// <summary>
	/// Returns the serialized resize-mode string.
	/// </summary>
	public override string ToString()
	{
		return RawValue;
	}

	/// <summary>
	/// Converts a known enum value to a <see cref="VideoResizeModeValue" />.
	/// </summary>
	public static implicit operator VideoResizeModeValue(VideoResizeModes from)
	{
		return new VideoResizeModeValue(from);
	}

	/// <summary>
	/// Converts a raw string value to a <see cref="VideoResizeModeValue" />.
	/// </summary>
	public static implicit operator VideoResizeModeValue(string from)
	{
		return new VideoResizeModeValue(from);
	}
}
