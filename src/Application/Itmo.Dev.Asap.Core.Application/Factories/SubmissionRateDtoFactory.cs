using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Application.Factories;

public static class SubmissionRateDtoFactory
{
    public static SubmissionRateDto CreateFromRatedSubmission(
        RatedSubmission ratedSubmission,
        Assignment assignment)
    {
        Submission submission = ratedSubmission.Submission;

        var dto = new SubmissionRateDto(
            submission.Id,
            submission.Code,
            submission.State.Kind.ToString(),
            submission.SubmissionDate.Value,
            Rating: submission.Rating * 100,
            RawPoints: ratedSubmission.RawPoints.Value,
            MaxRawPoints: assignment.MaxPoints.Value,
            ExtraPoints: submission.ExtraPoints?.Value,
            PenaltyPoints: ratedSubmission.PointPenalty.Value,
            TotalPoints: ratedSubmission.TotalPoints.Value);

        return dto;
    }
}