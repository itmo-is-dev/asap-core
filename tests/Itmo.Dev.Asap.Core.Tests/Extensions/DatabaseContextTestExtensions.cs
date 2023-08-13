using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.SubmissionStateWorkflows;

namespace Itmo.Dev.Asap.Core.Tests.Extensions;

public static class DatabaseContextTestExtensions
{
    public static async Task<Submission> GetSubmissionAsync(
        this IPersistenceContext context,
        params SubmissionStateKind[] states)
    {
        var query = SubmissionQuery.Build(x => x
            .WithSubmissionStates(states)
            .WithSubjectCourseWorkflow(SubmissionStateWorkflowType.ReviewWithDefense)
            .WithLimit(1));

        return await context.Submissions.QueryAsync(query, default).FirstAsync();
    }
}