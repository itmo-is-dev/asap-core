using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Notifications;

internal static class GroupAssignmentDeadlineUpdated
{
    public record Notification(GroupAssignmentDto GroupAssignment) : INotification;
}