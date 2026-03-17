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

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }
        public DbSet<PlanFeature> PlanFeatures { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}