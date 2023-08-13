using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itmo.Dev.Asap.Core.DataAccess.Configurations.Users;

public class StudentConfiguration : IEntityTypeConfiguration<StudentModel>
{
    public void Configure(EntityTypeBuilder<StudentModel> builder)
    {
        builder.HasKey(x => x.UserId);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id);

        builder
            .HasOne(x => x.StudentGroup)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.StudentGroupId)
            .HasPrincipalKey(x => x.Id);
    }
}