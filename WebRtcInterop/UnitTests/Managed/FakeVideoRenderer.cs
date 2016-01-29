using WebRtcNet;

namespace WebRtcInterop.UnitTests
{
    internal class FakeVideoRenderer : IVideoRenderer
    {
        private IMediaStreamTrack track;

        public FakeVideoRenderer(IMediaStreamTrack track)
        {
            this.track = track;
        }
    }
}