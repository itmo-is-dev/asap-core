using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionRejectedResult
{
    private SubmissionRejectedResult() { }

    public sealed record Success(int SubmissionCode) : SubmissionRejectedResult;

    public sealed record InvalidState(SubmissionStateDto SubmissionState) : SubmissionRejectedResult;
}