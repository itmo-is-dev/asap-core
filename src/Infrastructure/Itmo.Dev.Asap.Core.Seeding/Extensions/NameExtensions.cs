using Bogus.DataSets;

namespace Itmo.Dev.Asap.Core.Seeding.Extensions;

internal static class NameExtensions
{
    private const float ChanceOfHavingMiddleName = 0.95f;

    public static string MiddleName(this Name name)
    {
        return name.Random.Bool(ChanceOfHavingMiddleName)
            ? name.FirstName()
            : string.Empty;
    }
}