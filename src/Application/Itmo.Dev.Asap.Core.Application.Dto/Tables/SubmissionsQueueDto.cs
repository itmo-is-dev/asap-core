using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Users;

namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record struct SubmissionsQueueDto(
    string GroupName,
    IReadOnlyCollection<StudentDto> Students,
    IReadOnlyList<SubmissionDto> Submissions);