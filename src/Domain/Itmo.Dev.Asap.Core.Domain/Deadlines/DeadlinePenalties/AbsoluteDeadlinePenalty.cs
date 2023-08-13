using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;

public class AbsoluteDeadlinePenalty : DeadlinePenalty
{
    public AbsoluteDeadlinePenalty(TimeSpan spanBeforeActivation, Points absoluteValue)
        : base(spanBeforeActivation)
    {
        AbsoluteValue = absoluteValue;
    }

    public Points AbsoluteValue { get; set; }

    public override Points Apply(Points points)
    {
        return points - AbsoluteValue;
    }
}