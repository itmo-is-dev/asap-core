using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record struct SubmissionsQueueDto(
    string GroupName,
    IReadOnlyCollection<StudentDto> Students,
    IReadOnlyList<SubmissionDto> Submissions);