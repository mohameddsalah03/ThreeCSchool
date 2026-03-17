using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.DisplayName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(u => u.TimeZone)
                .HasMaxLength(100)
                .HasDefaultValue("Africa/Cairo");

            builder.Property(u => u.ProfilePicture)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            //Refresh Taken
            builder.Property(u => u.RefreshToken)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.RefreshTokenExpiryTime)
                .IsRequired(false);
            
            // OTP
            builder.Property(u => u.OtpCode)
                   .HasMaxLength(10)
                   .IsRequired(false);

            builder.Property(u => u.OtpExpiry)
                   .IsRequired(false);

            // One-to-One with Cart
            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many with Reviews
            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many with Courses (as Instructor)
            builder.HasMany(u => u.CoursesAsInstructor)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}