namespace Itmo.Dev.Asap.Core.Domain.Submissions;

public abstract record SubmissionStateMoveResult
{
    private SubmissionStateMoveResult() { }

    public sealed record Success(ISubmissionState State) : SubmissionStateMoveResult;

    public sealed record InvalidMove : SubmissionStateMoveResult;
}