using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class PricingPlanConfiguration : IEntityTypeConfiguration<PricingPlan>
    {
        public void Configure(EntityTypeBuilder<PricingPlan> builder)
        {
            builder.ToTable("PricingPlans");

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.NameEn)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.NameAr)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.SubtitleEn)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(p => p.SubtitleAr)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(p => p.OriginalPrice)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(p => p.LevelCompletionCount).IsRequired();

            builder.Property(p => p.IsFeatured).HasDefaultValue(false);
            builder.Property(p => p.IsActive).HasDefaultValue(true);
            builder.Property(p => p.DisplayOrder).HasDefaultValue(0);

            // PricingPlan → PlanFeatures (cascade)
            builder.HasMany(p => p.Features)
                .WithOne(f => f.PricingPlan)
                .HasForeignKey(f => f.PricingPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // PricingPlan → CartItems (restrict)
            builder.HasMany(p => p.CartItems)
                .WithOne(ci => ci.PricingPlan)
                .HasForeignKey(ci => ci.PricingPlanId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}