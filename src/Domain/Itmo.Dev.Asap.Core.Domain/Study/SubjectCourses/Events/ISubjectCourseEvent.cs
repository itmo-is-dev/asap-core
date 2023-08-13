namespace Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;

public interface ISubjectCourseEvent
{
    ValueTask AcceptAsync(ISubjectCourseEventVisitor visitor, CancellationToken cancellationToken);
}