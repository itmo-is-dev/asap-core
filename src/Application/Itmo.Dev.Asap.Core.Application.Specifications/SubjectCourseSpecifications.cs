using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class SubjectCourseSpecifications
{
    public static async Task<SubjectCourse> GetByIdAsync(
        this ISubjectCourseRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithId(id));

        SubjectCourse? subjectCourse = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return subjectCourse ?? throw EntityNotFoundException.For<SubjectCourse>(id);
    }

    public static async Task<SubjectCourse> GetByAssignmentId(
        this ISubjectCourseRepository repository,
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithAssignmentId(assignmentId));

        SubjectCourse? subjectCourse = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (subjectCourse is not null)
            return subjectCourse;

        throw new EntityNotFoundException($"SubjectCourse for assignment {assignmentId} not found");
    }

    public static async Task<SubjectCourse> GetBySubmissionIdAsync(
        this ISubjectCourseRepository repository,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithSubmissionId(submissionId));

        SubjectCourse? subjectCourse = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (subjectCourse is not null)
            return subjectCourse;

        throw new EntityNotFoundException($"SubjectCourse for submission {submissionId} not found");
    }
}