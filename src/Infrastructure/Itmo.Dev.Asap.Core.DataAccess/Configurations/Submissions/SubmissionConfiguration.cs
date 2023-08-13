using Itmo.Dev.Asap.Core.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itmo.Dev.Asap.Core.DataAccess.Configurations.Submissions;

public class SubmissionConfiguration : IEntityTypeConfiguration<SubmissionModel>
{
    public void Configure(EntityTypeBuilder<SubmissionModel> builder)
    {
        builder
            .HasOne(x => x.Student)
            .WithMany(x => x.Submissions)
            .HasForeignKey(x => x.StudentId)
            .HasPrincipalKey(x => x.UserId);

        builder
            .HasOne(x => x.GroupAssignment)
            .WithMany(x => x.Submissions)
            .HasForeignKey(x => new { x.StudentGroupId, x.AssignmentId })
            .HasPrincipalKey(x => new { x.StudentGroupId, x.AssignmentId });
    }
}