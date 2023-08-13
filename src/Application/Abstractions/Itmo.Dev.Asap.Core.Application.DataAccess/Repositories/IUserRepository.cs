using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IUserRepository
{
    IAsyncEnumerable<User> QueryAsync(UserQuery query, CancellationToken cancellationToken);

    Task<long> CountAsync(UserQuery query, CancellationToken cancellationToken);

    void Add(User user);

    void Update(User user);

    void AddRange(IEnumerable<User> users);
}