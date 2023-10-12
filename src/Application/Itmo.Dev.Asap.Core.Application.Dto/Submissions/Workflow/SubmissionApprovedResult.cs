using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionApprovedResult
{
    private SubmissionApprovedResult() { }

    public sealed record Success(SubmissionRateDto SubmissionRate) : SubmissionApprovedResult;

    public sealed record InvalidState(SubmissionStateDto State) : SubmissionApprovedResult;
}