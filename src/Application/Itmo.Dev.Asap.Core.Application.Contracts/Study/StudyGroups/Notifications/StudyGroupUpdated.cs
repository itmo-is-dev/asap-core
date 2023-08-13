using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Notifications;

internal static class StudyGroupUpdated
{
    public record Notification(StudyGroupDto Group) : INotification;
}