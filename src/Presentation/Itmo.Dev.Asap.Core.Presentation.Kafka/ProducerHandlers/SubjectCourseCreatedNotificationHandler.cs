using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Itmo.Dev.Platform.Kafka.Producer.Models;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.ProducerHandlers;

internal class SubjectCourseCreatedNotificationHandler : INotificationHandler<SubjectCourseCreated.Notification>
{
    private readonly IKafkaMessageProducer<SubjectCourseCreatedKey, SubjectCourseCreatedValue> _producer;

    public SubjectCourseCreatedNotificationHandler(
        IKafkaMessageProducer<SubjectCourseCreatedKey, SubjectCourseCreatedValue> producer)
    {
        _producer = producer;
    }

    public async Task Handle(SubjectCourseCreated.Notification notification, CancellationToken cancellationToken)
    {
        var message = new ProducerKafkaMessage<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(
            notification.MapToKey(),
            notification.MapToValue());

        await _producer.ProduceAsync(message, cancellationToken);
    }
}