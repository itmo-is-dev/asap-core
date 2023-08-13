using Itmo.Dev.Asap.Core.Application.Abstractions.Formatters;
using Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Extensions;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Mapping;

namespace Itmo.Dev.Asap.Core.Application.SubjectCourses;

public class SubjectCourseService : ISubjectCourseService
{
    private readonly IPersistenceContext _context;
    private readonly IUserFullNameFormatter _userFullNameFormatter;

    public SubjectCourseService(
        IPersistenceContext context,
        IUserFullNameFormatter userFullNameFormatter)
    {
        _context = context;
        _userFullNameFormatter = userFullNameFormatter;
    }

    public async Task<SubjectCoursePointsDto> CalculatePointsAsync(
        Guid subjectCourseId,
        CancellationToken cancellationToken)
    {
        StudentAssignment[] studentAssignments = await _context.StudentAssignments
            .GetBySubjectCourseIdAsync(subjectCourseId, cancellationToken)
            .OrderBy(x => x.Assignment.Order)
            .ToArrayAsync(cancellationToken);

        StudentPointsDto[] studentPoints = studentAssignments
            .GroupBy(x => x.Student)
            .Select(MapToStudentPoints)
            .OrderBy(x => x.Student.GroupName)
            .ThenBy(x => _userFullNameFormatter.GetFullName(x.Student.User))
            .ToArray();

        AssignmentDto[] assignments = studentAssignments
            .Select(x => x.Assignment)
            .Distinct()
            .Select(x => x.ToDto())
            .ToArray();

        return new SubjectCoursePointsDto(assignments, studentPoints);
    }

    private StudentPointsDto MapToStudentPoints(IGrouping<Student, StudentAssignment> grouping)
    {
        StudentDto studentDto = grouping.Key.ToDto();

        AssignmentPointsDto[] pointsDto = grouping
            .Select(x => x.CalculatePoints())
            .WhereNotNull()
            .Select(x => new AssignmentPointsDto(x.Assignment.Id, x.SubmissionDate, x.IsBanned, x.Points.Value))
            .ToArray();

        return new StudentPointsDto(studentDto, pointsDto);
    }
}