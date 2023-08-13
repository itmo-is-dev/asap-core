using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;

internal class DeleteSubjectCourseGroup
{
    public record Command(Guid SubjectCourseId, Guid StudentGroupId) : IRequest;
}