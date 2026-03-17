using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.NameEn)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.NameAr)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.Slug)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(c => c.Slug).IsUnique();

            builder.Property(c => c.IconUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.DisplayOrder).HasDefaultValue(0);
            builder.Property(c => c.IsActive).HasDefaultValue(true);

            builder.Property(c => c.ParentCategoryId).IsRequired(false);

            // Self-referencing relationship
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Category → Courses
            builder.HasMany(c => c.Courses)
                .WithOne(co => co.Category)
                .HasForeignKey(co => co.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}