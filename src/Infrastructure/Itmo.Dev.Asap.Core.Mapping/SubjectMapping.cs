using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class SubjectMapping
{
    public static SubjectDto ToDto(this Subject subject)
    {
        return new SubjectDto(subject.Id, subject.Title);
    }
}