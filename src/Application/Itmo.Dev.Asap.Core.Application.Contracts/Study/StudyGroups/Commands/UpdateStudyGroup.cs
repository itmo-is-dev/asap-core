using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Commands;

internal static class UpdateStudyGroup
{
    public record Command(Guid Id, string Name) : IRequest<Response>;

    public record Response(StudyGroupDto Group);
}