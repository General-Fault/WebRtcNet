using System;
using System.Collections.Generic;

namespace WebRtcNet.Media;

/// <summary>
/// Constraints for the MediaTrack.
/// </summary>
/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraints" />
public partial class MediaTrackConstraints : MediaTrackConstraintSet
{
	private IList<MediaTrackConstraintSet>? _advanced;

	/// <summary>
	/// A sequence of additional constraint sets to apply in the order supplied by the caller.
	/// </summary>
	/// <remarks>
	/// This corresponds to <c>MediaTrackConstraints.advanced</c> in the specification.
	/// Local reference: <c>documents/specs/mediacapture/mediacapture-idl.webidl</c>
	/// (<c>MediaTrackConstraints</c>, <c>MediaTrackConstraintSet</c>, and
	/// <c>MediaStreamTrack.applyConstraints(optional MediaTrackConstraints constraints = {})</c>).
	/// </remarks>
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraints-advanced" />
	/// <seealso href="https://www.w3.org/TR/mediacapture-streams/#dom-mediatrackconstraintset" />
	public IList<MediaTrackConstraintSet>? Advanced
	{
		get => _advanced;
		set
		{
			if (value is not null)
			{
				for (var i = 0; i < value.Count; i++)
				{
					if (value[i] is null)
						throw new ArgumentException("Advanced constraint sets cannot contain null entries.", nameof(value));
				}
			}

			_advanced = value;
		}
	}

	/// <summary>
	/// Enumerates constraints in processing order: base set first, then <see cref="Advanced" /> entries in list order.
	/// </summary>
	/// <remarks>
	/// Local reference: <c>documents/specs/mediacapture/mediacapture-idl.webidl</c>
	/// (<c>MediaTrackConstraints</c> and <c>MediaTrackConstraints.advanced</c> sequence ordering).
	/// </remarks>
	public IEnumerable<MediaTrackConstraintSet> EnumerateConstraintSetsInProcessingOrder()
	{
		yield return this;

		if (_advanced is null)
			yield break;

		foreach (var constraintSet in _advanced)
			yield return constraintSet;
	}
}