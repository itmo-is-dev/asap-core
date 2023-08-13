using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Study;

public record struct StudentAssignmentPoints(
    Student Student,
    Assignment Assignment,
    bool IsBanned,
    Points Points,
    Points Penalty,
    DateOnly SubmissionDate);