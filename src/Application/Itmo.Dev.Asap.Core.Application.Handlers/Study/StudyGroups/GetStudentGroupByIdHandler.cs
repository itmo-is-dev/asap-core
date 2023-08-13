using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries.GetStudentGroupById;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;

internal class GetStudentGroupByIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetStudentGroupByIdHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        StudentGroup group = await _context.StudentGroups.GetByIdAsync(request.Id, cancellationToken);
        StudyGroupDto dto = group.ToDto();

        return new Response(dto);
    }
}