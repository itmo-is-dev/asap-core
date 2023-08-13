using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class IsuUserAssociationSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<IsuUserAssociationModel> _generator;

    public IsuUserAssociationSeeder(IEntityGenerator<IsuUserAssociationModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.UserAssociations.AddRange(_generator.GeneratedEntities);
    }
}