using Itmo.Dev.Asap.Core.Domain.Tools;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Itmo.Dev.Asap.Core.DataAccess.ValueConverters;

public class SpbDateTimeValueConverter : ValueConverter<SpbDateTime, DateTime>
{
    public SpbDateTimeValueConverter()
        : base(
            x => Calendar.ToUtc(x),
            x => Calendar.FromUtc(x)) { }
}