using Itmo.Dev.Asap.Core.DataAccess.Models.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itmo.Dev.Asap.Core.DataAccess.Configurations;

public class DeadlinePenaltyConfiguration : IEntityTypeConfiguration<DeadlinePenaltyModel>
{
    public void Configure(EntityTypeBuilder<DeadlinePenaltyModel> builder)
    {
        builder
            .HasOne(x => x.SubjectCourse)
            .WithMany(x => x.DeadlinePenalties)
            .HasForeignKey(x => x.SubjectCourseId)
            .HasPrincipalKey(x => x.Id);

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<AbsoluteDeadlinePenaltyModel>(nameof(AbsoluteDeadlinePenalty))
            .HasValue<FractionDeadlinePenaltyModel>(nameof(FractionDeadlinePenalty))
            .HasValue<CappingDeadlinePenaltyModel>(nameof(CappingDeadlinePenalty));
    }
}