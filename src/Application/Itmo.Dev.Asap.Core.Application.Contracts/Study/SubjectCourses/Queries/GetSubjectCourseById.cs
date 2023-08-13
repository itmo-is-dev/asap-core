using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;

internal static class GetSubjectCourseById
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(SubjectCourseDto SubjectCourse);
}