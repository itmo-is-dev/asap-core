using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Presentation.SignalR.Abstractions;
using Itmo.Dev.Asap.Core.Presentation.SignalR.Hubs;
using Itmo.Dev.Asap.Core.WebApi.Abstractions.Models.Queue;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Itmo.Dev.Asap.Core.Presentation.SignalR.Handlers;

internal class QueueUpdateNotificationHandler : INotificationHandler<QueueUpdated.Notification>
{
    private readonly IHubContext<QueueHub, IQueueHubClient> _hubContext;

    public QueueUpdateNotificationHandler(IHubContext<QueueHub, IQueueHubClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(QueueUpdated.Notification notification, CancellationToken cancellationToken)
    {
        string notificationGroup = string.Concat(notification.SubjectCourseId, notification.GroupId);

        var queue = new SubjectCourseQueueModel(
            notification.SubjectCourseId,
            notification.GroupId,
            notification.SubmissionsQueue);

        IQueueHubClient group = _hubContext.Clients.Group(notificationGroup);
        await group.SendUpdateQueueMessage(queue, cancellationToken);
    }
}