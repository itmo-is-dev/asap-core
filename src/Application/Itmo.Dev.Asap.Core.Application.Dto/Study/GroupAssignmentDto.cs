namespace Itmo.Dev.Asap.Core.Application.Dto.Study;

public record GroupAssignmentDto(
    Guid GroupId,
    string GroupName,
    Guid AssignmentId,
    string AssignmentTitle,
    DateOnly Deadline);