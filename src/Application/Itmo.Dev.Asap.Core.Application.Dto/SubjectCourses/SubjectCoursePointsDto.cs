using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;

namespace Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;

public record struct SubjectCoursePointsDto(
    IReadOnlyCollection<AssignmentDto> Assignments,
    IReadOnlyList<StudentPointsDto> StudentsPoints);