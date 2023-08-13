using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Commands;

internal static class CreateAssignment
{
    public record Command(
        Guid SubjectCourseId,
        string Title,
        string ShortName,
        int Order,
        double MinPoints,
        double MaxPoints) : IRequest<Response>;

    public record Response(AssignmentDto Assignment);
}