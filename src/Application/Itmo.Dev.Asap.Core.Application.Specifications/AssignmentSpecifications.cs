using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class AssignmentSpecifications
{
    public static async Task<Assignment> GetByIdAsync(
        this IAssignmentRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = AssignmentQuery.Build(x => x.WithId(id));

        Assignment? assignment = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return assignment ?? throw EntityNotFoundException.For<Assignment>(id);
    }

    public static IAsyncEnumerable<Assignment> GetByIdsAsync(
        this IAssignmentRepository repository,
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        var query = AssignmentQuery.Build(x => x.WithIds(ids));
        return repository.QueryAsync(query, cancellationToken);
    }

    public static IAsyncEnumerable<Assignment> FindBySubjectCourseId(
        this IAssignmentRepository repository,
        Guid subjectCourseId,
        CancellationToken cancellationToken)
    {
        var query = AssignmentQuery.Build(x => x.WithSubjectCourseId(subjectCourseId));
        return repository.QueryAsync(query, cancellationToken);
    }
}