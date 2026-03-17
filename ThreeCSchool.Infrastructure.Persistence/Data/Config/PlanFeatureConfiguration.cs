using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class PlanFeatureConfiguration : IEntityTypeConfiguration<PlanFeature>
    {
        public void Configure(EntityTypeBuilder<PlanFeature> builder)
        {
            builder.ToTable("PlanFeatures");

            builder.Property(f => f.Id).ValueGeneratedOnAdd();
            builder.Property(f => f.DescriptionEn).HasMaxLength(200).IsRequired();
            builder.Property(f => f.DescriptionAr).HasMaxLength(200).IsRequired();
            builder.Property(f => f.IsIncluded).HasDefaultValue(true);
            builder.Property(f => f.DisplayOrder).HasDefaultValue(0);

            builder.HasIndex(f => f.PricingPlanId);
        }
    }
}