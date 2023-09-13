using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;

namespace Itmo.Dev.Asap.Core.Application.Specifications;

public static class GroupAssignmentSpecifications
{
    public static Task<GroupAssignment> GetByIdAsync(
        this IGroupAssignmentRepository repository,
        Guid groupId,
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        var id = new GroupAssignmentId(groupId, assignmentId);
        return repository.GetByIdAsync(id, cancellationToken);
    }

    public static async Task<GroupAssignment> GetByIdAsync(
        this IGroupAssignmentRepository repository,
        GroupAssignmentId id,
        CancellationToken cancellationToken)
    {
        var query = GroupAssignmentQuery.Build(x => x
            .WithGroupId(id.StudentGroupId)
            .WithAssignmentId(id.AssignmentId));

        GroupAssignment? groupAssignment = await repository
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (groupAssignment is not null)
            return groupAssignment;

        string message = $"Could not find GroupAssignment, GroupId = {id.StudentGroupId}, AssignmentId = {id.AssignmentId}";
        throw new EntityNotFoundException(message);
    }
}