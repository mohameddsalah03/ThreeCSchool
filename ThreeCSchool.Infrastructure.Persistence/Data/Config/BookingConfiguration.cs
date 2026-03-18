using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Core.Domain.Models.Data.Enums;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.Status)
                .HasDefaultValue(BookingStatus.Confirmed);

            builder.Property(b => b.BookedAt).IsRequired();

            builder.Property(b => b.UserId)
                .HasMaxLength(450)
                .IsRequired();

            // Unique — نفس الطالب مينفعش يحجز نفس الجلسة مرتين
            builder.HasIndex(b => new { b.UserId, b.CourseSessionId })
                .IsUnique();

            builder.HasIndex(b => b.CourseSessionId);

            // Booking → User (restrict)
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}