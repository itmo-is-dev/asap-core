namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

public static class ProtoMapper
{
    public static Guid ToGuid(this string value)
        => Guid.Parse(value);
}