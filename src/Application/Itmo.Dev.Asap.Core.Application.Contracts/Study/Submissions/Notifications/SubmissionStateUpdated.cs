using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;

internal static class SubmissionStateUpdated
{
    public record Notification(SubmissionDto Submission) : INotification;
}