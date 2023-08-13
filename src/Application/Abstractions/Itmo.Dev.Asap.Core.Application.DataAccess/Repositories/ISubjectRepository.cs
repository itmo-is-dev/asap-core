using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface ISubjectRepository
{
    IAsyncEnumerable<Subject> QueryAsync(SubjectQuery query, CancellationToken cancellationToken);

    void Add(Subject subject);

    void Update(Subject subject);
}