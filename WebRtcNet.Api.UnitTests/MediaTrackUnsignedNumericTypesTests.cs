using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class MediaTrackUnsignedNumericTypesTests
{
	[Test]
	public void MediaTrackConstraints_UsesUintForConstrainULongMembers()
	{
		Assert.That(typeof(MediaTrackConstraints).GetProperty(nameof(MediaTrackConstraints.Width))!.PropertyType,
			Is.EqualTo(typeof(MediaTrackConstraints.PositiveUIntRangeConstraint)));
		Assert.That(typeof(MediaTrackConstraints).GetProperty(nameof(MediaTrackConstraints.Height))!.PropertyType,
			Is.EqualTo(typeof(MediaTrackConstraints.PositiveUIntRangeConstraint)));
		Assert.That(typeof(MediaTrackConstraints).GetProperty(nameof(MediaTrackConstraints.SampleRate))!.PropertyType,
			Is.EqualTo(typeof(MediaTrackConstraints.PositiveUIntRangeConstraint)));
		Assert.That(typeof(MediaTrackConstraints).GetProperty(nameof(MediaTrackConstraints.SampleSize))!.PropertyType,
			Is.EqualTo(typeof(MediaTrackConstraints.PositiveUIntRangeConstraint)));
		Assert.That(typeof(MediaTrackConstraints).GetProperty(nameof(MediaTrackConstraints.ChannelCount))!.PropertyType,
			Is.EqualTo(typeof(MediaTrackConstraints.PositiveUIntRangeConstraint)));
	}

	[Test]
	public void MediaTrackCapabilities_UsesUintValueRangesForConstrainULongMembers()
	{
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.Width))!.PropertyType,
			Is.EqualTo(typeof(ValueRange<uint>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.Height))!.PropertyType,
			Is.EqualTo(typeof(ValueRange<uint>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.SampleRate))!.PropertyType,
			Is.EqualTo(typeof(ValueRange<uint>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.SampleSize))!.PropertyType,
			Is.EqualTo(typeof(ValueRange<uint>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.ChannelCount))!.PropertyType,
			Is.EqualTo(typeof(ValueRange<uint>)));
	}

	[Test]
	public void MediaTrackSettings_UsesUintForConstrainULongMembers()
	{
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.Width))!.PropertyType,
			Is.EqualTo(typeof(uint)));
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.Height))!.PropertyType,
			Is.EqualTo(typeof(uint)));
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.SampleRate))!.PropertyType,
			Is.EqualTo(typeof(uint)));
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.SampleSize))!.PropertyType,
			Is.EqualTo(typeof(uint)));
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.ChannelCount))!.PropertyType,
			Is.EqualTo(typeof(uint)));
	}
}