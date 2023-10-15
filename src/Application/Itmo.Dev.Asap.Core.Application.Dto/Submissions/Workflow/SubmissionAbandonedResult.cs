using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionAbandonedResult
{
    private SubmissionAbandonedResult() { }

    public sealed record Success(int SubmissionCode) : SubmissionAbandonedResult;

    public sealed record InvalidState(SubmissionStateDto SubmissionState) : SubmissionAbandonedResult;
}