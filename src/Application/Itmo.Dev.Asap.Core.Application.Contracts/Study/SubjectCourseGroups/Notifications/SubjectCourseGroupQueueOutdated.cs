using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;

public static class SubjectCourseGroupQueueOutdated
{
    public record Notification(Guid SubjectCourseId, Guid GroupId) : INotification;
}