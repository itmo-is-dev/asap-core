using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;

public static class CreateUser
{
    public record Command(string FirstName, string MiddleName, string LastName) : IRequest<Response>;

    public record Response(UserDto User);
}