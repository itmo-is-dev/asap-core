using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Itmo.Dev.Platform.Kafka.Producer.Models;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.ProducerHandlers;

internal class QueueUpdatedNotificationHandler : INotificationHandler<QueueUpdated.Notification>
{
    private readonly IKafkaMessageProducer<QueueUpdatedKey, QueueUpdatedValue> _producer;

    public QueueUpdatedNotificationHandler(IKafkaMessageProducer<QueueUpdatedKey, QueueUpdatedValue> producer)
    {
        _producer = producer;
    }

    public async Task Handle(QueueUpdated.Notification notification, CancellationToken cancellationToken)
    {
        var message = new ProducerKafkaMessage<QueueUpdatedKey, QueueUpdatedValue>(
            notification.MapToKey(),
            notification.MapToValue());

        await _producer.ProduceAsync(message, cancellationToken);
    }
}