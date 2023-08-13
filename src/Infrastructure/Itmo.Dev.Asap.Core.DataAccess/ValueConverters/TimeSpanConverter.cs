using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Itmo.Dev.Asap.Core.DataAccess.ValueConverters;

public class TimeSpanConverter : ValueConverter<TimeSpan, long>
{
    public TimeSpanConverter()
        : base(x => x.Ticks, x => TimeSpan.FromTicks(x)) { }
}