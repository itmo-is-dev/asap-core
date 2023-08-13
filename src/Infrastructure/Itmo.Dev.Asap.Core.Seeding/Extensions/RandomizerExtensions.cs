using Bogus;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Seeding.Extensions;

public static class RandomizerExtensions
{
    public static Points Points(this Randomizer randomizer, double min = 0.0d, double max = 1.0d)
    {
        return new Points(randomizer.Double(min, max));
    }

    public static Fraction Fraction(this Randomizer randomizer)
    {
        return new Fraction(randomizer.Double());
    }
}