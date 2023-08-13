using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;

public class MentorAddedEvent : ISubjectCourseEvent
{
    public MentorAddedEvent(Mentor mentor)
    {
        Mentor = mentor;
    }

    public Mentor Mentor { get; }

    public ValueTask AcceptAsync(ISubjectCourseEventVisitor visitor, CancellationToken cancellationToken)
    {
        return visitor.VisitAsync(this, cancellationToken);
    }
}