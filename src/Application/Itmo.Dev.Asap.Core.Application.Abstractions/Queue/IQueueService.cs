using Itmo.Dev.Asap.Core.Application.Dto.Tables;

namespace Itmo.Dev.Asap.Core.Application.Abstractions.Queue;

public interface IQueueService
{
    Task<SubmissionsQueueDto> GetSubmissionsQueueAsync(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken);
}