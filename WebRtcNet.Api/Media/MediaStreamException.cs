using System;

namespace WebRtcNet.Media;

/// <summary>
/// Represents an error that occurs while creating or interacting with a <see cref="MediaStream" />.
/// </summary>
/// <remarks>
/// This exception is raised by the interop layer when native media-stream operations fail in ways
/// that do not map to a more specific managed exception type.
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#mediastream" />
public class MediaStreamException : Exception
{
	/// <summary>
	/// Initializes a new exception with no associated stream.
	/// </summary>
	public MediaStreamException()
		: this(null, string.Empty)
	{
	}

	/// <summary>
	/// Initializes a new exception with a message and no associated stream.
	/// </summary>
	/// <param name="message">The exception message.</param>
	public MediaStreamException(string message)
		: this(null, message)
	{
	}

	/// <summary>
	/// Initializes a new exception associated with a stream.
	/// </summary>
	/// <param name="stream">The stream associated with the failure, or <see langword="null" />.</param>
	public MediaStreamException(MediaStream? stream)
		: this(stream, string.Empty)
	{
	}

	/// <summary>
	/// Initializes a new exception associated with a stream and message.
	/// </summary>
	/// <param name="stream">The stream associated with the failure, or <see langword="null" />.</param>
	/// <param name="message">The exception message.</param>
	public MediaStreamException(MediaStream? stream, string message)
		: base(message)
	{
		Stream = stream;
	}

	/// <summary>
	/// The stream associated with the failure, if available.
	/// </summary>
	public MediaStream? Stream { get; }
}
