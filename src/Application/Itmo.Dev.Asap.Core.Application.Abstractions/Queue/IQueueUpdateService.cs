namespace Itmo.Dev.Asap.Core.Application.Abstractions.Queue;

public interface IQueueUpdateService
{
    void Update(Guid subjectCourseId, Guid studentGroupId);
}