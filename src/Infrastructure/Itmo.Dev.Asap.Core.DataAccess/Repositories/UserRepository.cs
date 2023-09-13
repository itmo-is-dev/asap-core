using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class UserRepository : RepositoryBase<User, UserModel>, IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    protected override DbSet<UserModel> DbSet => _context.Users;

    public IAsyncEnumerable<User> QueryAsync(UserQuery query, CancellationToken cancellationToken)
    {
        IQueryable<UserModel> queryable = ApplyQuery(_context.Users, query);

        return queryable
            .Include(x => x.Associations)
            .AsAsyncEnumerable()
            .Select(UserMapper.MapTo);
    }

    public Task<long> CountAsync(UserQuery query, CancellationToken cancellationToken)
    {
        IQueryable<UserModel> queryable = ApplyQuery(_context.Users, query);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override UserModel MapFrom(User entity)
    {
        return UserMapper.MapFrom(entity);
    }

    protected override bool Equal(User entity, UserModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(UserModel model, User entity)
    {
        model.FirstName = entity.FirstName;
        model.MiddleName = entity.MiddleName;
        model.LastName = entity.LastName;
    }

    private IQueryable<UserModel> ApplyQuery(IQueryable<UserModel> queryable, UserQuery query)
    {
        if (query.Ids is not [])
        {
            queryable = queryable.Where(x => query.Ids.Contains(x.Id));
        }

        if (query.FullNamePatterns is not [])
        {
            string[] patterns = query.FullNamePatterns.Select(pattern => $"%{pattern}%").ToArray();

            queryable = queryable.Where(user =>
                patterns.Any(pattern => EF.Functions.ILike(user.FirstName, pattern))
                || patterns.Any(pattern => EF.Functions.ILike(user.MiddleName, pattern))
                || patterns.Any(pattern => EF.Functions.ILike(user.LastName, pattern)));
        }

        if (query.UniversityIds is not [])
        {
            queryable = queryable
                .Select(x => new
                {
                    user = x,
                    association = x.Associations.OfType<IsuUserAssociationModel>().Single(),
                })
                .Where(x => query.UniversityIds.Contains(x.association.UniversityId))
                .Select(x => x.user);
        }

        if (query.Cursor is not null)
        {
            queryable = queryable.OrderBy(x => x.Id);
        }

        if (query.OrderByLastName is not null)
        {
            queryable = query.OrderByLastName.Value switch
            {
                OrderDirection.Ascending =>
                    queryable is IOrderedQueryable<UserModel> ordered
                        ? ordered.ThenBy(x => x.LastName)
                        : queryable.OrderBy(x => x.LastName),

                OrderDirection.Descending =>
                    queryable is IOrderedQueryable<UserModel> ordered
                        ? ordered.ThenByDescending(x => x.LastName)
                        : queryable.OrderByDescending(x => x.LastName),

                _ => throw new ArgumentOutOfRangeException(nameof(query)),
            };
        }

        if (query.Cursor is not null)
        {
            queryable = queryable.Skip(query.Cursor.Value);
        }

        if (query.Limit is not null)
        {
            queryable = queryable.Take(query.Limit.Value);
        }

        return queryable;
    }
}