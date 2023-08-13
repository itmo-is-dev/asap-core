using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Notifications;

internal static class AssignmentPointsUpdated
{
    public record Notification(AssignmentDto Assignment) : INotification;
}