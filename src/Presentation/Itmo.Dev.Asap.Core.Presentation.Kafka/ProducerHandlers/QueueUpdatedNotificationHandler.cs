using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Itmo.Dev.Platform.Kafka.Producer.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.ProducerHandlers;

internal class QueueUpdatedNotificationHandler : INotificationHandler<QueueUpdated.Notification>
{
    private readonly IKafkaMessageProducer<QueueUpdatedKey, QueueUpdatedValue> _producer;
    private readonly ILogger<QueueUpdatedNotificationHandler> _logger;

    public QueueUpdatedNotificationHandler(
        IKafkaMessageProducer<QueueUpdatedKey, QueueUpdatedValue> producer,
        ILogger<QueueUpdatedNotificationHandler> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task Handle(QueueUpdated.Notification notification, CancellationToken cancellationToken)
    {
        var message = new ProducerKafkaMessage<QueueUpdatedKey, QueueUpdatedValue>(
            notification.MapToKey(),
            notification.MapToValue());

        _logger.LogInformation(
            "Sending queue updated message subject course = {SubjectCourseId}, student group = {StudentGroupId}",
            notification.SubjectCourseId,
            notification.StudentGroupId);

        await _producer.ProduceAsync(message, cancellationToken);
    }
}