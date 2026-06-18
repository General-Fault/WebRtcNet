using System;

namespace WebRtcNet.Media;

/// <summary>
/// The InputDeviceInfo interface gives access to the capabilities of the input device it represents.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-inputdeviceinfo"/>
public sealed record InputDeviceInfo : MediaDeviceInfo
{
	private readonly Func<MediaTrackCapabilities>? _getCapabilities;

	internal InputDeviceInfo(string deviceId, MediaDeviceKind kind, string label, string groupId,
		Func<MediaTrackCapabilities>? getCapabilities = null)
		: base(deviceId, kind, label, groupId)
	{
		_getCapabilities = getCapabilities;
	}

	/// <summary>
	/// Returns a <see cref="MediaTrackCapabilities"/> object describing the primary audio or video track of a
	/// device's MediaStream (according to its kind value), in the absence of any user-supplied constraints.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-inputdeviceinfo-getcapabilities"/>
	public MediaTrackCapabilities GetCapabilities()
	{
		if (_getCapabilities is not null)
			return _getCapabilities();

		return MediaTrackCapabilities.Create(
			deviceId: DeviceId,
			groupId: GroupId);
	}

	/// <summary>
	/// Factory method for creating InputDeviceInfo instances (for interop use only).
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public new static InputDeviceInfo Create(string deviceId, MediaDeviceKind kind, string label, string groupId,
		Func<MediaTrackCapabilities>? getCapabilities = null)
		=> new(deviceId, kind, label, groupId, getCapabilities);
}