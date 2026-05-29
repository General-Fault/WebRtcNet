using System;

namespace WebRtcNet.Media;

/// <summary>
/// Thrown when a constraint cannot be satisfied by any available capture device or track configuration.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown from <see cref="MediaStreamTrack.ApplyConstraints"/> when no available configuration
/// satisfies the requested constraints. The <see cref="Constraint"/> property identifies the specific constraint
/// that could not be satisfied.
/// </para>
/// <para>
/// Corresponds to <c>OverconstrainedError : DOMException</c> in the W3C Media Capture and Streams specification.
/// </para>
/// </remarks>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#overconstrainederror-interface"/>
public class OverconstrainedError : Exception
{
	/// <summary>
	/// Initializes a new instance of <see cref="OverconstrainedError"/>.
	/// </summary>
	/// <param name="constraint">The name of the constraint that could not be satisfied.</param>
	/// <param name="message">A human-readable description of the error.</param>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-overconstrainederror-constructor"/>
	public OverconstrainedError(string constraint, string message = "") : base(message)
	{
		Constraint = constraint;
	}

	/// <summary>
	/// The name of the constraint that was not satisfied. Empty string if the constraint is not known.
	/// </summary>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-overconstrainederror-constraint"/>
	public string Constraint { get; }
}