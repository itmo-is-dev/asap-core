using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record GroupAssignmentQuery(
    IReadOnlyCollection<Guid> GroupIds,
    IReadOnlyCollection<Guid> AssignmentIds,
    IReadOnlyCollection<GroupAssignmentId> GroupAssignmentIds,
    IReadOnlyCollection<Guid> SubjectCourseIds);