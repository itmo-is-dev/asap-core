namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record struct SubmissionsQueueDto(string GroupName, IReadOnlyList<QueueSubmissionDto> Submissions);