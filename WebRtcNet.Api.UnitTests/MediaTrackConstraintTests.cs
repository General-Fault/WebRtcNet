using NUnit.Framework;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class MediaTrackConstraintTests
{
	[Test]
	public void MediaTrackSupportedConstraints_Defaults_BackgroundBlur_To_False()
	{
		var constraints = new WebRtcNet.Media.MediaTrackSupportedConstraints();

		Assert.IsFalse(constraints.BackgroundBlur);
	}

	[Test]
	public void MediaTrackConstraints_Defaults_BackgroundBlur_To_Null()
	{
		var constraints = new WebRtcNet.Media.MediaTrackConstraints();

		Assert.IsNull(constraints.BackgroundBlur);
	}

	[Test]
	public void MediaTrackCapabilities_Defaults_BackgroundBlur_To_Empty()
	{
		var capabilities = new WebRtcNet.Media.MediaTrackCapabilities();

		Assert.That(capabilities.BackgroundBlur, Is.Empty);
	}

	[Test]
	public void MediaTrackSettings_Defaults_BackgroundBlur_To_False()
	{
		var settings = new WebRtcNet.Media.MediaTrackSettings();

		Assert.IsFalse(settings.BackgroundBlur);
	}
}
