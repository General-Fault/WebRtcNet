using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BasicVideoChat.Signaling;
using WebRtcNet;
using WebRtcNet.Media;

namespace BasicVideoChat;

public partial class MainWindow : Window
{
	private const int DefaultPort = 7777;

	// Default STUN server for NAT traversal. Works for both LAN and internet connections.
	// For LAN-only use you can pass an empty RtcConfiguration().
	// Replace with your own STUN/TURN server if needed.
	// See: https://webrtc.org/getting-started/turn-server
	private static readonly RtcConfiguration DefaultConfiguration = new(
		new[] { new RtcIceServer("stun:stun.l.google.com:19302") });

	private RtcPeerConnection? _peerConnection;
	private TcpSignalingChannel? _signaling;
	private MediaStream? _localStream;
	private MediaStreamTrack? _audioTrack;
	private MediaStreamTrack? _videoTrack;
	private WpfVideoRenderer? _localRenderer;
	private WpfVideoRenderer? _remoteRenderer;

	public MainWindow()
	{
		InitializeComponent();
	}

	private bool IsHost => HostRadio.IsChecked == true;

	private void HostRadio_Checked(object sender, RoutedEventArgs e) =>
		IpBox.IsEnabled = false;

	private void GuestRadio_Checked(object sender, RoutedEventArgs e) =>
		IpBox.IsEnabled = true;

	private async void ConnectBtn_Click(object sender, RoutedEventArgs e)
	{
		ConnectBtn.IsEnabled = false;
		try
		{
			await StartCallAsync();
			HangUpBtn.IsEnabled = true;
			MuteBtn.IsEnabled = true;
			CameraBtn.IsEnabled = true;
		}
		catch (Exception ex)
		{
			SetStatus($"Error: {ex.Message}");
			ConnectBtn.IsEnabled = true;
		}
	}

	private async void HangUpBtn_Click(object sender, RoutedEventArgs e) =>
		await HangUpAsync();

	private void MuteBtn_Click(object sender, RoutedEventArgs e)
	{
		if (_audioTrack != null)
			_audioTrack.Enabled = MuteBtn.IsChecked != true;
	}

	private void CameraBtn_Click(object sender, RoutedEventArgs e)
	{
		if (_videoTrack != null)
			_videoTrack.Enabled = CameraBtn.IsChecked != true;
	}

	private async Task StartCallAsync()
	{
		SetStatus("Acquiring media...");

		var mediaDevices = CreateInteropInstance<MediaDevices>("WebRtcInterop.Media.MediaDevices");
		_localStream = await mediaDevices.GetUserMedia(new MediaStreamConstraints(true, true));

		_audioTrack = _localStream.GetAudioTracks().FirstOrDefault();
		_videoTrack = _localStream.GetVideoTracks().FirstOrDefault();

		_localRenderer = new WpfVideoRenderer(LocalVideo);
		_remoteRenderer = new WpfVideoRenderer(RemoteVideo);
		// TODO: Attach _localRenderer to _videoTrack once VideoRenderer interface is expanded.

		// NOTE: Many WebRtcInterop methods currently throw NotImplementedException — this example
		// will not run end-to-end until they are implemented.  SetLocalDescription and
		// AddIceCandidate are the minimum required for a basic call flow.
		_peerConnection = CreateInteropInstance<RtcPeerConnection>("WebRtcInterop.RtcPeerConnection", DefaultConfiguration);
		_peerConnection.OnIceCandidate += OnIceCandidate;
		_peerConnection.OnTrack += OnTrack;
		_peerConnection.OnConnectionStateChange += OnConnectionStateChange;

		foreach (var track in _localStream.GetTracks())
			_peerConnection.AddTrack(track, _localStream);

		_signaling = new TcpSignalingChannel();
		_signaling.MessageHandler = OnSignalingMessageAsync;
		_signaling.Disconnected += () => Dispatcher.BeginInvoke(async () => await HangUpAsync());

		if (IsHost)
		{
			var port = int.TryParse(PortBox.Text, out var p) ? p : DefaultPort;
			SetStatus($"Listening on port {port}...");
			await _signaling.ListenAsync(port);
			SetStatus("Guest connected. Creating offer...");

			// Drive the offer explicitly here rather than relying on OnNegotiationNeeded,
			// since we control when the signaling channel is ready.
			var offer = await _peerConnection.CreateOffer();
			await _peerConnection.SetLocalDescription(offer);
			await _signaling.SendAsync(new SignalingMessage { Type = SignalingMessageType.Offer, Sdp = offer.Sdp });
			SetStatus("Offer sent. Waiting for answer...");
		}
		else
		{
			var host = IpBox.Text.Trim();
			var port = int.TryParse(PortBox.Text, out var p) ? p : DefaultPort;
			SetStatus($"Connecting to {host}:{port}...");
			await _signaling.ConnectAsync(host, port);
			SetStatus("Connected. Waiting for offer...");
		}
	}

	// MessageHandler is awaited by TcpSignalingChannel before the next message is dispatched,
	// ensuring that SetRemoteDescription always completes before any AddIceCandidate call.
	private async Task OnSignalingMessageAsync(SignalingMessage message)
	{
		try
		{
			switch (message.Type)
			{
				case SignalingMessageType.Offer:
					await _peerConnection!.SetRemoteDescription(
						new RtcSessionDescription(RtcSdpType.Offer, message.Sdp!));
					var answer = await _peerConnection.CreateAnswer();
					await _peerConnection.SetLocalDescription(answer);
					await _signaling!.SendAsync(new SignalingMessage
					{
						Type = SignalingMessageType.Answer,
						Sdp = answer.Sdp
					});
					SetStatus("Answer sent.");
					break;

				case SignalingMessageType.Answer:
					await _peerConnection!.SetRemoteDescription(
						new RtcSessionDescription(RtcSdpType.Answer, message.Sdp!));
					SetStatus("Answer received.");
					break;

				case SignalingMessageType.Candidate:
					await _peerConnection!.AddIceCandidate(new RtcIceCandidate(
						message.Candidate!, message.SdpMid, message.SdpMLineIndex));
					break;

				case SignalingMessageType.Bye:
					Dispatcher.BeginInvoke(async () => await HangUpAsync());
					break;
			}
		}
		catch (Exception ex)
		{
			SetStatus($"Signaling error: {ex.Message}");
		}
	}

	private void OnIceCandidate(object? sender, RtcIceCandidateEventArgs e)
	{
		// Capture _signaling before the fire-and-forget to guard against a concurrent HangUp.
		var sig = _signaling;
		if (sig != null)
			_ = sig.SendAsync(new SignalingMessage
			{
				Type = SignalingMessageType.Candidate,
				Candidate = e.Candidate.Candidate,
				SdpMid = e.Candidate.SdpMid,
				SdpMLineIndex = e.Candidate.SdpMLineIndex
			});
	}

	private void OnTrack(object? sender, RtcTrackEventArgs e)
	{
		// TODO: Attach e.Track to _remoteRenderer once VideoRenderer interface is expanded.
		Dispatcher.Invoke(() => SetStatus($"Remote {e.Track.Kind} track received."));
	}

	private void OnConnectionStateChange(object? sender, EventArgs e) =>
		Dispatcher.Invoke(() => SetStatus($"Connection: {_peerConnection?.ConnectionState}"));

	private async Task HangUpAsync()
	{
		// Best-effort Bye — send before tearing down the channel.
		if (_signaling != null)
		{
			try { await _signaling.SendAsync(new SignalingMessage { Type = SignalingMessageType.Bye }); }
			catch { }
			_signaling.Dispose();
			_signaling = null;
		}

		_peerConnection?.Close();
		_peerConnection = null;

		_audioTrack?.Stop();
		_videoTrack?.Stop();
		_localStream?.Dispose();
		_localStream = null;
		_audioTrack = null;
		_videoTrack = null;

		ConnectBtn.IsEnabled = true;
		HangUpBtn.IsEnabled = false;
		MuteBtn.IsEnabled = false;
		MuteBtn.IsChecked = false;
		CameraBtn.IsEnabled = false;
		CameraBtn.IsChecked = false;
		SetStatus("Ready");
	}

	private void SetStatus(string message) =>
		Dispatcher.Invoke(() => StatusText.Text = message);

	private static T CreateInteropInstance<T>(string fullTypeName, params object[] args) where T : class
	{
		var type =
			Type.GetType($"{fullTypeName}, WebRtcInterop.Core") ??
			Type.GetType($"{fullTypeName}, WebRtcInterop.Framework") ??
			Type.GetType($"{fullTypeName}, WebRtcInterop");
		if (type == null)
			throw new NotSupportedException($"{fullTypeName} is not available. Ensure WebRtcInterop is built for the active target framework.");

		if (Activator.CreateInstance(type, args) is T instance)
			return instance;

		throw new InvalidOperationException($"{fullTypeName} does not implement {typeof(T).FullName}.");
	}
}
