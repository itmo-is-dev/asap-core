using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.UserAssociations;

public abstract partial class UserAssociation : IEntity<Guid>
{
    protected UserAssociation(Guid id, Guid userId) : this(id)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}