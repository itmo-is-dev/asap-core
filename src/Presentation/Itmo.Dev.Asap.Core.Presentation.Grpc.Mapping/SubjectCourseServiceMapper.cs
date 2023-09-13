using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.SubjectCourses;
using Riok.Mapperly.Abstractions;
using System.Text.Json;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
internal static partial class SubjectCourseServiceMapper
{
    public static partial GetSubjectCourseById.Query MapTo(this GetByIdRequest request);

    public static partial CreateSubjectCourse.Command MapTo(this CreateRequest request);

    public static partial UpdateSubjectCourse.Command MapTo(this UpdateRequest request);

    public static partial GetSubjectCourseStudents.Query MapTo(this GetStudentsRequest request);

    public static partial GetAssignmentsBySubjectCourse.Query MapTo(this GetAssignmentsRequest request);

    public static partial GetSubjectCourseGroupsBySubjectCourseId.Query MapTo(this GetGroupsRequest request);

    public static partial GetSubmissionsQueue.Query MapTo(this GetStudentGroupQueueRequest request);

    public static partial AddFractionDeadlinePolicy.Command MapTo(this AddDeadlineRequest request);

    public static partial UpdateMentors.Command MapTo(this UpdateMentorsRequest request);

    public static partial GetByIdResponse MapFrom(this GetSubjectCourseById.Response response);

    public static partial CreateResponse MapFrom(this CreateSubjectCourse.Response response);

    public static partial UpdateResponse MapFrom(this UpdateSubjectCourse.Response response);

    public static partial GetStudentsResponse MapFrom(this GetSubjectCourseStudents.Response response);

    public static partial GetAssignmentsResponse MapFrom(this GetAssignmentsBySubjectCourse.Response response);

    public static partial GetGroupsResponse MapFrom(this GetSubjectCourseGroupsBySubjectCourseId.Response response);

    [MapProperty(nameof(GetSubmissionsQueue.Response.SubmissionsQueue), nameof(GetStudentGroupQueueResponse.Queue))]
    public static partial GetStudentGroupQueueResponse MapFrom(this GetSubmissionsQueue.Response response);

    private static TimeSpan MapToTimeSpan(Duration duration)
        => duration.ToTimeSpan();

    private static Timestamp MapToTimestamp(DateTime dateTime)
        => Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));

    private static GetSubjectCourseStudents.PageToken? MapStringToGetStudentsPageToken(string? str)
        => str is null ? null : JsonSerializer.Deserialize<GetSubjectCourseStudents.PageToken>(str);

    private static string? MapGetStudentsPageTokenToString(GetSubjectCourseStudents.PageToken? token)
        => token is null ? null : JsonSerializer.Serialize(token);
}