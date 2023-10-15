using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;

public abstract record SubmissionReactivatedResult
{
    private SubmissionReactivatedResult() { }

    public sealed record Success : SubmissionReactivatedResult;

    public sealed record InvalidState(SubmissionStateDto SubmissionState) : SubmissionReactivatedResult;
}