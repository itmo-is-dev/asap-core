using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;

internal static class QueryUsers
{
    public record Query(
        PageToken? PageToken,
        int PageSize,
        string[] NamePatterns,
        int[] UniversityIds) : IRequest<Response>;

    public record Response(IReadOnlyCollection<UserDto> Users, PageToken? PageToken);

    public record struct PageToken(int Cursor);
}