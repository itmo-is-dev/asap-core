using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Producer;
using Itmo.Dev.Platform.Kafka.Producer.Models;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Notifications.StudentAssignmentPointsUpdated;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.ProducerHandlers;

internal class StudentPointsUpdatedNotificationHandler : INotificationHandler<Notification>
{
    private readonly IKafkaMessageProducer<StudentPointsUpdatedKey, StudentPointsUpdatedValue> _producer;

    public StudentPointsUpdatedNotificationHandler(
        IKafkaMessageProducer<StudentPointsUpdatedKey, StudentPointsUpdatedValue> producer)
    {
        _producer = producer;
    }

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        IEnumerable<ProducerKafkaMessage<StudentPointsUpdatedKey, StudentPointsUpdatedValue>> m = notification.Points
            .Select(x => new ProducerKafkaMessage<StudentPointsUpdatedKey, StudentPointsUpdatedValue>(
                MapToKey(x),
                MapToValue(x)));

        await _producer.ProduceAsync(m.ToAsyncEnumerable(), cancellationToken);
    }

    private StudentPointsUpdatedKey MapToKey(StudentAssignmentPointsDto points)
    {
        return new StudentPointsUpdatedKey
        {
            SubjectCourseId = points.SubjectCourseId.ToString(),
        };
    }

    private StudentPointsUpdatedValue MapToValue(StudentAssignmentPointsDto points)
    {
        return new StudentPointsUpdatedValue
        {
            StudentId = points.StudentId.ToString(),
            AssignmentId = points.AssignmentId.ToString(),
            Date = Timestamp.FromDateTimeOffset(points.Date),
            IsBanned = points.IsBanned,
            Points = points.Points,
        };
    }
}