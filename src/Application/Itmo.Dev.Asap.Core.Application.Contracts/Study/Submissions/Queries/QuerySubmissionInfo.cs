using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Queries;

internal static class QuerySubmissionInfo
{
    public record Query(IEnumerable<Guid> Ids) : IRequest<Response>;

    public record Response(IReadOnlyCollection<SubmissionInfoDto> Submissions);
}