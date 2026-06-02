using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class MediaStreamConstraintsIdlParityTests
{
	[Test]
	public void MediaStreamConstraints_IdlParity_BooleanMembers_Map_From_BoolConstructor()
	{
		var constraints = new MediaStreamConstraints(audio: true, video: false);

		Assert.That(constraints.Audio, Is.True);
		Assert.That(constraints.AudioConstraints, Is.Null);
		Assert.That(constraints.Video, Is.False);
		Assert.That(constraints.VideoConstraints, Is.Null);
	}

	[Test]
	public void MediaStreamConstraints_IdlParity_AudioUnionObject_EnablesAudio_And_Preserves_Instance()
	{
		var audio = new MediaTrackConstraints
		{
			Width = 640,
		};
		var constraints = new MediaStreamConstraints(audio, video: false);

		Assert.That(constraints.Audio, Is.True);
		Assert.That(constraints.AudioConstraints, Is.SameAs(audio));
		Assert.That(constraints.Video, Is.False);
		Assert.That(constraints.VideoConstraints, Is.Null);
	}

	[Test]
	public void MediaStreamConstraints_IdlParity_VideoUnionObject_EnablesVideo_And_Preserves_Instance()
	{
		var video = new MediaTrackConstraints
		{
			Height = 720,
		};
		var constraints = new MediaStreamConstraints(audio: false, video);

		Assert.That(constraints.Audio, Is.False);
		Assert.That(constraints.AudioConstraints, Is.Null);
		Assert.That(constraints.Video, Is.True);
		Assert.That(constraints.VideoConstraints, Is.SameAs(video));
	}
}
