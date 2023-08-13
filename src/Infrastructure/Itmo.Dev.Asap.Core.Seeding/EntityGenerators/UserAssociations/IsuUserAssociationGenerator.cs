using Bogus;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Seeding.Options;

namespace Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

public class IsuUserAssociationGenerator : EntityGeneratorBase<IsuUserAssociationModel>
{
    private const int MinIsuNumber = 100000;
    private const int MaxIsuNumber = 1000000;

    private readonly Faker _faker;

    private readonly IEntityGenerator<UserModel> _userGenerator;

    public IsuUserAssociationGenerator(
        EntityGeneratorOptions<IsuUserAssociationModel> options,
        Faker faker,
        IEntityGenerator<UserModel> userGenerator)
        : base(options)
    {
        _faker = faker;
        _userGenerator = userGenerator;
    }

    protected override IsuUserAssociationModel Generate(int index)
    {
        if (index >= _userGenerator.GeneratedEntities.Count)
        {
            const string message = "Isu association count is greater than count of users.";
            throw new ArgumentOutOfRangeException(nameof(index), message);
        }

        UserModel user = _userGenerator.GeneratedEntities[index];

        foreach (UserAssociationModel userAssociation in user.Associations)
        {
            if (userAssociation is IsuUserAssociationModel isuUserAssociation)
                return isuUserAssociation;
        }

        int id = _faker.Random.Int(MinIsuNumber, MaxIsuNumber);

        return new IsuUserAssociationModel(_faker.Random.Guid(), user.Id, id);
    }
}