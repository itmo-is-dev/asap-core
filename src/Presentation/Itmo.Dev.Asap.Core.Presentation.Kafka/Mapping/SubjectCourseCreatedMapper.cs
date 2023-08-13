using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;

[Mapper]
internal static partial class SubjectCourseCreatedMapper
{
    public static partial SubjectCourseCreatedKey MapToKey(this SubjectCourseCreated.Notification notification);

    public static partial SubjectCourseCreatedValue MapToValue(this SubjectCourseCreated.Notification notification);
}