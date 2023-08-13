using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Itmo.Dev.Platform.Kafka.Producer.Models;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.ProducerHandlers;

internal class SubjectCoursePointsUpdatedNotificationHandler
    : INotificationHandler<SubjectCoursePointsUpdated.Notification>
{
    private readonly IKafkaMessageProducer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue> _producer;

    public SubjectCoursePointsUpdatedNotificationHandler(
        IKafkaMessageProducer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue> producer)
    {
        _producer = producer;
    }

    public async Task Handle(SubjectCoursePointsUpdated.Notification notification, CancellationToken cancellationToken)
    {
        var message = new ProducerKafkaMessage<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(
            notification.MapToKey(),
            notification.MapToValue());

        await _producer.ProduceAsync(message, cancellationToken);
    }
}