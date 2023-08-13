using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Queries;

public static class GetSubmissionsQueue
{
    public record Query(Guid SubjectCourseId, Guid StudentGroupId) : IRequest<Response>;

    public record Response(SubmissionsQueueDto SubmissionsQueue);
}