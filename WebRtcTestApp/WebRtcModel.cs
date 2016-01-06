using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using WebRtcNet;

namespace WebRtcTestApp
{
    public class WebRtcModel
    {
        private RtcConfiguration _configuration;

        public WebRtcModel(IEnumerable<Tuple<string, string, string>> iceServers)
        {
            _configuration = new RtcConfiguration(from iceServer in iceServers select new RtcIceServer(iceServer.Item1, iceServer.Item2, iceServer.Item3));
        }

        public async Task<ICall> StartCallAsync(ITransport connection)
        {
            var peerConnection = new WebRtcInterop.RtcPeerConnection(_configuration);

            var stream = Media.GetUserMedia(null);
            peerConnection.AddStream(stream);

            var options = new RtcOfferOptions();
            var sessionDescription = await peerConnection.CreateOffer(options);

            return null;
        }
    }

    public interface ITransport
    {
        Task SendMessage(string message);
        event EventHandler<string> OnMessage;
    }

    public interface IStartingCall
    {
        void Cancel();
        Task<ICall> AnswerTask { get; }
    }

    public interface ICall
    {
        void EndCall();

        Task CalDisposedTask { get; }
        Task CallTask { get; }
    }

    public interface IIncommingCall
    {
        Task<ICall> AnswerCall(CancellationToken cancellationToken);
        void RejectCall();
    }

    public interface IOutgoingCall
    {
        void Cance();
        Task<IStartingCall> WaitForAnswer();
    }
}
