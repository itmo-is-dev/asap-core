using Itmo.Dev.Asap.Core.Application.Dto.Study;
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
}