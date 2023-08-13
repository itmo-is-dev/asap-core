using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;

internal static class UpdateUserUniversityId
{
    public record Command(Guid UserId, int UniversityId) : IRequest<Response>;

    public record Response(UserDto User);
}