using Microsoft.Extensions.DependencyInjection;
using ThreeCSchool.Core.Service.Abstraction.Services;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth;

namespace ThreeCSchool.Core.Service.Services
{
    internal class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;

        public ServiceManager(IServiceProvider serviceProvider)
        {
            _authService = new Lazy<IAuthService>(
                () => serviceProvider.GetRequiredService<IAuthService>());
        }

        public IAuthService AuthService => _authService.Value;
    }
}