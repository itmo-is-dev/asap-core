using Itmo.Dev.Asap.Core.Application.Dto.Students;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;

public static class GetSubjectCourseStudents
{
    public record Query(Guid SubjectCourseId, PageToken? PageToken, int PageSize) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudentDto> Students, PageToken? PageToken);

    public record PageToken(Guid UserId);
}