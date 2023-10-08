using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands;

internal static class UpdateGroupAssignmentDeadlines
{
    public record Command(Guid AssignmentId, DateOnly Deadline, IEnumerable<Guid> GroupIds) : IRequest<Response>;

    public record Response(IEnumerable<GroupAssignmentDto> GroupAssignments);
}