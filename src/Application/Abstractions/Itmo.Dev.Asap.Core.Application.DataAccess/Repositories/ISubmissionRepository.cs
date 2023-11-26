using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface ISubmissionRepository
{
    IAsyncEnumerable<Submission> QueryAsync(SubmissionQuery query, CancellationToken cancellationToken);

    IAsyncEnumerable<FirstSubmissionModel> QueryFirstSubmissionsAsync(
        FirstSubmissionQuery query,
        CancellationToken cancellationToken);

    Task<int> CountAsync(SubmissionQuery query, CancellationToken cancellationToken);

    void Add(Submission submission);

    void Update(Submission submission);
}