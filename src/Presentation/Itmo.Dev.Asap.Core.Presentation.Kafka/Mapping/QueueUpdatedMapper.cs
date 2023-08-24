using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
internal static partial class QueueUpdatedMapper
{
    public static partial QueueUpdatedKey MapToKey(this QueueUpdated.Notification notification);

    public static QueueUpdatedValue MapToValue(this QueueUpdated.Notification notification)
    {
        QueueUpdatedValue value = MapToQueueUpdatedValue(notification);
        MapToStudents(value.SubmissionsQueue.Students, notification.SubmissionsQueue.Students);

        return value;
    }

    private static partial QueueUpdatedValue MapToQueueUpdatedValue(this QueueUpdated.Notification notification);

    private static partial QueueUpdatedValue.Types.Student MapToStudent(this StudentDto student);

    private static Timestamp MapToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(dateTime);

    private static void MapToStudents(
        IDictionary<string, QueueUpdatedValue.Types.Student> field,
        IEnumerable<StudentDto> students)
    {
        foreach (StudentDto student in students)
        {
            field.Add(student.User.Id.ToString(), student.MapToStudent());
        }
    }
}