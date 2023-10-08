using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record StudentGroupQuery(
    Guid[] Ids,
    Guid[] ExcludedIds,
    Guid[] StudentIds,
    string[] NamePatterns,
    Guid[] SubjectCourseIds,
    Guid[] ExcludedSubjectCourseIds,
    Guid? Cursor,
    int? Limit);