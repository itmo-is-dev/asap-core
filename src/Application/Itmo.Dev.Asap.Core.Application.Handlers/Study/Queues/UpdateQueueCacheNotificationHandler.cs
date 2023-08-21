using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Queues;

internal class UpdateQueueCacheNotificationHandler
    : INotificationHandler<QueueUpdated.Notification>
{
    private readonly IMemoryCache _cache;

    public UpdateQueueCacheNotificationHandler(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task Handle(QueueUpdated.Notification notification, CancellationToken cancellationToken)
    {
        string cacheKey = string.Concat(notification.SubjectCourseId, notification.StudentGroupId);
        _cache.Remove(cacheKey);
        _cache.Set(cacheKey, notification.SubmissionsQueue);

        return Task.CompletedTask;
    }
}