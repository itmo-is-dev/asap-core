using Itmo.Dev.Asap.Core.WebApi.Abstractions.Models.Queue;

namespace Itmo.Dev.Asap.Core.Presentation.SignalR.Abstractions;

public interface IQueueHubClient
{
    Task SendUpdateQueueMessage(SubjectCourseQueueModel submissionsQueue, CancellationToken cancellationToken);

    Task SendError(string message);
}