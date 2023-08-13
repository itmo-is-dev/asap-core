namespace Itmo.Dev.Asap.Core.Application.Dto.Users;

public record StudentDto(UserDto User, Guid? GroupId, string GroupName, int? UniversityId);