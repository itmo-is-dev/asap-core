using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class UserSpecifications
{
    public static async Task<User?> FindByIdAsync(
        this IUserRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = UserQuery.Build(x => x.WithId(id));

        return await repository.QueryAsync(query, cancellationToken).SingleOrDefaultAsync(cancellationToken);
    }

    public static async Task<User> GetByIdAsync(
        this IUserRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        User? user = await FindByIdAsync(repository, id, cancellationToken);

        return user ?? throw EntityNotFoundException.For<User>(id);
    }
}