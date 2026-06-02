using System;
using NUnit.Framework;
using WebRtcNet.Media;

namespace WebRtcNet.Api.UnitTests;

[TestFixture]
public class RangeConstraintValidationTests
{
	[Test]
	public void RangeConstraint_Throws_When_MinGreaterThanMax()
	{
		var constraint = new MediaTrackConstraints.RangeConstraint<int>
		{
			Max = 5
		};

		Assert.Throws<ArgumentOutOfRangeException>(() => constraint.Min = 6);
	}

	[Test]
	public void RangeConstraint_Throws_When_ExactOutsideMinMax()
	{
		var constraint = new MediaTrackConstraints.RangeConstraint<int>
		{
			Min = 2,
			Max = 4
		};

		Assert.Throws<ArgumentOutOfRangeException>(() => constraint.Exact = 5);
	}

	[Test]
	public void PositiveUIntRangeConstraint_Throws_When_ZeroProvided()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => _ = new MediaTrackConstraints.PositiveUIntRangeConstraint(0u));
	}

	[Test]
	public void PositiveDoubleRangeConstraint_Throws_When_ZeroProvided()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => _ = new MediaTrackConstraints.PositiveDoubleRangeConstraint(0.0));
	}

	[Test]
	public void NonNegativeDoubleRangeConstraint_Allows_Zero()
	{
		var constraint = new MediaTrackConstraints.NonNegativeDoubleRangeConstraint(0.0);

		Assert.That(constraint.Exact, Is.EqualTo(0.0));
	}

	[Test]
	public void DoubleRangeConstraint_Throws_When_NaNProvided()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => 
			_ = new MediaTrackConstraints.DoubleRangeConstraint(double.NaN));
	}

	[Test]
	public void DoubleRangeConstraint_Throws_When_InfinityProvided()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			_ = new MediaTrackConstraints.DoubleRangeConstraint(double.PositiveInfinity));
	}
}