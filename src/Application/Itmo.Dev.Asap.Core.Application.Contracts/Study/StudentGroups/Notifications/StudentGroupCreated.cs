using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Notifications;

internal static class StudentGroupCreated
{
    public record Notification(StudentGroupDto Group) : INotification;
}