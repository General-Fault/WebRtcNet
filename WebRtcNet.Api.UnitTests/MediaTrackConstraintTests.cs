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
	public void MediaTrackSettings_Defaults_BackgroundBlur_To_False()
	{
		var settings = new MediaTrackSettings();

		Assert.IsFalse(settings.BackgroundBlur);
	}

	[Test]
	public void MediaTrackConstraints_Defaults_EchoCancellation_To_Null()
	{
		var constraints = new MediaTrackConstraints();

		Assert.IsNull(constraints.EchoCancellation);
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
		EchoCancellationConstraint constraint = EchoCancellationMode.RemoteOnly;
		var exact = constraint.Exact;

		Assert.That(exact.HasValue, Is.True);
		Assert.That(exact.GetValueOrDefault().IsMode, Is.True);
		Assert.That(exact.GetValueOrDefault().Mode, Is.EqualTo(EchoCancellationMode.RemoteOnly));
		Assert.That(exact.GetValueOrDefault().ModeValue, Is.EqualTo("remote-only"));
	}

	[Test]
	public void EchoCancellationValue_RawString_PreservesUnknownMode()
	{
		var value = new EchoCancellationValue("vendor-advanced-mode");

		Assert.That(value.IsMode, Is.True);
		Assert.That(value.Mode, Is.Null);
		Assert.That(value.ModeValue, Is.EqualTo("vendor-advanced-mode"));
	}
}