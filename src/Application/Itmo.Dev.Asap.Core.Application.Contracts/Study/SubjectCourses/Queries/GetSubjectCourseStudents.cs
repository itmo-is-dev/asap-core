using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;

public static class GetSubjectCourseStudents
{
    public record Query(Guid SubjectCourseId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<Guid> StudentIds);
}