using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Notifications;
using Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Itmo.Dev.Platform.Kafka.Producer.Models;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.ProducerHandlers;

internal class AssignmentCreatedNotificationHandler : INotificationHandler<AssignmentCreated.Notification>
{
    private readonly IKafkaMessageProducer<AssignmentCreatedKey, AssignmentCreatedValue> _producer;

    public AssignmentCreatedNotificationHandler(
        IKafkaMessageProducer<AssignmentCreatedKey, AssignmentCreatedValue> producer)
    {
        _producer = producer;
    }

    public async Task Handle(AssignmentCreated.Notification notification, CancellationToken cancellationToken)
    {
        var message = new ProducerKafkaMessage<AssignmentCreatedKey, AssignmentCreatedValue>(
            notification.MapToKey(),
            notification.MapToValue());

        await _producer.ProduceAsync(message, cancellationToken);
    }
}