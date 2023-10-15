using Itmo.Dev.Asap.Core.Application.Dto.Students;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Notifications;

internal static class StudentAssignmentPointsUpdated
{
    public record Notification(IEnumerable<StudentAssignmentPointsDto> Points) : INotification;
}