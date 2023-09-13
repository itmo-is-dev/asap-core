using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Mapping;

[Mapper]
internal static partial class SubjectCoursePointsUpdatedMapper
{
    public static partial SubjectCoursePointsUpdatedKey MapToKey(
        this SubjectCoursePointsUpdated.Notification notification);

    public static SubjectCoursePointsUpdatedValue MapToValue(
        this SubjectCoursePointsUpdated.Notification notification)
    {
        return new SubjectCoursePointsUpdatedValue
        {
            SubjectCourseId = notification.SubjectCourseId.ToString(),
            Points = new SubjectCoursePointsUpdatedValue.Types.SubjectCoursePoints
            {
                Assignments = { notification.Points.Assignments.Select(x => x.ToProto()) },
                Students = { notification.Points.StudentsPoints.Select(x => x.Student.ToProto()) },
                Points = { notification.Points.StudentsPoints.Select(x => x.ToProto()) },
            },
        };
    }

    private static partial SubjectCoursePointsUpdatedValue.Types.Assignment ToProto(this AssignmentDto assignment);

    private static partial SubjectCoursePointsUpdatedValue.Types.Student ToProto(this StudentDto student);

    private static SubjectCoursePointsUpdatedValue.Types.StudentPoints ToProto(this StudentPointsDto pointsDto)
    {
        return new SubjectCoursePointsUpdatedValue.Types.StudentPoints
        {
            StudentId = pointsDto.Student.User.Id.ToString(),
            Points = { pointsDto.Points.Select(MapToAssignmentPoints) },
        };
    }

    private static partial SubjectCoursePointsUpdatedValue.Types.AssignmentPoints MapToAssignmentPoints(
        AssignmentPointsDto source);

    private static Timestamp ToTimestamp(DateOnly dateOnly)
    {
        var dateTime = dateOnly.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.Zero));
        return Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
    }
}