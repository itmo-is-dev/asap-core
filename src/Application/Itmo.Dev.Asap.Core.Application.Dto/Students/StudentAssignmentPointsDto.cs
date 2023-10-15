namespace Itmo.Dev.Asap.Core.Application.Dto.Students;

public record StudentAssignmentPointsDto(
    Guid StudentId,
    Guid AssignmentId,
    Guid SubjectCourseId,
    DateTimeOffset Date,
    bool IsBanned,
    double? Points);