using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface ISubjectCourseRepository
{
    IAsyncEnumerable<SubjectCourse> QueryAsync(SubjectCourseQuery query, CancellationToken cancellationToken);

    void Add(SubjectCourse subjectCourse);

    ValueTask ApplyAsync(ISubjectCourseEvent evt, CancellationToken cancellationToken);
}