using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.Domain.UserAssociations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class UserAssociationRepository :
    RepositoryBase<UserAssociation, UserAssociationModel>,
    IUserAssociationRepository
{
    private readonly DatabaseContext _context;
    private readonly ILogger<UserAssociationRepository> _logger;

    public UserAssociationRepository(DatabaseContext context, ILogger<UserAssociationRepository> logger) : base(context)
    {
        _context = context;
        _logger = logger;
    }

    protected override DbSet<UserAssociationModel> DbSet => _context.UserAssociations;

    protected override UserAssociationModel MapFrom(UserAssociation entity)
    {
        return UserAssociationsMapper.MapFrom(entity);
    }

    protected override bool Equal(UserAssociation entity, UserAssociationModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(UserAssociationModel model, UserAssociation entity)
    {
        if ((entity, model) is (IsuUserAssociation association, IsuUserAssociationModel associationModel))
        {
            associationModel.UniversityId = association.UniversityId;
        }
        else
        {
            _logger.LogWarning(
                "Attempt to update incompatible user association types: Model = {ModelType}, Entity = {EntityType}",
                model.GetType(),
                entity.GetType());
        }
    }
}