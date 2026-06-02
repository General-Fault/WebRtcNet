using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BasicVideoChat.Signaling;

/// <summary>
/// Minimal TCP signaling channel for peer-to-peer connection setup.
/// </summary>
/// <remarks>
/// One peer calls <see cref="ListenAsync"/> (Host) and the other calls
/// <see cref="ConnectAsync"/> (Guest). Messages are newline-delimited JSON.
/// <para>
/// The read loop awaits <see cref="MessageHandler"/> before processing the next message,
/// ensuring that <c>SetRemoteDescription</c> always completes before any
/// <c>AddIceCandidate</c> call arrives from the remote peer.
/// </para>
/// </remarks>
internal sealed class TcpSignalingChannel : IDisposable
{
	private TcpListener? _listener;
	private TcpClient? _client;
	private StreamReader? _reader;
	private StreamWriter? _writer;
	private readonly SemaphoreSlim _writeLock = new(1, 1);
	private CancellationTokenSource _cts = new();

	/// <summary>
	/// Invoked sequentially for each received message. The read loop awaits this
	/// delegate before reading the next line.
	/// </summary>
	public Func<SignalingMessage, Task>? MessageHandler { get; set; }

	/// <summary>Raised when the remote side closes the connection.</summary>
	public event Action? Disconnected;

	/// <summary>
	/// Starts listening on <paramref name="port"/> and waits for a single guest connection.
	/// Returns once the guest has connected.
	/// </summary>
	public async Task ListenAsync(int port)
	{
		_listener = new TcpListener(IPAddress.Any, port);
		_listener.Start();
		_client = await _listener.AcceptTcpClientAsync();
		_listener.Stop();
		AttachStreams();
		_ = ReadLoopAsync(_cts.Token);
	}

	/// <summary>
	/// Connects to a host at <paramref name="host"/>:<paramref name="port"/>.
	/// </summary>
	public async Task ConnectAsync(string host, int port)
	{
		_client = new TcpClient();
		await _client.ConnectAsync(host, port);
		AttachStreams();
		_ = ReadLoopAsync(_cts.Token);
	}

	private void AttachStreams()
	{
		var stream = _client!.GetStream();
		_reader = new StreamReader(stream, Encoding.UTF8);
		_writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
	}

	/// <summary>
	/// Sends <paramref name="message"/> as a newline-delimited JSON object.
	/// Uses a write lock so it is safe to call concurrently from ICE callbacks.
	/// </summary>
	public async Task SendAsync(SignalingMessage message)
	{
		var json = JsonSerializer.Serialize(message);
		await _writeLock.WaitAsync().ConfigureAwait(false);
		try
		{
			await _writer!.WriteLineAsync(json).ConfigureAwait(false);
		}
		finally
		{
			_writeLock.Release();
		}
	}

	private async Task ReadLoopAsync(CancellationToken ct)
	{
		try
		{
			while (!ct.IsCancellationRequested)
			{
				var line = await _reader!.ReadLineAsync().ConfigureAwait(false);
				if (line == null)
					break;

				var message = JsonSerializer.Deserialize<SignalingMessage>(line);
				if (message != null && MessageHandler != null)
					await MessageHandler(message).ConfigureAwait(false);
			}
		}
		catch (OperationCanceledException) { }
		catch (IOException) { }
		catch (ObjectDisposedException) { }
		finally
		{
			Disconnected?.Invoke();
		}
	}

	public void Dispose()
	{
		_cts.Cancel();
		_cts.Dispose();
		_writer?.Dispose();
		_reader?.Dispose();
		_client?.Dispose();
		_listener?.Stop();
		_writeLock.Dispose();
	}
}
