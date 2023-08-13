using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;

internal static class UpdateSubmissionDate
{
    public record Command(
        Guid IssuerId,
        Guid UserId,
        Guid AssignmentId,
        int? Code,
        DateOnly Date) : IRequest<Response>;

    public record Response(SubmissionRateDto Submission);
}