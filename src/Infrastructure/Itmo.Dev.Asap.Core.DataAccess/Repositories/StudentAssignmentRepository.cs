using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

#pragma warning disable CA1506
public class StudentAssignmentRepository : IStudentAssignmentRepository
{
    private readonly DatabaseContext _context;
    private readonly ISubjectCourseRepository _subjectCourseRepository;

    public StudentAssignmentRepository(DatabaseContext context, ISubjectCourseRepository subjectCourseRepository)
    {
        _context = context;
        _subjectCourseRepository = subjectCourseRepository;
    }

    public async Task<StudentAssignment> GetByIdAsync(
        Guid studentId,
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _subjectCourseRepository
            .QueryAsync(SubjectCourseQuery.Build(x => x.WithAssignmentId(assignmentId)), cancellationToken)
            .SingleAsync(cancellationToken);

        StudentModel studentModel = await _context.Students
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.UserId.Equals(studentId))
            .SingleAsync(cancellationToken);

        AssignmentModel assignmentModel = await _context.Assignments
            .Include(x => x.GroupAssignments)
            .ThenInclude(x => x.StudentGroup)
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.Id.Equals(assignmentId))
            .SingleAsync(cancellationToken);

        List<SubmissionModel> submissionModels = await _context.Submissions
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.StudentId.Equals(studentId))
            .Where(x => x.AssignmentId.Equals(assignmentId))
            .ToListAsync(cancellationToken);

        GroupAssignment[] groupAssignments = assignmentModel.GroupAssignments
            .Select(ga => GroupAssignmentMapper.MapTo(
                ga,
                ga.StudentGroup.Name,
                ga.Assignment.Title,
                ga.Assignment.ShortName))
            .ToArray();

        Submission[] submissions = submissionModels
            .Join(
                groupAssignments,
                x => x.StudentGroupId,
                x => x.Id.StudentGroupId,
                SubmissionMapper.MapTo)
            .ToArray();

        return new StudentAssignment(
            StudentMapper.MapTo(studentModel),
            AssignmentMapper.MapTo(assignmentModel),
            groupAssignments,
            submissions,
            subjectCourse);
    }

    public async IAsyncEnumerable<StudentAssignment> GetBySubjectCourseIdAsync(
        Guid subjectCourseId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _subjectCourseRepository
            .QueryAsync(SubjectCourseQuery.Build(x => x.WithId(subjectCourseId)), cancellationToken)
            .SingleAsync(cancellationToken);

        IQueryable<AssignmentModel> assignmentQuery = _context.Assignments;

        assignmentQuery = assignmentQuery
            .Include(x => x.GroupAssignments)
            .ThenInclude(x => x.StudentGroup)
            .ThenInclude(x => x.Students)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.Associations);

        assignmentQuery = assignmentQuery
            .Include(x => x.GroupAssignments)
            .ThenInclude(x => x.Submissions)
            .ThenInclude(x => x.Student)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.Associations);

        assignmentQuery = assignmentQuery
            .Include(x => x.GroupAssignments)
            .ThenInclude(x => x.Submissions)
            .ThenInclude(x => x.GroupAssignment)
            .ThenInclude(x => x.Assignment);

        assignmentQuery = assignmentQuery
            .Include(x => x.GroupAssignments)
            .ThenInclude(x => x.StudentGroup);

        assignmentQuery = assignmentQuery
            .Include(x => x.GroupAssignments)
            .ThenInclude(x => x.Assignment);

        List<AssignmentModel> assignmentModels = await assignmentQuery
            .AsSplitQuery()
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.SubjectCourse.Id.Equals(subjectCourseId))
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        IEnumerable<Student> students = assignmentModels
            .SelectMany(x => x.GroupAssignments)
            .SelectMany(x => x.StudentGroup.Students)
            .Distinct()
            .Select(StudentMapper.MapTo);

        IEnumerable<Assignment> assignments = assignmentModels.Select(AssignmentMapper.MapTo);

        GroupAssignment[] groupAssignments = assignmentModels
            .SelectMany(x => x.GroupAssignments)
            .Select(x => GroupAssignmentMapper
                .MapTo(x, x.StudentGroup.Name, x.Assignment.Title, x.Assignment.ShortName))
            .ToArray();

        Submission[] submissions = assignmentModels
            .SelectMany(x => x.GroupAssignments)
            .SelectMany(x => x.Submissions, (ga, s) => (ga, s))
            .Select(x =>
            {
                GroupAssignment groupAssignment = GroupAssignmentMapper.MapTo(
                    x.ga,
                    x.ga.StudentGroup.Name,
                    x.ga.Assignment.Title,
                    x.ga.Assignment.ShortName);

                return SubmissionMapper.MapTo(x.s, groupAssignment);
            })
            .ToArray();

        IEnumerable<(Student Student, Assignment Assignment)> enumerable = students
            .SelectMany(_ => assignments, (student, assignment) => (student, assignment));

        foreach ((Student student, Assignment assignment) in enumerable)
        {
            GroupAssignment[] ga = groupAssignments
                .Where(x => x.Id.AssignmentId.Equals(assignment.Id))
                .ToArray();

            Submission[] s = submissions
                .Where(x => x.Student.Equals(student))
                .Where(x => x.GroupAssignment.Id.AssignmentId.Equals(assignment.Id))
                .ToArray();

            yield return new StudentAssignment(student, assignment, ga, s, subjectCourse);
        }
    }
}