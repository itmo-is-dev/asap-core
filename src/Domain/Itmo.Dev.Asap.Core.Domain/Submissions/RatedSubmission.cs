using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.Domain.Submissions;

public record struct RatedSubmission(Submission Submission, Points Points);