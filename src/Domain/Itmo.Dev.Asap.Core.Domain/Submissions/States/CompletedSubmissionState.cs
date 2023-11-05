using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions.States;

public class CompletedSubmissionState : ISubmissionState
{
    public SubmissionStateKind Kind => SubmissionStateKind.Completed;

    public bool IsTerminalEffectiveState => true;

    public SubmissionStateMoveResult MoveToRated(Fraction? rating, Points? extraPoints)
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToPointsUpdated(Fraction? rating, Points? extraPoints)
        => new SubmissionStateMoveResult.Success(this);

    public SubmissionStateMoveResult MoveToBanned()
        => new SubmissionStateMoveResult.Success(new BannedSubmissionState());

    public SubmissionStateMoveResult MoveToUnbanned()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToActivated()
        => new SubmissionStateMoveResult.Success(new ActiveSubmissionState());

    public SubmissionStateMoveResult MoveToDeactivated()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToDateUpdated(SpbDateTime newDate)
        => new SubmissionStateMoveResult.Success(this);

    public SubmissionStateMoveResult MoveToDeleted()
        => new SubmissionStateMoveResult.Success(new DeletedSubmissionState());

    public SubmissionStateMoveResult MoveToCompleted()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToReviewed()
        => new SubmissionStateMoveResult.InvalidMove();
}