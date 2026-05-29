using System;
using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// Provides data for the <see cref="IMediaDevices.OnDeviceChange" /> event.
/// Carries the updated list of available devices and a hint about which devices were
/// recently physically inserted or activated by the user.
/// </summary>
/// <remarks>
/// Corresponds to the <c>DeviceChangeEvent</c> Web IDL interface and the
/// <c>DeviceChangeEventInit</c> dictionary. The <see cref="UserInsertedDevices" /> list
/// was added in a later revision of the spec; implementations that have not been updated
/// to support it will supply an empty list.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-devicechangeevent" />
public sealed class DeviceChangeEventArgs : EventArgs
{
	/// <summary>
	/// Initialises a new instance with the full device list and an optional user-inserted
	/// device subset.
	/// </summary>
	/// <param name="devices">
	/// All media input and output devices available at the time of the event. Must not be
	/// <see langword="null" />.
	/// </param>
	/// <param name="userInsertedDevices">
	/// The subset of <paramref name="devices" /> that the user physically inserted or
	/// activated immediately before the event fired. Pass <see langword="null" /> or an
	/// empty list when the implementation does not supply this information.
	/// </param>
	public DeviceChangeEventArgs(
		IReadOnlyList<MediaDeviceInfo> devices,
		IReadOnlyList<MediaDeviceInfo>? userInsertedDevices = null)
	{
		Devices = devices ?? Array.Empty<MediaDeviceInfo>();
		UserInsertedDevices = userInsertedDevices ?? Array.Empty<MediaDeviceInfo>();
	}

	/// <summary>
	/// The complete list of media input and output devices available at the time the event
	/// was fired.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-devicechangeevent-devices" />
	public IReadOnlyList<MediaDeviceInfo> Devices { get; }

	/// <summary>
	/// The subset of <see cref="Devices" /> that the user physically inserted or activated
	/// recently, and that are newly exposed as a result of this event. This list is empty
	/// when no such hint is available.
	/// </summary>
	/// <remarks>
	/// Every entry in this list is also present in <see cref="Devices" />.
	/// Applications can use this list to disambiguate a user inserting a device (strong
	/// signal of intent) from other reasons the device list may change.
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-devicechangeevent-userinserteddevices" />
	public IReadOnlyList<MediaDeviceInfo> UserInsertedDevices { get; }
}