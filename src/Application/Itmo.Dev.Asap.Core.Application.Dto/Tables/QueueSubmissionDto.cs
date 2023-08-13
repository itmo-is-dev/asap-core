using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Users;

namespace Itmo.Dev.Asap.Core.Application.Dto.Tables;

public record QueueSubmissionDto(StudentDto Student, SubmissionDto Submission);