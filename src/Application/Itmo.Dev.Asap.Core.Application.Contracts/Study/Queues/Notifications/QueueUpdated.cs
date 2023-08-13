using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;

public static class QueueUpdated
{
    public record Notification(Guid SubjectCourseId, Guid GroupId, SubmissionsQueueDto SubmissionsQueue) : INotification;
}