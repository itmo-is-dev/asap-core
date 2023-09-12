using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.SubmissionWorkflow;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class SubmissionWorkflowMapper
{
    public static partial UpdatedResponse MapFrom(this SubmissionUpdateResult result);

    private static Timestamp ToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
}