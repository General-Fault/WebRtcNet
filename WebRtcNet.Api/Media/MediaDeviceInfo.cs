using System.ComponentModel;

namespace WebRtcNet.Media;

/// <summary>
///     Represents a media device's basic usage.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevicekind" />
public enum MediaDeviceKind
{
	/// <summary>
	/// Represents an audio input device, such as a microphone.
	/// </summary>
	AudioInput,

	/// <summary>
	/// Represents an audio output device, such as speakers or headphones.
	/// </summary>
	AudioOutput,

	/// <summary>
	/// Represents a video input device, such as a camera.
	/// </summary>
	VideoInput
}

/// <summary>
///     Represents information about a single media device such as a webcam, speakers or a microphone.
///     This is an out-only snapshot returned by <see cref="MediaDevices.EnumerateDevices" />.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadeviceinfo" />
public record MediaDeviceInfo
{
	internal MediaDeviceInfo(string deviceId, MediaDeviceKind kind, string label, string groupId)
	{
		DeviceId = deviceId ?? string.Empty;
		Kind = kind;
		Label = label ?? string.Empty;
		GroupId = groupId ?? string.Empty;
	}

	/// <summary>
	///     The identifier of the represented device.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadeviceinfo-deviceid" />
	public string DeviceId { get; }

	/// <summary>
	///     The <see cref="MediaDeviceKind">kind</see> of the represented device.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadeviceinfo-kind" />
	public MediaDeviceKind Kind { get; }

	/// <summary>
	///     A label describing this device (for example "External USB Webcam"). This label is intended to allow the end user
	///     to tell the difference between devices. Applications can't assume that the label contains any specific
	///     information, such as the device type or model
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadeviceinfo-label" />
	public string Label { get; }

	/// <summary>
	///     The group identifier of the represented device. Two devices have the same group identifier if they belong to the
	///     same physical device. For example, the audio input and output devices representing the speaker and microphone of
	///     the same headset have the same groupId.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadeviceinfo-groupid" />
	public string GroupId { get; }

	/// <summary>
	///     Factory method for creating MediaDeviceInfo instances (for interop use only).
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static MediaDeviceInfo Create(string deviceId, MediaDeviceKind kind, string label, string groupId)
		=> new(deviceId, kind, label, groupId);
}