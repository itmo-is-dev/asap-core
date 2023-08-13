using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class UserMapper
{
    public static User MapTo(UserModel model)
    {
        var associations = model.Associations
            .Select(UserAssociationsMapper.MapTo)
            .ToHashSet();

        return new User(model.Id, model.FirstName, model.MiddleName, model.LastName, associations);
    }

    public static UserModel MapFrom(User entity)
    {
        var associations = entity.Associations
            .Select(UserAssociationsMapper.MapFrom)
            .ToHashSet();

        return new UserModel(entity.Id, entity.FirstName, entity.MiddleName, entity.LastName, associations);
    }
}