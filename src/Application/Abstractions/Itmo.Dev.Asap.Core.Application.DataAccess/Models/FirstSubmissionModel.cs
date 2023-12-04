namespace Itmo.Dev.Asap.Core.Application.DataAccess.Models;

public record FirstSubmissionModel(
    Guid Id,
    Guid StudentId,
    Guid AssignmentId);