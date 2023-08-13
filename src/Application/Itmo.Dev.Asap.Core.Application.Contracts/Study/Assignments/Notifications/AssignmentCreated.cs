using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Notifications;

public static class AssignmentCreated
{
    public record Notification(AssignmentDto Assignment) : INotification;
}