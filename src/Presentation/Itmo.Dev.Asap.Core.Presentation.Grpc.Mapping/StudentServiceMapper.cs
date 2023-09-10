using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;
using Itmo.Dev.Asap.Core.Students;
using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class StudentServiceMapper
{
    public static partial CreateStudents.Command MapTo(this CreateStudentsRequest request);

    public static partial DismissStudentFromGroup.Command MapTo(this DismissFromGroupRequest request);

    public static partial TransferStudent.Command MapTo(this TransferStudentRequest request);

    public static partial FindStudentsByQuery.Query MapTo(this QueryStudentRequest request);

    public static partial CreateStudentsResponse MapFrom(this CreateStudents.Response.Success response);

    public static partial DismissFromGroupResponse MapFrom(this DismissStudentFromGroup.Response response);

    public static partial TransferStudentResponse MapFrom(this TransferStudent.Response response);

    public static partial QueryStudentResponse MapFrom(this FindStudentsByQuery.Response response);

    private static FindStudentsByQuery.PageToken MapToPageToken(string value)
        => JsonConvert.DeserializeObject<FindStudentsByQuery.PageToken>(value);
}