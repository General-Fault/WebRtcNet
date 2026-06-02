using System.Collections.Generic;
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
	public void MediaTrackCapabilities_UsesCollectionForFacingMode()
	{
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.FacingMode))!.PropertyType,
			Is.EqualTo(typeof(IReadOnlyList<VideoFacingModes>)));
	}

	[Test]
	public void MediaTrackCapabilities_UsesCollectionForResizeMode()
	{
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.ResizeMode))!.PropertyType,
			Is.EqualTo(typeof(IReadOnlyList<VideoResizeModes>)));
	}

	[Test]
	public void MediaTrackCapabilities_UsesCollectionsForAllSequenceValuedMembers()
	{
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.EchoCancellation))!.PropertyType,
			Is.EqualTo(typeof(IReadOnlyList<EchoCancellationValue>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.BackgroundBlur))!.PropertyType,
			Is.EqualTo(typeof(IReadOnlyList<bool>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.AutoGainControl))!.PropertyType,
			Is.EqualTo(typeof(IReadOnlyList<bool>)));
		Assert.That(typeof(MediaTrackCapabilities).GetProperty(nameof(MediaTrackCapabilities.NoiseSuppression))!.PropertyType,
			Is.EqualTo(typeof(IReadOnlyList<bool>)));
	}

	[Test]
	public void MediaTrackCapabilities_DoesNotExposeLegacyScalarSequenceAdapters()
	{
		Assert.That(typeof(MediaTrackCapabilities).GetProperty("FacingModeValue"), Is.Null);
		Assert.That(typeof(MediaTrackCapabilities).GetProperty("ResizeModeValue"), Is.Null);
		Assert.That(typeof(MediaTrackCapabilities).GetProperty("EchoCancellationValue"), Is.Null);
		Assert.That(typeof(MediaTrackCapabilities).GetProperty("BackgroundBlurValue"), Is.Null);
		Assert.That(typeof(MediaTrackCapabilities).GetProperty("AutoGainControlValue"), Is.Null);
		Assert.That(typeof(MediaTrackCapabilities).GetProperty("NoiseSuppressionValue"), Is.Null);
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

	[Test]
	public void MediaTrackSettings_UsesNullableMembersForOptionalResizeAndAudioProcessingFlags()
	{
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.ResizeMode))!.PropertyType,
			Is.EqualTo(typeof(VideoResizeModes?)));
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.AutoGainControl))!.PropertyType,
			Is.EqualTo(typeof(bool?)));
		Assert.That(typeof(MediaTrackSettings).GetProperty(nameof(MediaTrackSettings.NoiseSuppression))!.PropertyType,
			Is.EqualTo(typeof(bool?)));
	}
}