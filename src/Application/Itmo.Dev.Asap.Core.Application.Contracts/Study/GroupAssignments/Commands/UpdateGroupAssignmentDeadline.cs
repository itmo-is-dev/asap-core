using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands;

internal static class UpdateGroupAssignmentDeadline
{
    public record Command(Guid GroupId, Guid AssignmentId, DateOnly Deadline) : IRequest<Response>;

    public record Response(GroupAssignmentDto GroupAssignment);
}