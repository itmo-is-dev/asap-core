using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;

public static class SubjectCourseCreated
{
    public record Notification(SubjectCourseDto SubjectCourse, string CorrelationId) : INotification;
}