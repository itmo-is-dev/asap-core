﻿using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePolicies;

public class DeadlinePolicy
{
    private readonly HashSet<DeadlinePenalty> _penalties;

    public DeadlinePolicy(HashSet<DeadlinePenalty> penalties)
    {
        _penalties = penalties;
    }

    public IReadOnlyCollection<DeadlinePenalty> DeadlinePenalties => _penalties;

    public void AddDeadlinePenalty(DeadlinePenalty penalty)
    {
        if (_penalties.Add(penalty) is false)
            throw new DomainInvalidOperationException("Deadline penalty with specified span already exists");
    }

    public Points ApplyPointPenalty(Points points, DateOnly deadline, DateOnly submissionDate)
    {
        DeadlinePenalty? deadlinePenalty = FindEffectiveDeadlinePenalty(deadline, submissionDate);
        return deadlinePenalty?.Apply(points.Value) ?? points;
    }

    public DeadlinePenalty? FindEffectiveDeadlinePenalty(DateOnly deadline, DateOnly submissionDate)
    {
        if (submissionDate <= deadline)
            return null;

        var submissionDeadlineOffset = TimeSpan.FromDays(
            submissionDate.DayNumber - deadline.DayNumber);

        DeadlinePenalty? activeDeadlinePenalty = DeadlinePenalties
            .Where(dp => dp.SpanBeforeActivation < submissionDeadlineOffset)
            .MaxBy(dp => dp.SpanBeforeActivation);

        return activeDeadlinePenalty;
    }
}