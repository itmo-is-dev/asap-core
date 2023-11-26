using Itmo.Dev.Asap.Core.Domain.Models;
using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record FirstSubmissionQuery(
    Guid SubjectCourseId,
    SubmissionStateKind[] States,
    FirstSubmissionQuery.PageTokenModel? PageToken,
    int PageSize)
{
    public sealed record PageTokenModel(Guid UserId, Guid AssignmentId);
}