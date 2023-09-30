using Itmo.Dev.Asap.Core.Application.Dto.Users;

namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record StudentPointsDto(
    StudentDto Student,
    int SubjectCourseOrdinal,
    IReadOnlyCollection<AssignmentPointsDto> Points);