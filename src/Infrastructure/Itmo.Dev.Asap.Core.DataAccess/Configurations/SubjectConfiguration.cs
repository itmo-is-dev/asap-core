using Itmo.Dev.Asap.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itmo.Dev.Asap.Core.DataAccess.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<SubjectModel>
{
    public void Configure(EntityTypeBuilder<SubjectModel> builder)
    {
        builder
            .HasMany(x => x.SubjectCourses)
            .WithOne(x => x.Subject)
            .HasForeignKey(x => x.SubjectId)
            .HasPrincipalKey(x => x.Id);
    }
}