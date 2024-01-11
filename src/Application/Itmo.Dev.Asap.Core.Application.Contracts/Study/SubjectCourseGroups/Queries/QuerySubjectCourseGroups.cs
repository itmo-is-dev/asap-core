using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Queries;

internal static class QuerySubjectCourseGroups
{
    public record Query(Guid SubjectCourseId, IEnumerable<Guid> Ids, IEnumerable<string> Names) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SubjectCourseGroupDto> SubjectCourseGroups);
}