namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionUpdatedResult
{
    private SubmissionUpdatedResult() { }

    public sealed record Success(SubmissionRateDto SubmissionRate, bool IsCreated) : SubmissionUpdatedResult;
}