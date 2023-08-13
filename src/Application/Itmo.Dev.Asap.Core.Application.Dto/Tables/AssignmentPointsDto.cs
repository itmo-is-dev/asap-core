namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record AssignmentPointsDto(Guid AssignmentId, DateOnly Date, bool IsBanned, double Points);