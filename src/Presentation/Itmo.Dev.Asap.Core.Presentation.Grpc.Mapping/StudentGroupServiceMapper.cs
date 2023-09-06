using Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Queries;
using Itmo.Dev.Asap.Core.StudentGroups;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class StudentGroupServiceMapper
{
    public static partial CreateStudentGroup.Command MapTo(this CreateRequest request);

    public static partial BulkGetStudentGroups.Query MapTo(this FindByIdsRequest request);

    public static partial UpdateStudentGroup.Command MapTo(this UpdateRequest request);

    public static partial GetStudentsByGroupId.Query MapTo(this GetStudentsRequest request);

    public static partial FindStudentGroupsByQuery.Query MapTo(this QueryStudentGroupRequest request);

    [MapProperty(nameof(CreateStudentGroup.Response.Group), nameof(CreateResponse.StudentGroup))]
    public static partial CreateResponse MapFrom(this CreateStudentGroup.Response response);

    public static partial FindByIdsResponse MapFrom(this BulkGetStudentGroups.Response response);

    public static partial UpdateResponse MapFrom(this UpdateStudentGroup.Response response);

    public static partial GetStudentsResponse MapFrom(this GetStudentsByGroupId.Response response);

    [MapProperty(nameof(FindStudentGroupsByQuery.Response.Groups), nameof(QueryStudentGroupResponse.StudentGroups))]
    public static partial QueryStudentGroupResponse MapFrom(this FindStudentGroupsByQuery.Response response);

    private static FindStudentGroupsByQuery.PageToken MapToPageToken(string value)
        => JsonConvert.DeserializeObject<FindStudentGroupsByQuery.PageToken>(value);
}