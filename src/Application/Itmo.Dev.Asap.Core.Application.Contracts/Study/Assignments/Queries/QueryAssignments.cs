using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;

internal static class QueryAssignments
{
    public record Query(IEnumerable<Guid> Ids, IEnumerable<string> Names) : IRequest<Response>;

    public record Response(IReadOnlyCollection<AssignmentDto> Assignments);
}