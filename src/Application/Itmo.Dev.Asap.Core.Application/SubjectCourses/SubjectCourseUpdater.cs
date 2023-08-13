using Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Tools;

namespace Itmo.Dev.Asap.Core.Application.SubjectCourses;

public class SubjectCourseUpdater : ISubjectCourseUpdateService
{
    private readonly ConcurrentHashSet<Guid> _pointUpdates;

    public SubjectCourseUpdater()
    {
        _pointUpdates = new ConcurrentHashSet<Guid>();
    }

    public IReadOnlyCollection<Guid> PointUpdates => _pointUpdates.GetAndClearValues();

    public void UpdatePoints(Guid subjectCourseId)
    {
        _pointUpdates.Add(subjectCourseId);
    }
}