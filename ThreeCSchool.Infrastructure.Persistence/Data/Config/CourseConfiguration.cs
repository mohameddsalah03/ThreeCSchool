using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.TitleEn)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(c => c.TitleAr)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(c => c.Slug)
                .HasMaxLength(400)
                .IsRequired();

            builder.HasIndex(c => c.Slug).IsUnique();

            builder.Property(c => c.DescriptionEn).IsRequired(false);
            builder.Property(c => c.DescriptionAr).IsRequired(false);

            builder.Property(c => c.ThumbnailUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.Type).IsRequired();

            builder.Property(c => c.Price)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.DiscountPrice)
                .HasColumnType("decimal(10,2)")
                .IsRequired(false);

            builder.Property(c => c.AverageRating)
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.IsDiscount).HasDefaultValue(false);
            builder.Property(c => c.IsFree).HasDefaultValue(false);
            builder.Property(c => c.IsDownloadable).HasDefaultValue(false);
            builder.Property(c => c.IsUpcoming).HasDefaultValue(false);
            builder.Property(c => c.IsPublished).HasDefaultValue(false);
            builder.Property(c => c.TotalEnrollments).HasDefaultValue(0);

            builder.Property(c => c.InstructorId)
                .HasMaxLength(450)
                .IsRequired();

            builder.HasIndex(c => c.CategoryId);
            builder.HasIndex(c => c.InstructorId);

            // Course → Lessons (cascade)
            builder.HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course → Sessions (cascade)
            builder.HasMany(c => c.Sessions)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course → Reviews (cascade)
            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course → CartItems (restrict — لو الكورس اتحذف مش هنحذف الـ cart)
            builder.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Course)
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}