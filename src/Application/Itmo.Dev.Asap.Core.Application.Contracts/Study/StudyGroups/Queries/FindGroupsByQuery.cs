using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries;

internal static class FindGroupsByQuery
{
    public record struct PageToken(Guid Id);

    public record Query(PageToken? PageToken, int PageSize, string[] NamePatterns) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudyGroupDto> Groups);
}