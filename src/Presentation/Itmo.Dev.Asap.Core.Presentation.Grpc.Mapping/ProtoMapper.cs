using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
public static partial class ProtoMapper
{
    public static Guid ToGuid(this string value)
    {
        return Guid.Parse(value);
    }

    public static partial Student MapToProtoStudent(this StudentDto student);
}