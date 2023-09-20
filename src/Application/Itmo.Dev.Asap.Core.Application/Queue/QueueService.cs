using Itmo.Dev.Asap.Core.Application.Abstractions.Queue;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Queue;
using Itmo.Dev.Asap.Core.Domain.Queue.Building;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Mapping;

namespace Itmo.Dev.Asap.Core.Application.Queue;

#pragma warning disable CA1506
public class QueueService : IQueueService
{
    private readonly IPersistenceContext _context;

    public QueueService(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<SubmissionsQueueDto> GetSubmissionsQueueAsync(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken)
    {
        StudentGroup group = await _context.StudentGroups.GetByIdAsync(studentGroupId, cancellationToken);

        SubmissionQueue queue = new DefaultQueueBuilder(studentGroupId, subjectCourseId).Build();

        var filterVisitor = new FilterCriteriaVisitor(new SubmissionQuery.Builder());
        queue.AcceptFilterCriteriaVisitor(filterVisitor);

        IAsyncEnumerable<Submission> submissionsEnumerable = _context.Submissions
            .QueryAsync(filterVisitor.Builder.Build(), cancellationToken);

        var evaluatorVisitor = new EvaluatorCriteriaVisitor(_context, subjectCourseId);
        submissionsEnumerable = queue.OrderSubmissionsAsync(submissionsEnumerable, evaluatorVisitor, cancellationToken);

        Submission[] submissions = await submissionsEnumerable.ToArrayAsync(cancellationToken);

        IEnumerable<Guid> assignmentIds = submissions.Select(x => x.GroupAssignment.Assignment.Id).Distinct();

        Assignment[] assignments = await _context.Assignments
            .GetByIdsAsync(assignmentIds, cancellationToken)
            .ToArrayAsync(cancellationToken);

        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByIdAsync(subjectCourseId, cancellationToken);

        RatedSubmission[] ratedSubmissions = submissions
            .Join(
                assignments,
                x => x.GroupAssignment.Assignment.Id,
                x => x.Id,
                (s, a) => (s, a))
            .Select(x => x.s.CalculateRatedSubmission(x.a, subjectCourse.DeadlinePolicy))
            .ToArray();

        SubmissionDto[] submissionDto = ratedSubmissions.Select(x => x.ToDto()).ToArray();

        StudentDto[] students = ratedSubmissions
            .Select(x => x.Submission.Student)
            .Distinct()
            .Select(x => x.ToDto())
            .ToArray();

        return new SubmissionsQueueDto(group.Name, students, submissionDto);
    }
}