using ThreeCSchool.Core.Service.Abstraction.Services.Auth;
using ThreeCSchool.Core.Service.Abstraction.Services.Categories;

namespace ThreeCSchool.Core.Service.Abstraction.Services
{
    public interface IServiceManager
    {
        IAuthService AuthService { get; }
        ICategoryService CategoryService { get; }
    }
}