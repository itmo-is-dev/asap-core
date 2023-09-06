using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Commands;

internal static class UpdateAssignmentPoints
{
    public record Command(Guid AssignmentId, double? MinPoints, double? MaxPoints) : IRequest<Response>;

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(AssignmentDto Assignment) : Response;

        public sealed record MaxPointsLessThanMinPoints : Response;
    }
}