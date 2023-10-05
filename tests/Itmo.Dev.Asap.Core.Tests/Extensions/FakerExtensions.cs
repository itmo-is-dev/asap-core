using Bogus;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.Tests.Extensions;

public static class FakerExtensions
{
    public static User User(this Faker faker)
    {
        return new User(faker.Random.Guid(), faker.Person.FirstName, faker.Internet.UserName(), faker.Person.LastName);
    }
}