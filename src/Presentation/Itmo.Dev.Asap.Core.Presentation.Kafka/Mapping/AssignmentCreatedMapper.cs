using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Notifications;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;

[Mapper]
internal static partial class AssignmentCreatedMapper
{
    public static partial AssignmentCreatedKey MapToKey(this AssignmentCreated.Notification notification);

    public static partial AssignmentCreatedValue MapToValue(this AssignmentCreated.Notification notification);
}