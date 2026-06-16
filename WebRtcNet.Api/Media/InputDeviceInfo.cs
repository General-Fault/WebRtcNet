namespace WebRtcNet.Media;

/// <summary>
/// The InputDeviceInfo interface gives access to the capabilities of the input device it represents.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-inputdeviceinfo"/>
public sealed record InputDeviceInfo : MediaDeviceInfo
{
	internal InputDeviceInfo(string deviceId, MediaDeviceKind kind, string label, string groupId)
		: base(deviceId, kind, label, groupId)
	{
	}

	/// <summary>
	/// Returns a MediaTrackCapabilities object describing the primary audio or video track of a device's MediaStream 
	/// (according to its kind value), in the absence of any user-supplied constraints.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-inputdeviceinfo-getcapabilities"/>
	public MediaTrackCapabilities GetCapabilities()
	{
		return MediaTrackCapabilities.Create(
			deviceId: DeviceId,
			groupId: GroupId);
	}

	/// <summary>
	/// Factory method for creating InputDeviceInfo instances (for interop use only).
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public new static InputDeviceInfo Create(string deviceId, MediaDeviceKind kind, string label, string groupId)
		=> new(deviceId, kind, label, groupId);
}