using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.UserAssociations;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands.UpdateUserUniversityId;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Users;

internal class UpdateUserUniversityIdHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateUserUniversityIdHandler(
        IPersistenceContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (_currentUser.CanManageStudents is false)
            throw UserHasNotAccessException.AccessViolation(_currentUser.Id);

        User user = await _context.Users.GetByIdAsync(request.UserId, cancellationToken);
        IsuUserAssociation? association = user.FindAssociation<IsuUserAssociation>();

        if (association is null)
        {
            association = IsuUserAssociation.CreateAndAttach(Guid.NewGuid(), user, request.UniversityId);
            _context.UserAssociations.Add(association);
        }
        else
        {
            association.UniversityId = request.UniversityId;
            _context.UserAssociations.Update(association);
        }

        await _context.SaveChangesAsync(cancellationToken);

        UserDto dto = user.ToDto();

        return new Response(dto);
    }
}