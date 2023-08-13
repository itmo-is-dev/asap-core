using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Notification;

internal static class SubjectUpdated
{
    public record Notification(SubjectDto Subject) : INotification;
}