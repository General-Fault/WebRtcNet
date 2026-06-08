using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using WebRtcNet.Logging;
using WebRtcNet.Media;

namespace WebRtcNet;

/// <summary>
/// Non-browser host root for acquiring WebRTC API entry points.
/// </summary>
public static class Host
{
	private static readonly Lazy<MediaDevices> media_devices_ =
		new(CreateMediaDevices, LazyThreadSafetyMode.ExecutionAndPublication);

	/// <summary>
	/// Gets the process-wide media devices entry point.
	/// </summary>
	public static MediaDevices MediaDevices => media_devices_.Value;

	/// <summary>
	/// Sets the ILoggerFactory for the host.
	/// Must be called before creating MediaDevices or PeerConnection instances to configure logging for the library.
	/// </summary>
	/// <param name="factory">Logger factory to use for all WebRtcNet layers.</param>
	/// <remarks>
	/// If not called, Debug builds will log to console; Release builds will be silent.
	/// </remarks>
	public static void SetLoggerFactory(ILoggerFactory factory)
	{
		if (factory == null)
			throw new ArgumentNullException(nameof(factory));
		WebRtcLogWriterBridge.SetLoggerFactory(factory);
	}

	/// <summary>
	/// Creates a peer connection using the active native backend.
	/// </summary>
	/// <param name="configuration">Peer connection configuration.</param>
	/// <returns>A new peer connection instance.</returns>
	public static RtcPeerConnection CreatePeerConnection(RtcConfiguration configuration)
	{
		if (configuration == null)
			throw new ArgumentNullException(nameof(configuration));
		return CreateInteropInstance(
			() => WebRtcInterop.RtcPeerConnectionFactory.CreatePeerConnection(configuration));
	}

	private static MediaDevices CreateMediaDevices() =>
		CreateInteropInstance(WebRtcInterop.Media.MediaDevicesFactory.CreateMediaDevices);

	private static T CreateInteropInstance<T>(Func<T> activator)
	{
		try
		{
			return activator();
		}
		catch (Exception ex) when (
			ex is DllNotFoundException ||
			ex is FileLoadException ||
			ex is FileNotFoundException ||
			ex is TypeLoadException ||
			ex is MissingMethodException ||
			ex is BadImageFormatException)
		{
			throw new InvalidOperationException(
				$"Failed to initialize native WebRTC backend type '{typeof(T).FullName}'. Ensure WebRtcInterop assemblies and native dependencies are present for this target.",
				ex);
		}
	}
}
