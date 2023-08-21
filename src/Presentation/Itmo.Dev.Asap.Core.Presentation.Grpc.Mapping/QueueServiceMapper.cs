using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Queue;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class QueueServiceMapper
{
    public static partial QueueUpdatedResponse.Types.Submission MapToProtoQueueSubmission(this SubmissionDto submission);
}