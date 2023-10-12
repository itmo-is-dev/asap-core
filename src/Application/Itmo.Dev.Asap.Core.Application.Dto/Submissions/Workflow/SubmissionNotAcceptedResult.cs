using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionNotAcceptedResult
{
    private SubmissionNotAcceptedResult() { }

    public sealed record Success(SubmissionRateDto SubmissionRate) : SubmissionNotAcceptedResult;

    public sealed record InvalidState(SubmissionStateDto SubmissionState) : SubmissionNotAcceptedResult;
}