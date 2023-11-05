using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions;

public interface ISubmissionState
{
    SubmissionStateKind Kind { get; }

    /// <summary>
    ///     Gets a value indicating whether the state is terminal, yet relevant for the system.
    /// </summary>
    bool IsTerminalEffectiveState { get; }

    SubmissionStateMoveResult MoveToRated(Fraction? rating, Points? extraPoints);

    SubmissionStateMoveResult MoveToPointsUpdated(Fraction? rating, Points? extraPoints);

    SubmissionStateMoveResult MoveToBanned();

    SubmissionStateMoveResult MoveToUnbanned();

    SubmissionStateMoveResult MoveToActivated();

    SubmissionStateMoveResult MoveToDeactivated();

    SubmissionStateMoveResult MoveToDateUpdated(SpbDateTime newDate);

    SubmissionStateMoveResult MoveToDeleted();

    SubmissionStateMoveResult MoveToCompleted();

    SubmissionStateMoveResult MoveToReviewed();
}