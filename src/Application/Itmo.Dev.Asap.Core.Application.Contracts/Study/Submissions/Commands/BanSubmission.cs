using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;

internal static class BanSubmission
{
    public record Command(Guid IssuerId, Guid StudentId, Guid AssignmentId, int? Code) : IRequest<Response>;

    public record Response(SubmissionDto Submission);
}