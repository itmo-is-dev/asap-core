using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.SubmissionStateWorkflows;
using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record SubmissionQuery(
    Guid[] Ids,
    int[] Codes,
    Guid[] UserIds,
    Guid[] SubjectCourseIds,
    Guid[] AssignmentIds,
    Guid[] StudentGroupIds,
    SubmissionStateKind[] SubmissionStates,
    SubmissionStateWorkflowType[] SubjectCourseWorkflows,
    OrderDirection? OrderByCode,
    int? Limit);