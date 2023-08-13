using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries;

internal static class BulkGetStudyGroups
{
    public record Query(IReadOnlyCollection<Guid> Ids) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudyGroupDto> Groups);
}