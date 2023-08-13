using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;

public static class SubjectCoursePointsOutdated
{
    public record Notification(Guid SubjectCourseId) : INotification;
}