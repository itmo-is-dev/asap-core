using Bogus;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Seeding.Extensions;
using Itmo.Dev.Asap.Core.Seeding.Options;

namespace Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

public class UserGenerator : EntityGeneratorBase<UserModel>
{
    private readonly Faker _faker;

    public UserGenerator(EntityGeneratorOptions<UserModel> options, Faker faker)
        : base(options)
    {
        _faker = faker;
    }

    protected override UserModel Generate(int index)
    {
        return new UserModel(
            _faker.Random.Guid(),
            _faker.Name.FirstName(),
            _faker.Name.MiddleName(),
            _faker.Name.LastName(),
            new List<UserAssociationModel>());
    }
}