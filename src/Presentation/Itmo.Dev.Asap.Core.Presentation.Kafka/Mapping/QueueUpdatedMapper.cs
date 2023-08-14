using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
internal static partial class QueueUpdatedMapper
{
    public static partial QueueUpdatedKey MapToKey(this QueueUpdated.Notification notification);

    public static partial QueueUpdatedValue MapToValue(this QueueUpdated.Notification notification);

    public static Timestamp MapToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(dateTime);
}