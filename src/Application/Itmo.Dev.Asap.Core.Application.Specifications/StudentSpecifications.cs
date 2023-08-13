using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Students;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class StudentSpecifications
{
    public static IAsyncEnumerable<Student> GetStudentsBySubjectCourseIdAsync(
        this IStudentRepository repository,
        Guid subjectCourseId,
        CancellationToken cancellationToken)
    {
        var query = StudentQuery.Build(x => x.WithSubjectCourseId(subjectCourseId));
        return repository.QueryAsync(query, cancellationToken);
    }

    public static async Task<Student> GetByIdAsync(
        this IStudentRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = StudentQuery.Build(x => x.WithId(id));

        Student? student = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return student ?? throw EntityNotFoundException.For<Student>(id);
    }
}