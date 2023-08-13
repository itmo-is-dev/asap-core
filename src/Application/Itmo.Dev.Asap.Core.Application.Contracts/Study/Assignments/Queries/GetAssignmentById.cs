using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;

internal static class GetAssignmentById
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(AssignmentDto Assignment);
}