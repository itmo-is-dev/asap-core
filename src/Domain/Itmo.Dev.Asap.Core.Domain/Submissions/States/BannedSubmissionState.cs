using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions.States;

public class BannedSubmissionState : ISubmissionState
{
    public SubmissionStateKind Kind => SubmissionStateKind.Banned;

    public bool IsTerminalEffectiveState => true;

    public SubmissionStateMoveResult MoveToRated(Fraction? rating, Points? extraPoints)
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToPointsUpdated(Fraction? rating, Points? extraPoints)
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToBanned()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToActivated()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToDeactivated()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToDateUpdated(SpbDateTime newDate)
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToDeleted()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToCompleted()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToReviewed()
        => new SubmissionStateMoveResult.InvalidMove();
}