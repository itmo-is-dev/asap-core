using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries.QueryUsers;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class QueryUsersHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QueryUsersHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = UserQuery.Build(builder => builder
            .WithFullNamePatterns(request.NamePatterns)
            .WithUniversityIds(request.UniversityIds)
            .WithCursor(request.PageToken?.Cursor)
            .WithLimit(request.PageSize));

        UserDto[] users = await _context.Users
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        PageToken? pageToken = users.Length < request.PageSize
            ? null
            : new PageToken(request.PageToken?.Cursor ?? 0 + users.Length);

        return new Response(users, pageToken);
    }
}