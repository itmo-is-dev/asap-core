using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;

internal static class FindUserByUniversityId
{
    public record Query(int UniversityId) : IRequest<Response>;

    public record Response(UserDto? User);
}