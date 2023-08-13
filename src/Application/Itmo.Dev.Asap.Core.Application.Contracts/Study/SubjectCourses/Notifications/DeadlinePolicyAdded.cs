using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;

internal static class DeadlinePolicyAdded
{
    public record Notification(Guid SubjectCourseId) : INotification;
}