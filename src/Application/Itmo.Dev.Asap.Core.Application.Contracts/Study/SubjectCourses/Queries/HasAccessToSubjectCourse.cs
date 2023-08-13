using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;

internal static class HasAccessToSubjectCourse
{
    public record Query(Guid SubjectCourseId) : IRequest<Response>;

    public record Response(bool HasAccess);
}