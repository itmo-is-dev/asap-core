using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions.States;

public class ActiveSubmissionState : ISubmissionState
{
    public SubmissionStateKind Kind => SubmissionStateKind.Active;

    public bool IsTerminalEffectiveState => false;

    public SubmissionStateMoveResult MoveToRated(Fraction? rating, Points? extraPoints)
        => new SubmissionStateMoveResult.Success(new CompletedSubmissionState());

    public SubmissionStateMoveResult MoveToPointsUpdated(Fraction? rating, Points? extraPoints)
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToBanned()
        => new SubmissionStateMoveResult.Success(new BannedSubmissionState());

    public SubmissionStateMoveResult MoveToActivated()
        => new SubmissionStateMoveResult.InvalidMove();

    public SubmissionStateMoveResult MoveToDeactivated()
        => new SubmissionStateMoveResult.Success(new InactiveSubmissionState());

    public SubmissionStateMoveResult MoveToDateUpdated(SpbDateTime newDate)
        => new SubmissionStateMoveResult.Success(this);

    public SubmissionStateMoveResult MoveToDeleted()
        => new SubmissionStateMoveResult.Success(new DeletedSubmissionState());

    public SubmissionStateMoveResult MoveToCompleted()
        => new SubmissionStateMoveResult.Success(new CompletedSubmissionState());

    public SubmissionStateMoveResult MoveToReviewed()
        => new SubmissionStateMoveResult.Success(new ReviewedSubmissionState());
}