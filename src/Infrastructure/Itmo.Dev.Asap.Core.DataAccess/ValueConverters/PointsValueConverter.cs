using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Itmo.Dev.Asap.Core.DataAccess.ValueConverters;

public class PointsValueConverter : ValueConverter<Points, double>
{
    public PointsValueConverter()
        : base(x => x.Value, x => new Points(x)) { }
}