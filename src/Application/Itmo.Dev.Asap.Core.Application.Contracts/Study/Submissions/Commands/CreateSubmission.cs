using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;

public static class CreateSubmission
{
    public record Command(Guid IssuerId, Guid StudentId, Guid AssignmentId, string Payload) : IRequest<Response>;

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(SubmissionDto Submission) : Response;

        public sealed record Unauthorized : Response;
    }
}