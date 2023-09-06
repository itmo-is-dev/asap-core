using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Queries;

internal static class FindStudentGroupsByQuery
{
    public record struct PageToken(Guid Id);

    public record Query(
        PageToken? PageToken,
        int PageSize,
        Guid[] ExcludedIds,
        string[] NamePatterns,
        Guid[] ExcludedSubjectCourseIds) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudentGroupDto> Groups);
}