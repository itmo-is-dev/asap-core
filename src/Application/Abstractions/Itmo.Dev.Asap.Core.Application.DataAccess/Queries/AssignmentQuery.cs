using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record AssignmentQuery(
    IReadOnlyCollection<Guid> Ids,
    IReadOnlyCollection<Guid> SubjectCourseIds,
    OrderDirection? OrderByOrder);