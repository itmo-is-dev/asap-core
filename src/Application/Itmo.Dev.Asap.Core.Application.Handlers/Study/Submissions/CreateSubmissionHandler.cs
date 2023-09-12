using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands.CreateSubmission;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

#pragma warning disable CA1506
internal class CreateSubmissionHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public CreateSubmissionHandler(IPersistenceContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        (Guid issuerId, Guid studentId, Guid assignmentId, string payload) = request;

        var studentQuery = StudentQuery.Build(x => x.WithId(studentId).WithAssignmentId(assignmentId));

        Student? student = await _context.Students
            .QueryAsync(studentQuery, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (student is null)
            throw EntityNotFoundException.For<Student>(studentId);

        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(assignmentId, cancellationToken);

        if (student.UserId.Equals(issuerId) is false)
        {
            var mentorQuery = MentorQuery.Build(x => x
                .WithUserId(issuerId)
                .WithSubjectCourseId(subjectCourse.Id));

            Mentor? mentor = await _context.Mentors
                .QueryAsync(mentorQuery, cancellationToken)
                .FirstOrDefaultAsync(cancellationToken);

            if (mentor is null)
            {
                return new Response.Unauthorized();
            }
        }

        if (student.Group is null)
        {
            throw new EntityNotFoundException($"Could not find group for student {studentId}");
        }

        GroupAssignment groupAssignment = await _context.GroupAssignments
            .GetByIdAsync(student.Group.Id, assignmentId, cancellationToken);

        var submissionCountQuery = SubmissionQuery.Build(x => x
            .WithUserId(student.UserId)
            .WithAssignmentId(assignmentId));

        int count = await _context.Submissions.CountAsync(submissionCountQuery, cancellationToken);

        var submission = new Submission(
            Guid.NewGuid(),
            count + 1,
            student,
            Calendar.CurrentDateTime,
            payload,
            groupAssignment);

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync(cancellationToken);

        Assignment assignment = await _context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Assignment.Id, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);
        SubmissionDto dto = submission.ToDto(ratedSubmission.TotalPoints);

        var evt = new SubmissionUpdated.Notification(dto);
        await _publisher.Publish(evt, default);

        return new Response.Success(dto);
    }
}