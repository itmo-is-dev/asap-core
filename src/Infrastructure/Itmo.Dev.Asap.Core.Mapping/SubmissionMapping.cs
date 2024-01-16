using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class SubmissionMapping
{
    public static SubmissionDto ToDto(this RatedSubmission submission)
    {
        return new SubmissionDto(
            submission.Submission.Id,
            submission.Submission.Code,
            submission.Submission.SubmissionDate.AsUtcDateTime(),
            submission.Submission.Student.UserId,
            submission.Submission.GroupAssignment.Id.AssignmentId,
            submission.Submission.Payload,
            submission.Submission.ExtraPoints.AsDto(),
            Points: submission.TotalPoints.AsDto(),
            submission.Submission.GroupAssignment.Assignment.ShortName,
            submission.Submission.State.Kind.AsDto());
    }

    public static SubmissionInfoDto ToInfoDto(this Submission submission)
    {
        return new SubmissionInfoDto(
            Id: submission.Id,
            CreatedAt: submission.SubmissionDateOnly.ToDateTime(new TimeOnly(0, 0)),
            State: submission.State.Kind.AsDto());
    }
}