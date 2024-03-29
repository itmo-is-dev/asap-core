using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Core.Submissions;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class SubmissionServiceMapper
{
    public static partial ActivateSubmission.Command MapTo(this ActivateRequest request);

    public static partial BanSubmission.Command MapTo(this BanRequest request);

    public static partial UnbanSubmission.Command MapTo(this UnbanRequest request);

    public static partial CreateSubmission.Command MapTo(this CreateRequest request);

    public static partial DeactivateSubmission.Command MapTo(this DeactivateRequest request);

    public static partial DeleteSubmission.Command MapTo(this DeleteRequest request);

    public static partial MarkSubmissionReviewed.Command MapTo(this MarkReviewedRequest request);

    public static partial RateSubmission.Command MapTo(this RateRequest request);

    public static partial ActivateResponse MapFrom(this ActivateSubmission.Response response);

    public static partial BanResponse MapFrom(this BanSubmission.Response response);

    public static partial DeactivateResponse MapFrom(this DeactivateSubmission.Response response);

    public static partial DeleteResponse MapFrom(this DeleteSubmission.Response response);

    public static partial MarkReviewedResponse MapFrom(this MarkSubmissionReviewed.Response response);

    [MapProperty(nameof(SubmissionRateDto.Id), nameof(SubmissionRate.SubmissionId))]
    public static partial SubmissionRate MapFrom(this SubmissionRateDto rateDto);

    public static SubmissionInfo MapToProtoModel(this SubmissionInfoDto submission)
    {
        return new SubmissionInfo
        {
            SubmissionId = submission.Id.ToString(),
            CreatedAt = Timestamp.FromDateTimeOffset(submission.CreatedAt),
            State = submission.State.MapToProtoModel(),
        };
    }

    private static Timestamp ToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));

    private static SubmissionState MapToProtoModel(this SubmissionStateDto state)
    {
        return state switch
        {
            SubmissionStateDto.Active => SubmissionState.Active,
            SubmissionStateDto.Inactive => SubmissionState.Inactive,
            SubmissionStateDto.Deleted => SubmissionState.Deleted,
            SubmissionStateDto.Completed => SubmissionState.Completed,
            SubmissionStateDto.Reviewed => SubmissionState.Reviewed,
            SubmissionStateDto.Banned => SubmissionState.Banned,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }
}