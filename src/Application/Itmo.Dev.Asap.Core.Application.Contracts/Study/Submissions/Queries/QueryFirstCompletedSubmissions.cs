using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Queries;

internal static class QueryFirstCompletedSubmissions
{
    public record Query(Guid SubjectCourseId, PageToken? PageToken, int PageSize) : IRequest<Response>;

    public sealed record PageToken(Guid UserId, Guid AssignmentId);

    public record Response(IReadOnlyCollection<FirstSubmissionDto> Submissions, PageToken? PageToken);
}