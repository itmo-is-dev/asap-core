namespace Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;

public partial class IsuUserAssociationModel : UserAssociationModel
{
    public IsuUserAssociationModel(Guid id, Guid userId, int universityId) : base(id, userId)
    {
        UniversityId = universityId;
    }

    public int UniversityId { get; set; }
}