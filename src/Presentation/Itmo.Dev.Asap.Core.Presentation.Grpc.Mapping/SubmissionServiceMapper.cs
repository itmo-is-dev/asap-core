using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;
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

    public static partial CreateSubmission.Command MapTo(this CreateRequest request);

    public static partial DeactivateSubmission.Command MapTo(this DeactivateRequest request);

    public static partial DeleteSubmission.Command MapTo(this DeleteRequest request);

    public static partial MarkSubmissionReviewed.Command MapTo(this MarkReviewedRequest request);

    public static partial RateSubmission.Command MapTo(this RateRequest request);

    public static partial ActivateResponse MapFrom(this ActivateSubmission.Response response);

    public static partial BanResponse MapFrom(this BanSubmission.Response response);

    public static partial CreateResponse MapFrom(this CreateSubmission.Response response);

    public static partial DeactivateResponse MapFrom(this DeactivateSubmission.Response response);

    public static partial DeleteResponse MapFrom(this DeleteSubmission.Response response);

    public static partial MarkReviewedResponse MapFrom(this MarkSubmissionReviewed.Response response);

    public static partial SubmissionRate MapFrom(this SubmissionRateDto rateDto);

    private static Timestamp ToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
}