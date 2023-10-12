using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
internal static partial class SubmissionWorkflowMapper
{
    [MapProperty(nameof(SubmissionRateDto.Id), nameof(SubmissionRate.SubmissionId))]
    public static partial SubmissionRate MapToSubmissionRate(this SubmissionRateDto rate);

    public static partial SubmissionState MapToSubmissionState(this SubmissionStateDto state);

    private static Timestamp ToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
}