using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions.States;

public class CompletedSubmissionState : ISubmissionState
{
    public SubmissionStateKind Kind => SubmissionStateKind.Completed;

    public bool IsTerminalEffectiveState => true;

    public ISubmissionState MoveToRated(Fraction? rating, Points? extraPoints)
    {
        const string message = "Submission is already completed and cannot be rated again";
        throw new DomainInvalidOperationException(message);
    }

    public ISubmissionState MoveToPointsUpdated(Fraction? rating, Points? extraPoints)
    {
        return this;
    }

    public ISubmissionState MoveToBanned()
    {
        return new BannedSubmissionState();
    }

    public ISubmissionState MoveToActivated()
    {
        return new ActiveSubmissionState();
    }

    public ISubmissionState MoveToDeactivated()
    {
        const string message = "Submission is already completed and cannot be deactivated";
        throw new DomainInvalidOperationException(message);
    }

    public ISubmissionState MoveToDateUpdated(SpbDateTime newDate)
    {
        return this;
    }

    public ISubmissionState MoveToDeleted()
    {
        return new DeletedSubmissionState();
    }

    public ISubmissionState MoveToCompleted()
    {
        const string message = "Submission is already completed and cannot be completed again";
        throw new DomainInvalidOperationException(message);
    }

    public ISubmissionState MoveToReviewed()
    {
        const string message = "Submission is already completed and cannot be reviewed again";
        throw new DomainInvalidOperationException(message);
    }
}