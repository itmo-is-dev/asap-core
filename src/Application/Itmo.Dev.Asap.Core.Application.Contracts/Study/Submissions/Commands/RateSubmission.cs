using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;

internal static class RateSubmission
{
    public record Command(
        Guid IssuerId,
        Guid SubmissionId,
        double? RatingPercent,
        double? ExtraPoints) : IRequest<Response>;

    public record Response(SubmissionRateDto Submission);
}