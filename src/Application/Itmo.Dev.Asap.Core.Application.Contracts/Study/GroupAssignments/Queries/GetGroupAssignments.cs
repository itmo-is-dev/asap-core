using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Queries;

internal static class GetGroupAssignments
{
    public record Query(Guid AssignmentId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<GroupAssignmentDto> GroupAssignments);
}