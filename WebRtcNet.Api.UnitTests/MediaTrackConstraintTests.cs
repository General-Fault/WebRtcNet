using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class MediaTrackConstraintTests
{
	[Test]
	public void MediaTrackSupportedConstraints_Defaults_BackgroundBlur_To_False()
	{
		var constraints = new MediaTrackSupportedConstraints();

		Assert.IsFalse(constraints.BackgroundBlur);
	}

	[Test]
	public void MediaTrackConstraints_Defaults_BackgroundBlur_To_Null()
	{
		var constraints = new MediaTrackConstraints();

		Assert.IsNull(constraints.BackgroundBlur);
	}

	[Test]
	public void MediaTrackCapabilities_Defaults_BackgroundBlur_To_Empty()
	{
		var capabilities = new MediaTrackCapabilities();

		Assert.That(capabilities.BackgroundBlur, Is.Empty);
	}

	[Test]
	public void MediaTrackCapabilities_Defaults_FacingMode_To_Empty()
	{
		var capabilities = new MediaTrackCapabilities();

		Assert.That(capabilities.FacingMode, Is.Empty);
	}

	[Test]
	public void MediaTrackCapabilities_Defaults_ResizeMode_To_Empty()
	{
		var capabilities = new MediaTrackCapabilities();

		Assert.That(capabilities.ResizeMode, Is.Empty);
	}

	[Test]
	public void MediaTrackSettings_Defaults_BackgroundBlur_To_False()
	{
		var settings = new MediaTrackSettings();

		Assert.IsFalse(settings.BackgroundBlur);
	}

	[Test]
	public void MediaTrackSettings_Defaults_ResizeMode_To_Null()
	{
		var settings = new MediaTrackSettings();

		Assert.IsNull(settings.ResizeMode);
	}

	[Test]
	public void MediaTrackSettings_Defaults_FacingMode_To_Null()
	{
		var settings = new MediaTrackSettings();

		Assert.IsNull(settings.FacingMode);
	}

	[Test]
	public void MediaTrackSettings_Does_Not_Expose_Volume()
	{
		var property = typeof(MediaTrackSettings).GetProperty("Volume");

		Assert.That(property, Is.Null);
	}

	[Test]
	public void MediaTrackSettings_Defaults_AutoGainControl_To_Null()
	{
		var settings = new MediaTrackSettings();

		Assert.IsNull(settings.AutoGainControl);
	}

	[Test]
	public void MediaTrackSettings_Defaults_NoiseSuppression_To_Null()
	{
		var settings = new MediaTrackSettings();

		Assert.IsNull(settings.NoiseSuppression);
	}

	[Test]
	public void MediaTrackConstraints_Defaults_EchoCancellation_To_Null()
	{
		var constraints = new MediaTrackConstraints();

		Assert.IsNull(constraints.EchoCancellation);
	}

	[Test]
	public void MediaTrackConstraints_IdlParity_Advanced_Defaults_To_Null_When_Unset()
	{
		var constraints = new MediaTrackConstraints();

		Assert.That(constraints.Advanced, Is.Null);
	}

	[Test]
	public void MediaTrackConstraintSet_IdlParity_Does_Not_Expose_Advanced_Member()
	{
		var advancedProperty = typeof(MediaTrackConstraintSet).GetProperty(nameof(MediaTrackConstraints.Advanced));

		Assert.That(advancedProperty, Is.Null);
	}

	[Test]
	public void MediaTrackConstraints_IdlParity_Advanced_Sequence_Preserves_List_Order()
	{
		var first = new MediaTrackConstraintSet { Width = 640 };
		var second = new MediaTrackConstraintSet { Width = 1280 };
		var third = new MediaTrackConstraintSet { Width = 1920 };
		var constraints = new MediaTrackConstraints
		{
			Advanced = [first, second, third],
		};

		Assert.That(constraints.Advanced, Has.Count.EqualTo(3));
		Assert.That(constraints.Advanced![0], Is.SameAs(first));
		Assert.That(constraints.Advanced[1], Is.SameAs(second));
		Assert.That(constraints.Advanced[2], Is.SameAs(third));
	}

	[Test]
	public void MediaTrackConstraints_IdlParity_ConstraintProcessingOrder_Is_BaseThenAdvanced()
	{
		var first = new MediaTrackConstraintSet { Width = 640 };
		var second = new MediaTrackConstraintSet { Width = 1280 };
		var constraints = new MediaTrackConstraints
		{
			Width = 320,
			Advanced = [first, second],
		};

		var ordered = constraints.EnumerateConstraintSetsInProcessingOrder().ToArray();

		Assert.That(ordered, Has.Length.EqualTo(3));
		Assert.That(ordered[0], Is.SameAs(constraints));
		Assert.That(ordered[1], Is.SameAs(first));
		Assert.That(ordered[2], Is.SameAs(second));
	}

	[Test]
	public void MediaTrackConstraints_IdlParity_Advanced_Rejects_Null_ConstraintSet_Entries()
	{
		var constraints = new MediaTrackConstraints();
		IList<MediaTrackConstraintSet> invalidAdvanced =
			(IList<MediaTrackConstraintSet>)(object)new List<MediaTrackConstraintSet?> { new(), null };

		var exception = Assert.Throws<ArgumentException>(() => constraints.Advanced = invalidAdvanced);

		Assert.That(exception!.ParamName, Is.EqualTo("value"));
	}

	[Test]
	public void MediaTrackConstraintSet_IdlParity_HasRequiredConstraints_IsFalse_For_IdealOnly_Values()
	{
		var constraints = new MediaTrackConstraintSet
		{
			Width = new MediaTrackConstraints.PositiveUIntRangeConstraint { Ideal = 1280 },
			FacingMode = new MediaTrackConstraints.Constraint<VideoFacingModeValue>(VideoFacingModes.User)
			{
				Exact = null,
				Ideal = VideoFacingModes.User,
			},
			DeviceId = new MediaTrackConstraints.StringConstraint("placeholder") { Exact = null, Ideal = "camera-1" },
			EchoCancellation = new EchoCancellationConstraint { Ideal = true },
		};

		Assert.That(constraints.HasRequiredConstraints, Is.False);
	}

	[Test]
	public void MediaTrackConstraintSet_IdlParity_HasRequiredConstraints_IsTrue_For_Exact_And_Bounded_Values()
	{
		var constraints = new MediaTrackConstraintSet
		{
			Width = new MediaTrackConstraints.PositiveUIntRangeConstraint { Min = 640 },
		};

		Assert.That(constraints.HasRequiredConstraints, Is.True);
	}

	[Test]
	public void MediaTrackCapabilities_Defaults_EchoCancellation_To_Empty()
	{
		var capabilities = new MediaTrackCapabilities();

		Assert.That(capabilities.EchoCancellation, Is.Empty);
	}

	[Test]
	public void MediaTrackSettings_Defaults_EchoCancellation_To_FalseBoolean()
	{
		var settings = new MediaTrackSettings();

		Assert.That(settings.EchoCancellation.IsBoolean, Is.True);
		Assert.That(settings.EchoCancellation.BooleanValue, Is.False);
		Assert.That(settings.EchoCancellation.IsMode, Is.False);
	}

	[Test]
	public void EchoCancellationConstraint_ImplicitBool_SetsExactBoolean()
	{
		EchoCancellationConstraint constraint = true;
		var exact = constraint.Exact;

		Assert.That(exact.HasValue, Is.True);
		Assert.That(exact.GetValueOrDefault().IsBoolean, Is.True);
		Assert.That(exact.GetValueOrDefault().BooleanValue, Is.True);
	}

	[Test]
	public void EchoCancellationConstraint_ImplicitEnum_SetsExactMode()
	{
		EchoCancellationConstraint constraint = EchoCancellationMode.Software;
		var exact = constraint.Exact;

		Assert.That(exact.HasValue, Is.True);
		Assert.That(exact.GetValueOrDefault().IsMode, Is.True);
		Assert.That(exact.GetValueOrDefault().Mode, Is.EqualTo(EchoCancellationMode.Software));
		Assert.That(exact.GetValueOrDefault().ModeValue, Is.EqualTo("software"));
	}

	[Test]
	public void EchoCancellationValue_RawString_PreservesUnknownMode()
	{
		var value = new EchoCancellationValue("vendor-advanced-mode");

		Assert.That(value.IsMode, Is.True);
		Assert.That(value.Mode, Is.Null);
		Assert.That(value.ModeValue, Is.EqualTo("vendor-advanced-mode"));
	}

	[Test]
	public void VideoFacingModeValue_KnownEnum_UsesKnownValueAndRawString()
	{
		VideoFacingModeValue value = VideoFacingModes.Environment;

		Assert.That(value.IsKnown, Is.True);
		Assert.That(value.KnownValue, Is.EqualTo(VideoFacingModes.Environment));
		Assert.That(value.RawValue, Is.EqualTo("environment"));
	}

	[Test]
	public void VideoFacingModeValue_RawString_PreservesUnknownValue()
	{
		var value = new VideoFacingModeValue("vendor-facing-mode");

		Assert.That(value.IsKnown, Is.False);
		Assert.That(value.KnownValue, Is.Null);
		Assert.That(value.RawValue, Is.EqualTo("vendor-facing-mode"));
	}

	[Test]
	public void VideoResizeModeValue_KnownEnum_UsesKnownValueAndRawString()
	{
		VideoResizeModeValue value = VideoResizeModes.CropAndScale;

		Assert.That(value.IsKnown, Is.True);
		Assert.That(value.KnownValue, Is.EqualTo(VideoResizeModes.CropAndScale));
		Assert.That(value.RawValue, Is.EqualTo("crop-and-scale"));
	}

	[Test]
	public void VideoResizeModeValue_RawString_PreservesUnknownValue()
	{
		var value = new VideoResizeModeValue("vendor-resize-mode");

		Assert.That(value.IsKnown, Is.False);
		Assert.That(value.KnownValue, Is.Null);
		Assert.That(value.RawValue, Is.EqualTo("vendor-resize-mode"));
	}

	[Test]
	public void InputDeviceInfo_GetCapabilities_PopulatesIdentityFields()
	{
		var device = InputDeviceInfo.Create("device-1", MediaDeviceKind.VideoInput, "Camera", "group-1");
		var capabilities = device.GetCapabilities();

		Assert.That(capabilities.DeviceId, Is.EqualTo("device-1"));
		Assert.That(capabilities.GroupId, Is.EqualTo("group-1"));
	}

	[Test]
	public void InputDeviceInfo_GetCapabilities_InvokesDelegate_WhenProvided()
	{
		var expected = MediaTrackCapabilities.Create(
			deviceId: "device-2",
			groupId: "group-2",
			width: new ValueRange<uint> { Min = 640, Max = 1920 });

		var device = InputDeviceInfo.Create("device-2", MediaDeviceKind.VideoInput, "Camera", "group-2",
			() => expected);

		Assert.That(device.GetCapabilities(), Is.SameAs(expected));
	}

	[Test]
	public void InputDeviceInfo_GetCapabilities_FallsBackToIdentityFields_WhenNoDelegateProvided()
	{
		var device = InputDeviceInfo.Create("device-3", MediaDeviceKind.AudioInput, "Mic", "group-3");
		var capabilities = device.GetCapabilities();

		Assert.That(capabilities.DeviceId, Is.EqualTo("device-3"));
		Assert.That(capabilities.GroupId, Is.EqualTo("group-3"));
		Assert.That(capabilities.Width, Is.Null);
	}
}
