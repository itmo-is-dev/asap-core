using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Students;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class StudentMapping
{
    public static StudentDto ToDto(this Student student)
    {
        return new StudentDto(
            student.User.ToDto(),
            student.Group?.Id,
            student.Group?.Name ?? string.Empty);
    }

    public static StudentAssignmentPointsDto MapToStudentAssignmentPoints(
        this SubmissionDto submission,
        Guid subjectCourseId)
    {
        return new StudentAssignmentPointsDto(
            submission.StudentId,
            submission.AssignmentId,
            subjectCourseId,
            submission.SubmissionDate,
            submission.State is SubmissionStateDto.Banned,
            submission.Points);
    }
}