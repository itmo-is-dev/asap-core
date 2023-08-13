using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class PointsMapping
{
    public static double AsDto(this Points points)
    {
        return points.Value;
    }

    public static double? AsDto(this Points? points)
    {
        return points?.Value;
    }

    public static Points AsPoints(this double points)
    {
        return new Points(points);
    }

    public static Points? AsPoints(this double? points)
    {
        return points == null ? null : new Points(points.Value);
    }
}