using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries.FindUserByUniversityId;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class FindUserByUniversityIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public FindUserByUniversityIdHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users
            .QueryAsync(UserQuery.Build(x => x.WithUniversityId(request.UniversityId)), cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return new Response(user?.ToDto());
    }
}