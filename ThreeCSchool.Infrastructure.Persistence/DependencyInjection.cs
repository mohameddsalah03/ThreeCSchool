using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Data;
using ThreeCSchool.Infrastructure.Persistence.Data;
using ThreeCSchool.Infrastructure.Persistence.Repos;

namespace ThreeCSchool.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ThreeCDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MainContext"))
                );

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ThreeCDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDataSeeding, DataSeeding>();

            return services;
        }
    }
}