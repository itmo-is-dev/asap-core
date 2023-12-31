using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Study;

public partial class StudentAssignment : IEntity
{
    public StudentAssignment(
        Student student,
        Assignment assignment,
        IReadOnlyCollection<GroupAssignments.GroupAssignment> groupAssignments,
        IReadOnlyCollection<Submission> submissions,
        SubjectCourse subjectCourse)
        : this(assignment.Id, student.UserId)
    {
        Student = student;
        Assignment = assignment;
        GroupAssignments = groupAssignments;
        Submissions = submissions;
        SubjectCourse = subjectCourse;
    }

    [KeyProperty]
    public Student Student { get; }

    [KeyProperty]
    public Assignment Assignment { get; }

    public IReadOnlyCollection<GroupAssignments.GroupAssignment> GroupAssignments { get; }

    public IReadOnlyCollection<Submission> Submissions { get; }

    public SubjectCourse SubjectCourse { get; }

    public StudentAssignmentPoints? CalculatePoints()
    {
        IEnumerable<Submission> submissions = Submissions
            .Where(x => x.State.IsTerminalEffectiveState);

        (Submission submission, Points? points, bool isBanned) = submissions
            .Select(s => (
                submission: s,
                points: GetEffectivePoints(s),
                isBanned: s.State.Kind is SubmissionStateKind.Banned))
            .OrderByDescending(x => x.isBanned)
            .ThenByDescending(x => x.points)
            .FirstOrDefault();

        if (points is null && isBanned is false)
            return null;

        if (isBanned)
            points = null;

        return new StudentAssignmentPoints(
            Student,
            Assignment,
            isBanned,
            points ?? Points.None,
            GetPointPenalty(submission) ?? Points.None,
            submission.SubmissionDateOnly);
    }

    private Points? GetEffectivePoints(Submission submission)
    {
        return submission.Rating is not null
            ? submission.CalculateRatedSubmission(Assignment, SubjectCourse.DeadlinePolicy).TotalPoints
            : null;
    }

    private Points? GetPointPenalty(Submission submission)
    {
        if (submission.Rating is null)
            return null;

        Points? deadlineAppliedPoints = GetEffectivePoints(submission);

        if (deadlineAppliedPoints is null)
            return null;

        Points? points = Assignment.MaxPoints * submission.Rating;
        Points? penaltyPoints = points - deadlineAppliedPoints;

        return penaltyPoints;
    }
}