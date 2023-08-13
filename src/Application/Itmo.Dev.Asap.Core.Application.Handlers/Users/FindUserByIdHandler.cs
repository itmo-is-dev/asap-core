using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries.FindUserById;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class FindUserByIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public FindUserByIdHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FindByIdAsync(request.UserId, cancellationToken);
        UserDto? dto = user?.ToDto();

        return new Response(dto);
    }
}