using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record SubjectCourseQuery(
    Guid[] Ids,
    Guid[] SubjectIds,
    Guid[] AssignmentIds,
    Guid[] StudentGroupIds,
    Guid[] SubmissionIds);