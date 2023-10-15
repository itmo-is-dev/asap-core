using Itmo.Dev.Asap.Core.Application.Dto.Users;

namespace Itmo.Dev.Asap.Core.Application.Dto.Students;

public record StudentDto(UserDto User, Guid? GroupId, string GroupName);