using Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries;
using Itmo.Dev.Asap.Core.StudentGroups;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class StudentGroupServiceMapper
{
    public static partial CreateStudyGroup.Command MapTo(this CreateRequest request);

    public static partial BulkGetStudyGroups.Query MapTo(this FindByIdsRequest request);

    public static partial UpdateStudyGroup.Command MapTo(this UpdateRequest request);

    public static partial GetStudentsByGroupId.Query MapTo(this GetStudentsRequest request);

    public static partial FindGroupsByQuery.Query MapTo(this QueryStudentGroupRequest request);

    [MapProperty(nameof(CreateStudyGroup.Response.Group), nameof(CreateResponse.StudentGroup))]
    public static partial CreateResponse MapFrom(this CreateStudyGroup.Response response);

    public static partial FindByIdsResponse MapFrom(this BulkGetStudyGroups.Response response);

    public static partial UpdateResponse MapFrom(this UpdateStudyGroup.Response response);

    public static partial GetStudentsResponse MapFrom(this GetStudentsByGroupId.Response response);

    public static partial QueryStudentGroupResponse MapFrom(this FindGroupsByQuery.Response response);

    private static FindGroupsByQuery.PageToken MapToPageToken(string value)
        => JsonConvert.DeserializeObject<FindGroupsByQuery.PageToken>(value);
}