using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePolicies;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Platform.Testing;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Deadlines;

public class DeadlinePolicyTest : TestBase
{
    [Fact]
    public void AddEqualPenaltiesShouldThrow()
    {
        var fractionPenalty = new FractionDeadlinePenalty(TimeSpan.FromDays(7), Fraction.FromDenormalizedValue(20));
        var absolutePenalty = new AbsoluteDeadlinePenalty(TimeSpan.FromDays(7), new Points(1));

        var policy = new DeadlinePolicy(new HashSet<DeadlinePenalty>());
        policy.AddDeadlinePenalty(fractionPenalty);

        Assert.Throws<DomainInvalidOperationException>(() => policy.AddDeadlinePenalty(absolutePenalty));
    }

    [Fact]
    public void PointPenaltyTest()
    {
        var fraction = Fraction.FromDenormalizedValue(20);
        var absolutePoints = new Points(1);

        var fractionPenalty = new FractionDeadlinePenalty(TimeSpan.FromDays(7), fraction);
        var absolutePenalty = new AbsoluteDeadlinePenalty(TimeSpan.FromDays(14), absolutePoints);

        var policy = new DeadlinePolicy(new HashSet<DeadlinePenalty>());
        policy.AddDeadlinePenalty(fractionPenalty);
        policy.AddDeadlinePenalty(absolutePenalty);

        var maxPoints = new Points(10);

        Points? penaltyPoints1 = policy.GetPointPenalty(
            maxPoints,
            new DateOnly(2000, 10, 10),
            new DateOnly(2000, 10, 18));

        Assert.Equal(penaltyPoints1, maxPoints * fraction.Value);

        Points? penaltyPoints2 = policy.GetPointPenalty(
            maxPoints,
            new DateOnly(2000, 10, 10),
            new DateOnly(2003, 1, 1));

        Assert.Equal(penaltyPoints2, maxPoints - absolutePoints);
    }
}