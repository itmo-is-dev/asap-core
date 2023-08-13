using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;

internal static class SubjectCourseUpdated
{
    public record Notification(SubjectCourseDto SubjectCourse) : INotification;
}