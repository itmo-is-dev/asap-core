using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.Permissions;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class PermissionServiceMapper
{
    public static partial HasAccessToSubjectCourse.Query MapTo(this HasAccessToSubjectCourseRequest request);

    public static partial HasAccessToSubjectCourseResponse MapFrom(this HasAccessToSubjectCourse.Response response);
}