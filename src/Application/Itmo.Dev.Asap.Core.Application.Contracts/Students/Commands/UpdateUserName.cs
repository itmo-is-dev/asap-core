using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;

internal static class UpdateUserName
{
    public record Command(Guid UserId, string FirstName, string MiddleName, string LastName) : IRequest<Response>;

    public record Response(UserDto User);
}