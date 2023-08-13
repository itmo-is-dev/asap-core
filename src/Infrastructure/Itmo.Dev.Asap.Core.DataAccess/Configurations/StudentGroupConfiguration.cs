using Itmo.Dev.Asap.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itmo.Dev.Asap.Core.DataAccess.Configurations;

public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroupModel>
{
    public void Configure(EntityTypeBuilder<StudentGroupModel> builder)
    {
        builder
            .HasMany(x => x.Students)
            .WithOne(x => x.StudentGroup)
            .HasForeignKey(x => x.StudentGroupId)
            .HasPrincipalKey(x => x.Id);
    }
}