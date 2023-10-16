using Itmo.Dev.Asap.Core.Application.Dto.Students;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;

internal static class GetStudentsBySubjectCourseId
{
    public record Query(Guid SubjectCourseId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudentDto> Students);
}