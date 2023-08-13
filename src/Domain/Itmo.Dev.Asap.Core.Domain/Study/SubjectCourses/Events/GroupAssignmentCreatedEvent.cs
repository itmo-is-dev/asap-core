using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;

namespace Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;

public class GroupAssignmentCreatedEvent : ISubjectCourseEvent
{
    public GroupAssignmentCreatedEvent(GroupAssignment groupAssignment)
    {
        GroupAssignment = groupAssignment;
    }

    public GroupAssignment GroupAssignment { get; }

    public ValueTask AcceptAsync(ISubjectCourseEventVisitor visitor, CancellationToken cancellationToken)
    {
        return visitor.VisitAsync(this, cancellationToken);
    }
}