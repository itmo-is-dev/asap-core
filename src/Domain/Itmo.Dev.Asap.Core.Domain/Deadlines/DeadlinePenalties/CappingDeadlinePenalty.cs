using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;

public class CappingDeadlinePenalty : DeadlinePenalty
{
    public CappingDeadlinePenalty(TimeSpan spanBeforeActivation, Points cap)
        : base(spanBeforeActivation)
    {
        Cap = cap;
    }

    public Points Cap { get; set; }

    public override Points Apply(Points points)
    {
        return Points.Min(points, Cap);
    }
}