using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record StudentQuery(
    Guid[] Ids,
    Guid[] GroupIds,
    Guid[] AssignmentIds,
    Guid[] SubjectCourseIds,
    string[] GroupNamePatterns,
    string[] FullNamePatterns,
    int[] UniversityIds,
    Guid? Cursor,
    int? Limit);