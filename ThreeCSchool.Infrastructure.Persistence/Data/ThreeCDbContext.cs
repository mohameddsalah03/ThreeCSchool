using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreeCSchool.Core.Domain.Models.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Data
{
    public class ThreeCDbContext : IdentityDbContext<ApplicationUser>
    {
        public ThreeCDbContext(DbContextOptions<ThreeCDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename Identity tables
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            // Remove unused Identity tables
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();

            // Apply all configurations from this assembly
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // ── Content ───────────────────────────────────────────
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        // ── Sessions & Booking ────────────────────────────────
        public DbSet<CourseSession> CourseSessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // ── Commerce ──────────────────────────────────────────
        public DbSet<PricingPlan> PricingPlans { get; set; }
        public DbSet<PlanFeature> PlanFeatures { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // ── Social ────────────────────────────────────────────
        public DbSet<Review> Reviews { get; set; }

        // ── Marketing (Standalone) ────────────────────────────
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Lead> Leads { get; set; }
    }
}
