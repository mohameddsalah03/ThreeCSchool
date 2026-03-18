using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
    {
        public void Configure(EntityTypeBuilder<Testimonial> builder)
        {
            builder.ToTable("Testimonials");

            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.PersonName)
                .HasMaxLength(150)
                .IsRequired();

            // "Student" or "Parent"
            builder.Property(t => t.PersonType)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.YoutubeVideoId)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.TitleEn)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(t => t.TitleAr)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(t => t.IsActive).HasDefaultValue(true);
            builder.Property(t => t.DisplayOrder).HasDefaultValue(0);
        }
    }
}