using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class GroupAssignmentMapper
{
    public static GroupAssignment MapTo(
        GroupAssignmentModel model,
        string groupName,
        string assignmentTitle,
        string assignmentShortName)
    {
        return new GroupAssignment(
            new GroupAssignmentId(model.StudentGroupId, model.AssignmentId),
            model.Deadline,
            new StudentGroupInfo(model.StudentGroupId, groupName),
            new AssignmentInfo(model.AssignmentId, assignmentTitle, assignmentShortName));
    }

    public static GroupAssignmentModel MapFrom(GroupAssignment entity)
    {
        return new GroupAssignmentModel(entity.Id.StudentGroupId, entity.Id.AssignmentId, entity.Deadline);
    }
}