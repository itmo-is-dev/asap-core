using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Domain.Queue.Models;

public record struct EvaluatedSubmission(Submission Submission, double Value);