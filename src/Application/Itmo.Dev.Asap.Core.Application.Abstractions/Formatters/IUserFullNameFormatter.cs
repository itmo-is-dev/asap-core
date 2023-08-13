using Itmo.Dev.Asap.Core.Application.Dto.Users;

namespace Itmo.Dev.Asap.Core.Application.Abstractions.Formatters;

public interface IUserFullNameFormatter
{
    string GetFullName(UserDto user);
}