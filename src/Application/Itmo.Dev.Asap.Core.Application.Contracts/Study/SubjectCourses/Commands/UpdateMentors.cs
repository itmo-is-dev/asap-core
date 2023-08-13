using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;

public static class UpdateMentors
{
    public record Command(Guid SubjectCourseId, IReadOnlyCollection<Guid> UserIds) : IRequest;
}