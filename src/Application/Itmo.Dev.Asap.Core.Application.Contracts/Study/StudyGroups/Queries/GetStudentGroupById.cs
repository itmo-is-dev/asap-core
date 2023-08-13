using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries;

internal static class GetStudentGroupById
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(StudyGroupDto Group);
}