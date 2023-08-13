using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Groups;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IStudentGroupRepository
{
    IAsyncEnumerable<StudentGroup> QueryAsync(StudentGroupQuery query, CancellationToken cancellationToken);

    void Update(StudentGroup studentGroup);

    void Add(StudentGroup studentGroup);
}