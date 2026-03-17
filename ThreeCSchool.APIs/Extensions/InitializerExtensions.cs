using ThreeCSchool.Core.Domain.Contracts.Persistence;

namespace ThreeCSchool.APIs.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializeDbContextAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var seeder = services.GetRequiredService<IDataSeeding>();
            var logger = services.GetRequiredService<ILoggerFactory>()
                .CreateLogger("DbInitializer");

            try
            {
                await seeder.InitializeAsync();
                await seeder.DataSeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database initialization.");
            }

            return app;
        }
    }
}