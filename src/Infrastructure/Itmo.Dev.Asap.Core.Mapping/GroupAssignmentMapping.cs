using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class GroupAssignmentMapping
{
    public static GroupAssignmentDto ToDto(this GroupAssignment groupAssignment)
    {
        return new GroupAssignmentDto(
            groupAssignment.Id.StudentGroupId,
            groupAssignment.Group.Name,
            groupAssignment.Id.AssignmentId,
            groupAssignment.Assignment.Title,
            groupAssignment.Deadline);
    }
}