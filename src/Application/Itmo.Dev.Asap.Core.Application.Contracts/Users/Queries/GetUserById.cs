using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;

public static class GetUserById
{
    public record Query(Guid UserId) : IRequest<Response>;

    public record Response(UserDto User);
}