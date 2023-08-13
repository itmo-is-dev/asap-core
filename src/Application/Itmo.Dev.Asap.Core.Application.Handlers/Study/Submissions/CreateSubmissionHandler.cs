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
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands.CreateSubmission;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

#pragma warning disable CA1506
internal class CreateSubmissionHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public CreateSubmissionHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        (Guid issuerId, Guid studentId, Guid assignmentId, string payload) = request;

        var studentQuery = StudentQuery.Build(x => x.WithId(request.StudentId).WithAssignmentId(assignmentId));

        Student? student = await _context.Students
            .QueryAsync(studentQuery, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(assignmentId, cancellationToken);

        // If issuer is not a student, check if it is mentor and find student corresponding to the repository
        if (student is null)
        {
            Mentor? mentor = subjectCourse.Mentors.SingleOrDefault(x => x.UserId.Equals(issuerId));

            if (mentor is not null)
            {
                studentQuery = StudentQuery.Build(x => x.WithSubjectCourseId(subjectCourse.Id));

                student = await _context.Students
                    .QueryAsync(studentQuery, cancellationToken)
                    .SingleOrDefaultAsync(cancellationToken);

                if (student is null)
                    throw EntityNotFoundException.For<Student>(studentId);
            }
            else
            {
                throw EntityNotFoundException.UserNotFoundInSubjectCourse(studentId, subjectCourse.Title);
            }
        }

        if (student.Group is null)
        {
            throw new EntityNotFoundException($"Could not find group for student {studentId}");
        }

        GroupAssignment groupAssignment = await _context.GroupAssignments
            .GetByIdsAsync(student.Group.Id, assignmentId, cancellationToken);

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

        Points points = submission.CalculateEffectivePoints(assignment, subjectCourse.DeadlinePolicy).Points;

        SubmissionDto dto = submission.ToDto(points);

        return new Response(dto);
    }
}