using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WebRtcNet.Media;

namespace BasicVideoChat;

/// <summary>
/// WPF implementation of <see cref="VideoRenderer"/> using a <see cref="WriteableBitmap"/>-backed
/// <see cref="Image"/> control.
/// </summary>
/// <remarks>
/// <para>
/// This is a prototype that lives in <c>BasicVideoChat</c> until a dedicated
/// <c>WebRtcNet.Wpf</c> renderer assembly is created (see GitHub issue #36).
/// </para>
/// <para>
/// Once the <see cref="VideoRenderer"/> interface is expanded to include a frame-delivery
/// method, frames should be rendered here via the pattern below. The expected frame format
/// from the native WebRTC stack is I420 (YUV planar); convert to BGR24 before writing pixels.
/// </para>
/// <code>
/// public void RenderFrame(byte[] bgrData, int width, int height)
/// {
///     _image.Dispatcher.BeginInvoke(() =>
///     {
///         if (_bitmap == null || _bitmap.PixelWidth != width || _bitmap.PixelHeight != height)
///         {
///             _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
///             _image.Source = _bitmap;
///         }
///         _bitmap.Lock();
///         _bitmap.WritePixels(new Int32Rect(0, 0, width, height), bgrData, width * 3, 0);
///         _bitmap.Unlock();
///     });
/// }
/// </code>
/// </remarks>
public class WpfVideoRenderer : VideoRenderer
{
	private readonly Image _image;
#pragma warning disable IDE0052 // field reserved for future use when VideoRenderer is expanded
	private WriteableBitmap? _bitmap;
#pragma warning restore IDE0052

	/// <summary>Creates a renderer backed by the given WPF <see cref="Image"/> control.</summary>
	/// <param name="image">The WPF <see cref="Image"/> control that will display video frames.</param>
	public WpfVideoRenderer(Image image)
	{
		_image = image;
	}
}
