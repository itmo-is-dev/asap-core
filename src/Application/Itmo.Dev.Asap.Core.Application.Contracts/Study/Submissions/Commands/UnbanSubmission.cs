using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;

internal static class UnbanSubmission
{
    public record Command(Guid IssuerId, Guid StudentId, Guid AssignmentId, int? Code) : IRequest<Response>;

    public abstract record Response
    {
        public sealed record Success(SubmissionDto Submission) : Response;

        public sealed record InvalidMove(SubmissionStateDto SourceState) : Response;
    }
}