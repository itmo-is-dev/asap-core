using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries.FindCurrentUser;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class FindCurrentUserHandler : IRequestHandler<Query, Response>
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersistenceContext _context;

    public FindCurrentUserHandler(ICurrentUser currentUser, IPersistenceContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FindByIdAsync(_currentUser.Id, cancellationToken);
        UserDto? dto = user?.ToDto();

        return new Response(dto);
    }
}