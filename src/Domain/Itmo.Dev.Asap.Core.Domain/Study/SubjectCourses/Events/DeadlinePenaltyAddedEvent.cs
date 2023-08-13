using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;

namespace Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;

public class DeadlinePenaltyAddedEvent : ISubjectCourseEvent
{
    public DeadlinePenaltyAddedEvent(SubjectCourse subjectCourse, DeadlinePenalty penalty)
    {
        SubjectCourse = subjectCourse;
        Penalty = penalty;
    }

    public SubjectCourse SubjectCourse { get; }

    public DeadlinePenalty Penalty { get; }

    public ValueTask AcceptAsync(ISubjectCourseEventVisitor visitor, CancellationToken cancellationToken)
    {
        return visitor.VisitAsync(this, cancellationToken);
    }
}