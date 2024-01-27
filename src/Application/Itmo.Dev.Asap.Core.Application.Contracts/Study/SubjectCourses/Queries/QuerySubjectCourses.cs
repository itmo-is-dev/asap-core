using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;

internal static class QuerySubjectCourses
{
    public record Query(IEnumerable<Guid> Ids) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SubjectCourseDto> SubjectCourses);
}