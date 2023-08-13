using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IGroupAssignmentRepository
{
    IAsyncEnumerable<GroupAssignment> QueryAsync(GroupAssignmentQuery query, CancellationToken cancellationToken);

    void Update(GroupAssignment groupAssignment);
}