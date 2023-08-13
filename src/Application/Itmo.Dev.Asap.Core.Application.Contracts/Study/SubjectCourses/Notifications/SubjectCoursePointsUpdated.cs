using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;

public static class SubjectCoursePointsUpdated
{
    public record Notification(Guid SubjectCourseId, SubjectCoursePointsDto Points) : INotification;
}