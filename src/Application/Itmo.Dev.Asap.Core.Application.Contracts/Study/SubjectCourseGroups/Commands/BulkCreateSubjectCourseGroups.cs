using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;

internal static class BulkCreateSubjectCourseGroups
{
    public record Command(Guid SubjectCourseId, IReadOnlyCollection<Guid> StudentGroupIds) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SubjectCourseGroupDto> Groups);
}