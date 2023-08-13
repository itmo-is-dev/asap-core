using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;

internal static class UpdateSubjectCourse
{
    public record Command(Guid Id, string Title) : IRequest<Response>;

    public record Response(SubjectCourseDto SubjectCourse);
}