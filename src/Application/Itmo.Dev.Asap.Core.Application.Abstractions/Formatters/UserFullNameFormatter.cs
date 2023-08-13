using Itmo.Dev.Asap.Core.Application.Dto.Users;
using System.Text;

namespace Itmo.Dev.Asap.Core.Application.Abstractions.Formatters;

public class UserFullNameFormatter : IUserFullNameFormatter
{
    public string GetFullName(UserDto user)
    {
        StringBuilder fullNameBuilder = new StringBuilder()
            .Append(user.LastName)
            .Append(' ')
            .Append(user.FirstName);

        if (!string.IsNullOrEmpty(user.MiddleName))
        {
            fullNameBuilder
                .Append(' ')
                .Append(user.MiddleName);
        }

        return fullNameBuilder.ToString();
    }
}