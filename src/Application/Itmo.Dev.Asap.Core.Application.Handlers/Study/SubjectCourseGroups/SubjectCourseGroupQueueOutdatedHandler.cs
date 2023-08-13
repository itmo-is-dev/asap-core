using Itmo.Dev.Asap.Core.Application.Abstractions.Queue;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications.SubjectCourseGroupQueueOutdated;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;

internal class SubjectCourseGroupQueueOutdatedHandler : INotificationHandler<Notification>
{
    private readonly ILogger<SubjectCourseGroupQueueOutdatedHandler> _logger;
    private readonly IQueueService _queueService;
    private readonly IPublisher _publisher;

    public SubjectCourseGroupQueueOutdatedHandler(
        ILogger<SubjectCourseGroupQueueOutdatedHandler> logger,
        IQueueService queueService,
        IPublisher publisher)
    {
        _logger = logger;
        _queueService = queueService;
        _publisher = publisher;
    }

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Error while updating queue for subject course {SubjectCourseId} group {StudentGroupId}",
                notification.SubjectCourseId,
                notification.GroupId);
        }
    }

    private async Task ExecuteAsync(Notification notification, CancellationToken cancellationToken)
    {
        SubmissionsQueueDto submissionsQueue = await _queueService.GetSubmissionsQueueAsync(
            notification.SubjectCourseId,
            notification.GroupId,
            cancellationToken);

        var updatedNotification = new QueueUpdated.Notification(
            notification.SubjectCourseId,
            notification.GroupId,
            submissionsQueue);

        await _publisher.Publish(updatedNotification, cancellationToken);
    }
}