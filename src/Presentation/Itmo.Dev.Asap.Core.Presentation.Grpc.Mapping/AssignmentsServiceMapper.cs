using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Queries;
using Itmo.Dev.Asap.Core.Assignments;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class AssignmentsServiceMapper
{
    public static DateOnly MapToDateOnly(this Timestamp timestamp)
        => DateOnly.FromDateTime(timestamp.ToDateTime());

    public static Timestamp MapToTimestamp(this DateOnly dateOnly)
    {
        var dateTime = dateOnly.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.Zero));
        return Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
    }

    public static partial CreateAssignment.Command MapTo(this CreateRequest request);

    public static partial GetAssignmentById.Query MapTo(this GetByIdRequest request);

    public static partial UpdateAssignmentPoints.Command MapTo(this UpdatePointsRequest request);

    public static partial GetGroupAssignments.Query MapTo(this GetGroupAssignmentsRequest request);

    public static partial UpdateGroupAssignmentDeadline.Command MapTo(
        this UpdateGroupAssignmentRequest request);

    public static partial UpdateGroupAssignmentDeadlines.Command MapTo(
        this UpdateGroupAssignmentDeadlinesRequest request);

    public static partial CreateResponse MapFrom(this CreateAssignment.Response response);

    public static partial GetByIdResponse MapFrom(this GetAssignmentById.Response response);

    public static partial UpdatePointsResponse MapFrom(this UpdateAssignmentPoints.Response.Success response);

    public static partial GetGroupAssignmentsResponse MapFrom(this GetGroupAssignments.Response response);

    public static partial UpdateGroupAssignmentResponse MapFrom(
        this UpdateGroupAssignmentDeadline.Response response);

    public static partial UpdateGroupAssignmentDeadlinesResponse MapFrom(
        this UpdateGroupAssignmentDeadlines.Response response);
}