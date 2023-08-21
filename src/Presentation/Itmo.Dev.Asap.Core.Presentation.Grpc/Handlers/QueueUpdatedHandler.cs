using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Handlers;

internal class QueueUpdatedHandler : INotificationHandler<QueueUpdated.Notification>
{
    private readonly IObserver<QueueUpdated.Notification> _observer;

    public QueueUpdatedHandler(IObserver<QueueUpdated.Notification> observer)
    {
        _observer = observer;
    }

    public Task Handle(QueueUpdated.Notification notification, CancellationToken cancellationToken)
    {
        _observer.OnNext(notification);
        return Task.CompletedTask;
    }
}