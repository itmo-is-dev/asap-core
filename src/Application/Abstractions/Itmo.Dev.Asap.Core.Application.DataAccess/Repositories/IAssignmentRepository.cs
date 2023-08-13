using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IAssignmentRepository
{
    IAsyncEnumerable<Assignment> QueryAsync(AssignmentQuery query, CancellationToken cancellationToken);

    void Update(Assignment assignment);
}