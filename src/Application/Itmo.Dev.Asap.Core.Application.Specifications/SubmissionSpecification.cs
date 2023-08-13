using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class SubmissionSpecification
{
    public static async Task<Submission> GetSubmissionForCodeOrLatestAsync(
        this ISubmissionRepository repository,
        Guid userId,
        Guid assignmentId,
        int? code,
        CancellationToken cancellationToken)
    {
        SubmissionQuery.Builder builder = new SubmissionQuery.Builder()
            .WithUserId(userId)
            .WithAssignmentId(assignmentId);

        builder = code is null
            ? builder.WithOrderByCode(OrderDirection.Descending).WithLimit(1)
            : builder.WithCode(code.Value);

        Submission? submission = await repository
            .QueryAsync(builder.Build(), cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return submission ?? throw new EntityNotFoundException("Could not find submission");
    }

    public static async Task<Submission> GetByIdAsync(
        this ISubmissionRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = SubmissionQuery.Build(x => x.WithId(id));

        Submission? submission = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        return submission ?? throw EntityNotFoundException.For<Submission>(id);
    }
}