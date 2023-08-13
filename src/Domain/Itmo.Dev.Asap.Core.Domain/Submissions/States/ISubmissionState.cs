using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions.States;

public interface ISubmissionState
{
    SubmissionStateKind Kind { get; }

    /// <summary>
    ///     Gets a value indicating whether the state is terminal, yet relevant for the system.
    /// </summary>
    bool IsTerminalEffectiveState { get; }

    ISubmissionState MoveToRated(Fraction? rating, Points? extraPoints);

    ISubmissionState MoveToPointsUpdated(Fraction? rating, Points? extraPoints);

    ISubmissionState MoveToBanned();

    ISubmissionState MoveToActivated();

    ISubmissionState MoveToDeactivated();

    ISubmissionState MoveToDateUpdated(SpbDateTime newDate);

    ISubmissionState MoveToDeleted();

    ISubmissionState MoveToCompleted();

    ISubmissionState MoveToReviewed();
}