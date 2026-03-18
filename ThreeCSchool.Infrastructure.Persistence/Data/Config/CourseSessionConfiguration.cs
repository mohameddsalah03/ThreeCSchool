using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSession>
    {
        public void Configure(EntityTypeBuilder<CourseSession> builder)
        {
            builder.ToTable("CourseSessions");

            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.StartTime).IsRequired();

            builder.Property(s => s.EndTime).IsRequired();

            builder.Property(s => s.MaxCapacity).IsRequired();

            builder.Property(s => s.CurrentBookings).HasDefaultValue(0);

            builder.Property(s => s.SessionLink)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(s => s.IsActive).HasDefaultValue(true);

            builder.Property(s => s.InstructorId)
                .HasMaxLength(450)
                .IsRequired();

            builder.HasIndex(s => s.CourseId);
            builder.HasIndex(s => s.InstructorId);

            // Session → Bookings (cascade)
            builder.HasMany(s => s.Bookings)
                .WithOne(b => b.CourseSession)
                .HasForeignKey(b => b.CourseSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Session → Instructor
            builder.HasOne(s => s.Instructor)
                .WithMany(u => u.SessionsAsInstructor)
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}