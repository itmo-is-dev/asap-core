using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Submissions;

public record SubmissionInfoDto(Guid Id, DateTimeOffset CreatedAt, SubmissionStateDto State);