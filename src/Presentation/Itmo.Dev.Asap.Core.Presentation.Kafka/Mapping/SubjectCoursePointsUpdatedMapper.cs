using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;

[Mapper]
internal static partial class SubjectCoursePointsUpdatedMapper
{
    public static partial SubjectCoursePointsUpdatedKey MapToKey(
        this SubjectCoursePointsUpdated.Notification notification);

    public static partial SubjectCoursePointsUpdatedValue MapToValue(
        this SubjectCoursePointsUpdated.Notification notification);
}