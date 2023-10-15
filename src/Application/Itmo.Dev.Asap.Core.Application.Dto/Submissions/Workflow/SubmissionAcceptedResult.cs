using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionAcceptedResult
{
    private SubmissionAcceptedResult() { }

    public sealed record Success(SubmissionRateDto SubmissionRate) : SubmissionAcceptedResult;

    public sealed record InvalidState(SubmissionStateDto SubmissionState) : SubmissionAcceptedResult;
}