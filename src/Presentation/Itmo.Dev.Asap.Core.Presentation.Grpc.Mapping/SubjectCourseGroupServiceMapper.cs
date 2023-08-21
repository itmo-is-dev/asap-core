using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;
using Itmo.Dev.Asap.Core.SubjectCourseGroups;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class SubjectCourseGroupServiceMapper
{
    public static partial BulkCreateSubjectCourseGroups.Command MapTo(this CreateRequest request);

    public static partial DeleteSubjectCourseGroup.Command MapTo(this DeleteRequest request);

    [MapProperty(nameof(BulkCreateSubjectCourseGroups.Response.Groups), nameof(CreateResponse.SubjectCourseGroups))]
    public static partial CreateResponse MapFrom(this BulkCreateSubjectCourseGroups.Response response);
}