using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Application.Factories;

public static class SubmissionRateDtoFactory
{
    public static SubmissionRateDto CreateFromSubmission(
        Submission submission,
        SubjectCourse subjectCourse,
        Assignment assignment,
        GroupAssignment groupAssignment)
    {
        double? rating = null;

        if (submission.Rating is not null)
            rating = submission.Rating * 100;

        DeadlinePenalty? penalty = subjectCourse.DeadlinePolicy
            .FindEffectiveDeadlinePenalty(groupAssignment.Deadline, submission.SubmissionDateOnly);

        Points rawPoints = assignment.RatedWith(submission.Rating);
        Points points = penalty?.Apply(rawPoints) ?? rawPoints;
        Points penaltyPoints = rawPoints - points;

        points += submission.ExtraPoints ?? Points.None;

        var dto = new SubmissionRateDto(
            submission.Id,
            submission.Code,
            submission.State.Kind.ToString(),
            submission.SubmissionDate.Value,
            rating,
            rawPoints.Value,
            assignment.MaxPoints.Value,
            submission.ExtraPoints?.Value,
            penaltyPoints.Value,
            points.Value);

        return dto;
    }
}