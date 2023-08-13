using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;

internal static class SubmissionPointsUpdated
{
    public record Notification(SubmissionDto Submission) : INotification;
}