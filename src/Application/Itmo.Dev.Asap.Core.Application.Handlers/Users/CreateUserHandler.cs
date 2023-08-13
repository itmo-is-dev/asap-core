using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands.CreateUser;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class CreateUserHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public CreateUserHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var user = new User(Guid.NewGuid(), request.FirstName, request.MiddleName, request.LastName);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        UserDto dto = user.ToDto();

        return new Response(dto);
    }
}