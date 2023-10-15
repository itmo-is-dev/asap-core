using Itmo.Dev.Asap.Core.Application.Dto.Students;

namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record StudentPointsDto(StudentDto Student, IReadOnlyCollection<AssignmentPointsDto> Points);