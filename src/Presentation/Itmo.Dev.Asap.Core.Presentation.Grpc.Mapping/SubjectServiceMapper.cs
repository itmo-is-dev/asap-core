using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Queries;
using Itmo.Dev.Asap.Core.Subjects;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class SubjectServiceMapper
{
    public static partial CreateSubject.Command MapTo(this CreateSubjectRequest request);

    public static partial GetSubjects.Query MapTo(this GetAllRequest request);

    public static partial GetSubjectById.Query MapTo(this GetByIdRequest request);

    public static partial UpdateSubject.Command MapTo(this UpdateRequest request);

    public static partial GetSubjectCoursesBySubjectId.Query MapTo(this GetCoursesRequest request);

    public static partial CreateSubjectResponse MapFrom(this CreateSubject.Response response);

    public static partial GetAllResponse MapFrom(this GetSubjects.Response response);

    public static partial GetByIdResponse MapFrom(this GetSubjectById.Response response);

    public static partial UpdateResponse MapFrom(this UpdateSubject.Response response);

    public static partial GetCoursesResponse MapFrom(this GetSubjectCoursesBySubjectId.Response response);
}