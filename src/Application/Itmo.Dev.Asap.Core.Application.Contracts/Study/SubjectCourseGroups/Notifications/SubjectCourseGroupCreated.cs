using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;

internal static class SubjectCourseGroupCreated
{
    public record Notification(SubjectCourseGroupDto Group) : INotification;
}