using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class SubjectSpecifications
{
    public static async Task<Subject> GetByIdAsync(
        this ISubjectRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = SubjectQuery.Build(x => x.WithId(id));

        Subject? subject = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return subject ?? throw EntityNotFoundException.For<Subject>(id);
    }
}