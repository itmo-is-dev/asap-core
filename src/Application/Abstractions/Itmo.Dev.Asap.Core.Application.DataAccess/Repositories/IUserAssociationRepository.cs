using Itmo.Dev.Asap.Core.Domain.UserAssociations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IUserAssociationRepository
{
    void Add(UserAssociation entity);

    void Update(UserAssociation entity);

    void AddRange(IEnumerable<UserAssociation> entities);
}