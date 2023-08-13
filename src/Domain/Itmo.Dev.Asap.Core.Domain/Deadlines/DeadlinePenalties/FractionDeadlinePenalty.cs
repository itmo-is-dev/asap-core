using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;

public class FractionDeadlinePenalty : DeadlinePenalty
{
    public FractionDeadlinePenalty(TimeSpan spanBeforeActivation, Fraction fraction)
        : base(spanBeforeActivation)
    {
        Fraction = fraction;
    }

    public Fraction Fraction { get; set; }

    public override Points Apply(Points points)
    {
        return points * Fraction;
    }
}