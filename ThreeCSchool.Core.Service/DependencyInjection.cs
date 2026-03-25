using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThreeCSchool.Core.Service.Abstraction.Services;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth.Email;
using ThreeCSchool.Core.Service.Abstraction.Services.Courses;
using ThreeCSchool.Core.Service.Mapping;
using ThreeCSchool.Core.Service.Services;
using ThreeCSchool.Core.Service.Services.Auth;
using ThreeCSchool.Core.Service.Services.Auth.Email;
using ThreeCSchool.Core.Service.Services.Courses;
using ThreeCSchool.Shared.Settings;

namespace ThreeCSchool.Core.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICourseService, CourseService>();




            return services;
        }
    }
}