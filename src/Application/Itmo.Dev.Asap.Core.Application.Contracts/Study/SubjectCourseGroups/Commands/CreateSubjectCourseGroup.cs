using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;

internal static class CreateSubjectCourseGroup
{
    public record Command(Guid SubjectCourseId, Guid StudentGroupId) : IRequest<Response>;

    public record Response(SubjectCourseGroupDto SubjectCourseGroup);
}