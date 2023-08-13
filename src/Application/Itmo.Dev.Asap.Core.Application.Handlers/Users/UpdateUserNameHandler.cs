using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands.UpdateUserName;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class UpdateUserNameHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public UpdateUserNameHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        User user = await _context.Users.GetByIdAsync(request.UserId, cancellationToken);

        user.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName;
        user.LastName = request.LastName;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        UserDto dto = user.ToDto();

        return new Response(dto);
    }
}