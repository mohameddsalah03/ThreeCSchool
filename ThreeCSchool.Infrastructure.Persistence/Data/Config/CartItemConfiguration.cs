using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.Property(ci => ci.Id).ValueGeneratedOnAdd();

            builder.Property(ci => ci.PriceSnapshot)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(ci => ci.CourseId).IsRequired(false);
            builder.Property(ci => ci.PricingPlanId).IsRequired(false);

            builder.HasIndex(ci => ci.CartId);
        }
    }
}