using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.Property(l => l.Id).ValueGeneratedOnAdd();

            builder.Property(l => l.TitleEn)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(l => l.TitleAr)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(l => l.VideoUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(l => l.ContentText).IsRequired(false);

            builder.Property(l => l.DurationInMinutes).IsRequired(false);

            builder.Property(l => l.DisplayOrder).HasDefaultValue(0);

            builder.Property(l => l.IsFreePreview).HasDefaultValue(false);

            builder.HasIndex(l => l.CourseId);
        }
    }
}