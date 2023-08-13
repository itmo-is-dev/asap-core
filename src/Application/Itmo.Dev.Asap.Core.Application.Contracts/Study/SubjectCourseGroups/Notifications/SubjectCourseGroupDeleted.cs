using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;

internal static class SubjectCourseGroupDeleted
{
    public record Notification(Guid SubjectCourseId, Guid GroupId) : INotification;
}