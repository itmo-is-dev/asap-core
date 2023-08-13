using Itmo.Dev.Asap.Core.Domain.Study.Assignments;

namespace Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;

public class SubjectCourseAssignmentCreatedEvent : ISubjectCourseEvent
{
    public SubjectCourseAssignmentCreatedEvent(SubjectCourse subjectCourse, Assignment assignment)
    {
        SubjectCourse = subjectCourse;
        Assignment = assignment;
    }

    public SubjectCourse SubjectCourse { get; }

    public Assignment Assignment { get; }

    public ValueTask AcceptAsync(ISubjectCourseEventVisitor visitor, CancellationToken cancellationToken)
    {
        return visitor.VisitAsync(this, cancellationToken);
    }
}