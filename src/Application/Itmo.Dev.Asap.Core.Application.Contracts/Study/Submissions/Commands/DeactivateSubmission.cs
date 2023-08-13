using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;

internal static class DeactivateSubmission
{
    public record Command(Guid SubmissionId) : IRequest<Response>;

    public record Response(SubmissionDto Submission);
}