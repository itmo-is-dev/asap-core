using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public static partial class ProtoMapper
{
    public static Guid ToGuid(this string value)
    {
        return Guid.Parse(value);
    }

    public static partial Student MapToProtoStudent(this StudentDto student);

    public static partial Submission MapToProtoSubmission(this SubmissionDto submission);

    private static Timestamp MapToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
}