using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class DeviceChangeEventArgsTests
{
	private static MediaDeviceInfo MakeDevice(string id, MediaDeviceKind kind, string label = "")
	{
		// Use reflection to invoke the internal constructor.
		var ctor = typeof(MediaDeviceInfo).GetConstructor(
			BindingFlags.NonPublic | BindingFlags.Instance,
			null,
			[typeof(string), typeof(MediaDeviceKind), typeof(string), typeof(string)],
			null)!;
		return (MediaDeviceInfo)ctor.Invoke([id, kind, label, string.Empty]);
	}

	[Test]
	public void DeviceChangeEventArgs_Devices_ReturnsSuppliedList()
	{
		var devices = new List<MediaDeviceInfo>
		{
			MakeDevice("d1", MediaDeviceKind.AudioInput),
			MakeDevice("d2", MediaDeviceKind.VideoInput)
		};

		var args = new DeviceChangeEventArgs(devices);

		Assert.That(args.Devices, Is.EquivalentTo(devices));
	}

	[Test]
	public void DeviceChangeEventArgs_UserInsertedDevices_DefaultsToEmpty()
	{
		var args = new DeviceChangeEventArgs([]);

		Assert.That(args.UserInsertedDevices, Is.Empty);
	}

	[Test]
	public void DeviceChangeEventArgs_UserInsertedDevices_NullDefaultsToEmpty()
	{
		var args = new DeviceChangeEventArgs([]);

		Assert.That(args.UserInsertedDevices, Is.Empty);
	}

	[Test]
	public void DeviceChangeEventArgs_UserInsertedDevices_ReturnsSuppliedSubset()
	{
		var d1 = MakeDevice("d1", MediaDeviceKind.AudioInput);
		var d2 = MakeDevice("d2", MediaDeviceKind.VideoInput);
		var args = new DeviceChangeEventArgs([d1, d2], [d1]);

		Assert.That(args.UserInsertedDevices, Has.Count.EqualTo(1));
		Assert.That(args.UserInsertedDevices[0].DeviceId, Is.EqualTo("d1"));
	}

	[Test]
	public void DeviceChangeEventArgs_NullDevices_DefaultsToEmpty()
	{
		var args = new DeviceChangeEventArgs(null!);

		Assert.That(args.Devices, Is.Not.Null);
		Assert.That(args.Devices, Is.Empty);
	}

	[Test]
	public void DeviceChangeEventArgs_IsEventArgs()
	{
		Assert.That(typeof(EventArgs).IsAssignableFrom(typeof(DeviceChangeEventArgs)), Is.True);
	}

	[Test]
	public void MediaDevices_OnDeviceChange_IsTypedEventHandler()
	{
		var eventInfo = typeof(MediaDevices).GetEvent(nameof(MediaDevices.OnDeviceChange))!;

		Assert.That(eventInfo, Is.Not.Null);
		Assert.That(
			eventInfo.EventHandlerType,
			Is.EqualTo(typeof(EventHandler<DeviceChangeEventArgs>)));
	}
}