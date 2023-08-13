using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;

internal static class GetSubjectCoursesBySubjectId
{
    public record Query(Guid SubjectId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SubjectCourseDto> Courses);
}