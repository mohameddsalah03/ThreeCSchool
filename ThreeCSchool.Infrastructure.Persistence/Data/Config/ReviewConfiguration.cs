using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.Property(r => r.UserId).HasMaxLength(450).IsRequired();
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).IsRequired(false);
            builder.Property(r => r.IsApproved).HasDefaultValue(false);

            // Unique constraint: one review per user per course
            builder.HasIndex(r => new { r.UserId, r.CourseId }).IsUnique();

            builder.HasIndex(r => r.CourseId);
        }
    }
}