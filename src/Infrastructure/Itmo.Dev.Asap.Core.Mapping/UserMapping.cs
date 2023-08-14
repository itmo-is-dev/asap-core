using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Domain.UserAssociations;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        IsuUserAssociation? isuAssociation = user.FindAssociation<IsuUserAssociation>();
        return new UserDto(user.Id, user.FirstName, user.MiddleName, user.LastName, isuAssociation?.UniversityId);
    }
}