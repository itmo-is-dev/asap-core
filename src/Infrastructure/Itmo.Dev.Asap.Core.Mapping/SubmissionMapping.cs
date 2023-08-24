using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class SubmissionMapping
{
    public static SubmissionDto ToDto(this Submission submission, Points points)
    {
        return new SubmissionDto(
            submission.Id,
            submission.Code,
            submission.SubmissionDate.AsUtcDateTime(),
            submission.Student.UserId,
            submission.GroupAssignment.Id.AssignmentId,
            submission.Payload,
            submission.ExtraPoints.AsDto(),
            points.AsDto(),
            submission.GroupAssignment.Assignment.ShortName,
            submission.State.Kind.AsDto());
    }
}