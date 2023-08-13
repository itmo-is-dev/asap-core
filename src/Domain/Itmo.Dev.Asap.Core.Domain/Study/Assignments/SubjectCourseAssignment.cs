using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Study.Assignments;

public partial class SubjectCourseAssignment : IEntity
{
    private readonly HashSet<StudentGroupInfo> _groups;

    public SubjectCourseAssignment(AssignmentInfo assignment, HashSet<StudentGroupInfo> groups)
    {
        AssignmentId = assignment.Id;
        Assignment = assignment;
        _groups = groups;
    }

    [KeyProperty]
    public Guid AssignmentId { get; }

    public AssignmentInfo Assignment { get; }

    public IReadOnlyCollection<StudentGroupInfo> Groups => _groups;

    public GroupAssignment AddGroup(StudentGroupInfo group, DateOnly deadline)
    {
        if (_groups.Contains(group))
            throw new DomainInvalidOperationException($"Assignment for Group = {group.Name} already exists");

        var groupAssignmentId = new GroupAssignmentId(group.Id, AssignmentId);

        return new GroupAssignment(groupAssignmentId, deadline, group, Assignment);
    }
}