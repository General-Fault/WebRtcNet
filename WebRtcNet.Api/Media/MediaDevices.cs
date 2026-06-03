using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebRtcNet.Media;

/// <summary>
/// Represents access to media input/output devices.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#mediadevices" />
public abstract class MediaDevices
{
	/// <summary>
	/// Initializes the media devices wrapper.
	/// </summary>
	protected MediaDevices()
	{
	}

	/// <summary>
	/// Raised when the set of available media devices changes — for example, when a camera or
	/// microphone is connected or disconnected. The event args carry the updated device list and
	/// a hint about which devices were recently inserted by the user.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-ondevicechange" />
	public abstract event EventHandler<DeviceChangeEventArgs> OnDeviceChange;

	/// <summary>
	/// Collects information about the available media input and output devices.
	/// </summary>
	/// <returns>
	/// A task that, when complete, yields a list of <see cref="MediaDeviceInfo" /> objects
	/// representing each available device.
	/// </returns>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-enumeratedevices" />
	public abstract Task<IEnumerable<MediaDeviceInfo>> EnumerateDevices();

	#region 10.2 MediaDevices Interface Extensions

	/// <summary>
	/// Returns the set of constrainable properties recognised by this implementation.
	/// Applications can use this to determine which constraints can be applied reliably or to
	/// build predictable logic around advanced constraint sets.
	/// </summary>
	/// <returns>
	/// A <see cref="MediaTrackSupportedConstraints" /> instance whose members indicate which
	/// constrainable properties are supported.
	/// </returns>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-getsupportedconstraints" />
	public abstract MediaTrackSupportedConstraints GetSupportedConstraints();

	/// <summary>
	/// Requests access to media input devices and returns a <see cref="MediaStream" /> whose
	/// tracks satisfy the supplied constraints.
	/// </summary>
	/// <param name="constraints">
	/// Specifies the type and configuration of media tracks to include in the returned stream.
	/// </param>
	/// <returns>
	/// A task that, when complete, yields an <see cref="MediaStream" /> whose tracks conform
	/// to <paramref name="constraints" />.
	/// </returns>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediadevices-getusermedia" />
	public abstract Task<MediaStream> GetUserMedia(MediaStreamConstraints constraints);

	#endregion //10.2 MediaDevices Interface Extensions
}