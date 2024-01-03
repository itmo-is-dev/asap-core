using Itmo.Dev.Asap.Core.Application.Dto.Students;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;

internal static class FindStudentsByQuery
{
    public record struct PageToken(Guid StudentId);

    public record Query(
        PageToken? PageToken,
        int PageSize,
        string[] NamePatterns,
        string[] GroupNamePatterns,
        int[] UniversityIds,
        Guid[] Ids) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudentDto> Students);
}