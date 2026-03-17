using ThreeCSchool.Core.Service.Abstraction.Services.Auth;

namespace ThreeCSchool.Core.Service.Abstraction.Services
{
    public interface IServiceManager
    {
        IAuthService AuthService { get; }
    }
}