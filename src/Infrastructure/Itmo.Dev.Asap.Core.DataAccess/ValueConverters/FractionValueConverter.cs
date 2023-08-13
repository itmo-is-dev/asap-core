using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Itmo.Dev.Asap.Core.DataAccess.ValueConverters;

public class FractionValueConverter : ValueConverter<Fraction, double>
{
    public FractionValueConverter()
        : base(x => x.Value, x => new Fraction(x)) { }
}